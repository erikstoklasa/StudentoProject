import React, { useState, useEffect } from 'react';
import apiAddress from './variables.js'
import SubjectTitle from './SubComponents/SubjectTitle'
import StudentGrades from './SubComponents/StudentGrades'
import StudentMaterial from './SubComponents/StudentMaterial'
import AddGradePopup from './SubComponents/AddGradePopup'
import AddMaterialPopup from './SubComponents/addMaterialPopup'
import './SubjectDetail.css'
import moment from 'moment';

  
function SubjectDetail() {
    //initialize state
    const [subjectId, updateSubjectId] = useState(window.location.href.split("Details?id=").pop());
    const [subjectInfo, updateSubjectInfo] = useState();
    const [studentAverage, updateAverage] = useState();
    const [grades, updateGrades] = useState();
    const [showMaterialPopup, updateShowMaterialPopup] = useState(false);
    const [material, updateMaterials] = useState();
    const [showAddPopup, updateShowAddPopup] = useState(false)

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
        } if (grade === -10) {
            return '5-'
        }
    }

    //calculate student average from internal value, then store it in state
    const calculateStudentAverage = (gradeData) => {       
        if (gradeData) {
            let sum = 0;
            let gradeNum = gradeData.length;
            gradeData.forEach(grade => {
                sum = sum + parseInt(grade.value)
          
            });
            const average = sum / gradeNum
            const formattedAverage = 5 - (average / 25)
            updateAverage(formattedAverage)
        }
        else if (grades) {
            let sum = 0;
            let gradeNum = grades.length;
            grades.forEach(grade => {
                sum = sum + parseInt(grade.value)
          
            });
            const average = sum / gradeNum
            const formattedAverage = 5 - (average / 25)
            updateAverage(formattedAverage)
        }
    }

    const getInternalGradeValue = (displayValue) => {
        if (displayValue === '1*') return 110
        if (displayValue === '1+') return 110
        if (displayValue === '1') return 100
        if (displayValue === '1-') return 90
        if (displayValue === '2+') return 85
        if (displayValue === '2') return 75
        if (displayValue === '2-') return 65
        if (displayValue === '3+') return 60
        if (displayValue === '3') return 50
        if (displayValue === '3-') return 40
        if (displayValue === '4+') return 35
        if (displayValue === '4') return 25
        if (displayValue === '4-') return 15
        if (displayValue === '5+') return 10
        if (displayValue === '5') return 0
        if (displayValue === '5-') return -10
        
    }
 

    const fetchMaterials = () => {
       
        fetch(`${apiAddress}/SubjectMaterials/Student/Material?subjectInstanceId=${subjectId}`)
            .then(res => res.json())
            .then(data => {
                data.forEach(material => {
                    const linkText = material.fileExt.substring(1);
                    Object.assign(material, {
                        addedRelative: moment.utc(material.added).locale('cs').fromNow(),
                        addedDisplay: moment.utc(material.added).format("D.M.Y"),
                        link: linkText
                    })
                })
                if (data.length > 0) {
                    updateMaterials(data)
                } else {
                    updateMaterials(null)
                }                
            })
    }

    // fetch grades, subject info and student material(in the future)
    const fetchData = () => {
        
        if (subjectId) {
            fetch(`${apiAddress}/SubjectInstances/Student/${subjectId}`, {
                method: 'GET',
                headers: {
                    'Cache-Control': 'no-cache'
                }
            })
                .then(res => res.json())
                .then(data => updateSubjectInfo(data))
        }
        if (subjectId) {
            fetch(`${apiAddress}/Grades/Student?subjectInstanceId=${subjectId}`, {
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
                    gradesWithDisplayValue.forEach(grade => {
                        Object.assign(grade, { addedRelative: moment.utc(grade.added).locale('cs').fromNow() })
                        Object.assign(grade, { addedDisplay: moment.utc(grade.added).format("D.M.Y") })
                    })
                    updateGrades(gradesWithDisplayValue)
                })
        }
    }
    
    //initialize effect hook chain   
    useEffect(fetchData, subjectId)   
    useEffect(fetchMaterials, subjectId)
    useEffect(calculateStudentAverage, [grades])

    //update state to display add grade popup
    const showPopup = () => {
        updateShowAddPopup(true)
    }

    //update state to hide add grade popup
    const hidePopup = () => {
        updateShowAddPopup(false)
    }

    const uploadMaterials = (groupName, materials) => {
        const materialList = [...materials]
        if (groupName) {
            fetch(`${apiAddress}/SubjectMaterials/Student/MaterialGroup?name=${groupName}`, {
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
                            fetch(`${apiAddress}/SubjectMaterials/Student/Material`, {
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
                
                fetch(`${apiAddress}/SubjectMaterials/Student/Material`, {
                    method: 'POST',
                    body: formData
                }).then(res => res.json())
                    .then(data => { fetchMaterials() })
            })
        }
    }

    const deleteMaterial = (id) => {
        fetch(`${apiAddress}/SubjectMaterials/Student/Material?subjectMaterialId=${id}`, {
            method: 'DELETE'
        }).then(res => {
            if (res.ok) {
                fetchMaterials()
            }
        }) 
        
    }

    const displayMaterialPopup = () => {
        updateShowMaterialPopup(true)
    }

    const hideMaterialPopup = () => {
        updateShowMaterialPopup(false)
    }

    //send a request to post student grade
    const addStudentGrade = (name, value) => {
        const actualValue = getInternalGradeValue(value)
        fetch(`${apiAddress}/Grades/Student`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                value: actualValue,
                name: name,
                subjectInstanceId: subjectId
            })
        }).then(res => {
            if (res.ok) {
                return res.json()
            }
        }
        ).then(
            data => {
                if (data) {
                    Object.assign(data, {
                        displayValue: getGradeDisplayValue(parseInt(data.value)),
                        addedRelative: moment.utc(data.added).locale('cs').fromNow(),
                        addedDisplay: moment.utc(data.added).format("D.M.Y")
                    })
                    const newArr = [data, ...grades]                   
                    updateGrades(newArr)
                  
                }
            }
        )
    }

    const deleteStudentGrade = (id) => {
        const reqBody = [id];
        fetch(`${apiAddress}/Grades/Student/Batch`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(reqBody)
        })
            .then(res => {
                if (res.ok) {
                    const newArr = grades.filter(grade => grade.id != id)
                    updateGrades(newArr)                   
                }
            })
        
    }
    



       //display everything
    return (
        <div>
            {subjectInfo ? <SubjectTitle info={subjectInfo} average={studentAverage} /> : null}
        
            {grades && subjectInfo ?
                <div className="grades-material-container">
                    <StudentGrades grades={grades} info={subjectInfo} showPopup={showPopup} deleteGrade={deleteStudentGrade} />
                    <StudentMaterial material={material} showPopup={displayMaterialPopup} deleteMaterial={deleteMaterial} />
                    {showAddPopup ? <AddGradePopup addGrade={addStudentGrade} hidePopup={hidePopup} /> : null}
                </div>
                : null}
            { showMaterialPopup ? <AddMaterialPopup upload={uploadMaterials} hidePopup={hideMaterialPopup} /> : null}
           
        </div>

    );
}

export default SubjectDetail
