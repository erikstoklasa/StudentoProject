import React, { useState, useEffect } from 'react';
import apiAddress from './variables.js'
import SubjectTitle from './SubComponents/SubjectTitle'
import StudentGrades from './SubComponents/StudentGrades'
import StudentMaterial from './SubComponents/StudentMaterial'
import AddMaterialPopup from './SubComponents/addMaterialPopup'
import './SubjectDetail.css'
import moment from 'moment';
  
function SubjectDetail() {
    //initialize state
    const [subjectId, updateSubjectId] = useState();
    const [subjectInfo, updateSubjectInfo] = useState();
    const [studentAverage, updateAverage] = useState();   
    const [students, updateStudents] = useState();
    const [showMaterialPopup, updateShowMaterialPopup] = useState(false);
    const [material, updateMaterials] = useState();    

    //get subject instance id from url
    const determineSubjectID = () => { 
        const location = window.location.href
        const subjectId = location.split("Details?id=").pop()
        updateSubjectId(subjectId)
    }

    //format grades from internal to display value
    const getGradeDisplayValue = (grade) => {       
        if (grade == 110) {
            return '1+'
        }
        if (grade === 100) {
            return 1
        }
        if (grade === 90) { 
            return '1-'
        }
        if (grade === 85) {
            return '2+'
        }
        if (grade === 75) { 
            return 2
        }
        if (grade === 65) { 
            return '2-'
        }
        if (grade === 60) {
            return '3+'
        }
        if (grade === 50) { 
            return 3
        }
        if (grade === 40) { 
            return '3-'
        }
        if (grade === 35) {
            return '4+'
        }
        if (grade === 25) { 
            return 4
        }
        if (grade === 15) { 
            return '4-'
        }
        if (grade === 10) {
            return '5+'
        }
        if (grade === 0) { 
            return 5
        }        if (grade === -10) { 
            return '5-'
        }        
    }

    //calculate student average from internal value, then store it in state
    const calculateStudentAverage = (gradeData) => {
        let sum = 0;
        let gradeNum = gradeData.length;
        gradeData.forEach(grade => {
            sum = sum + parseInt(grade.value)
          
        });
        const average = sum / gradeNum      
        const formattedAverage = 5 - (average / 25)     
        updateAverage(formattedAverage)
    }
 
    
    // fetch grades, subject info and student material(in the future)
    const fetchData = () => {        
        if (subjectId) { 
            fetch(`${apiAddress}/SubjectInstances/Teacher/${subjectId}`, {
                method: 'GET',
                headers: {
                    'Cache-Control': 'no-cache'
                }
            })
                .then(res => res.json())
                .then(data => {                    
                    updateSubjectInfo(data)
                    updateStudents(data.students)
                }
            )
        }              
    }

    const fetchMaterials = () => {
        fetch(`${apiAddress}/SubjectMaterials/Teacher/Material?subjectInstanceId=${subjectId}`)
            .then(res => res.json())
            .then(data => {
                data.forEach(material => {
                    const linkText = material.fileExt.substring(1);
                    Object.assign(material, {
                        addedRelative: moment(material.added).locale('cs').fromNow(),
                        addedDisplay: moment(material.added).format("L"),
                        link: linkText
                    })
                })
                updateMaterials(data)
            })
    }

    const fetchGrades = () => {
        if (subjectId) {
            fetch(`${apiAddress}/Grades/Teacher?subjectInstanceId=${subjectId}`, {
                method: 'GET',
                headers: {
                    'Cache-Control': 'no-cache'
                }
            })
                .then(res => res.json())
                .then(data => {
                    
                    //format fetched grades data(add display value, add relative time using moment.js library)                   
                    calculateStudentAverage(data)
                    const gradesWithDisplayValue = data.map(grade => {
                        const displayValue = getGradeDisplayValue(parseInt(grade.value))
                        Object.assign(grade, { displayValue: displayValue })
                        return grade
                    })
                    assignGradesToStudents(gradesWithDisplayValue)
                })
        }
    }

    const assignGradesToStudents = (grades) => {
        if (students && grades) {
            const newStudents = students.map(student => {                
                const studentGrades = []
                let gradeSum = 0;
                let gradeNum = 0;
                
                grades.forEach(grade => {
                    if (student.id === grade.studentId) {
                        studentGrades.push(grade)
                        gradeSum = gradeSum + parseInt(grade.value)
                        gradeNum = gradeNum + 1
                    }                    
                })

                const studentAverage = 5 - ((gradeSum / gradeNum) / 25) 
                const newStudent = Object.assign(student, { grades: studentGrades })

                if (gradeNum > 0) {
                    Object.assign(newStudent, { average: studentAverage.toFixed(2) })
                } else {
                    Object.assign(newStudent, { average: ''})
                }

                return newStudent
            })
            updateStudents(newStudents)
        }
    }

    const uploadMaterials = (groupName, materials) => {   
        const materialList = [...materials]
        if (groupName) {
            fetch(`${apiAddress}/SubjectMaterials/Teacher/MaterialGroup?name=${groupName}`, {
                method: 'POST',
            })
                .then(res => res.json())
                .then(
                    data => {
                        materialList.forEach(material => {
                            Object.assign(material, { materialGroupId: data.id })
                        })
                        materialList.forEach(material => {
                            const formData = new FormData();
                            formData.append('FormFile', material.materialFile, material.materialFile.name)
                            formData.append('Material.Name', material.materialName)
                            formData.append('Material.Description', material.materialDescription)
                            formData.append('Material.SubjectInstanceId', subjectId)
                            formData.append('Material.SubjectMaterialGroupId', material.materialGroupId)                            
                            fetch(`${apiAddress}/SubjectMaterials/Teacher/Material`, {
                                method: 'POST',
                                body: formData
                            }).then(res => res.json())
                                .then(data => { fetchMaterials() })
                        })
                    })
        } else {            
            materialList.forEach(material => {                             
                const formData = new FormData();
                formData.append('FormFile', material.materialFile, material.materialFile.name)
                formData.append('Material.Name', material.materialName)
                formData.append('Material.Description', material.materialDescription)
                formData.append('Material.SubjectInstanceId', subjectId)                                          
                fetch(`${apiAddress}/SubjectMaterials/Teacher/Material`, {
                    method: 'POST',
                    body: formData
                }).then(res => res.json())
                    .then(data => { fetchMaterials()})
            })
        }
    }

    const deleteMaterial = (id) => {
        fetch(`${apiAddress}/SubjectMaterials/Teacher/Material?subjectMaterialId=${id}`, {
            method: 'DELETE'           
        }).then(res => res.json())
            .then(data => { fetchMaterials()})
    }

    const displayMaterialPopup = () => {
        updateShowMaterialPopup(true)
    }

    const hideMaterialPopup = () => {
        updateShowMaterialPopup(false)
    }
    
    //initialize effect hook chain
    useEffect(determineSubjectID, [])
    useEffect(fetchData, subjectId)
    useEffect(fetchMaterials, subjectId)
    useEffect(fetchGrades, students)
    

    //display everything
    return (
        <div>
            {subjectInfo && studentAverage ? <SubjectTitle info={subjectInfo} average={studentAverage} /> : null}            
            {students && subjectInfo?
                <div className="grades-material-container">
                    <StudentGrades students={students} info={subjectInfo} />
                    <StudentMaterial material={material} info={subjectInfo} deleteMaterial={deleteMaterial }showPopup={displayMaterialPopup}/>
                </div>                
            : null}
            <a href="/Teacher">Všechny předměty</a>
            { showMaterialPopup ? <AddMaterialPopup upload={uploadMaterials} hidePopup={ hideMaterialPopup}/> : null}
        </div>        
    );
}

export default SubjectDetail