import React, { useState, useEffect, useRef } from 'react';
import StudentColumn from './SubComponents/StudentColumn.js';
import AverageColumn from './SubComponents/AverageColumn.js';
import NewGradeColumn from './SubComponents/NewGradeColumn.js';
import GradeDisplaySection from './SubComponents/GradeDisplaySection.js';
import FillerColumn from './SubComponents/FillerColumn.js'
import apiAdress from './SubComponents/Variables'
import './GradePage.css';

const GradePage = () => {
    const [bulkGradeData, updateBulkGradeData] = useState([]);
    const [sortByAverage, updateSortByAverage] = useState(false);
    const [bigAverage, updateBigAverage] = useState([]);
    const [bulkStudentData, updateBulkStudentData] = useState([]);
    const [InstanceId, updateInstanceId] = useState();
    const [orderedStudents, updateOrderedStudents] = useState();
    /*const [orderedStudentsByAverage, updateOrderedByAverage] = useState()*/
    const [orderedGrades, updateOrderedGrades] = useState();
    const newGrades = [];

    const getInstanceId = () => {
        const idContainer = document.querySelector("#subjectInstanceId")
        updateInstanceId(idContainer.value);
    }   
    

    const fetchData = () => {
        if (InstanceId) {
            fetch(`${apiAdress}/Grades?subjectInstanceId=${InstanceId}`, {
                method: 'GET',
            }).then(res => res.json()).then(data => updateBulkGradeData(data))

            fetch(`${apiAdress}/SubjectInstances/${InstanceId}`, {
                method: 'GET',
            }).then(res => res.json()).then(data => {                
               
                updateBulkStudentData(data)
            }) 
        
        }
    }

    const sortStudents = () => {
        if (bulkStudentData.students && !sortByAverage) {
        const studentArray = bulkStudentData.students;            
        studentArray.sort((a, b) => a.lastName.localeCompare(b.lastName))
            if (studentArray.length > 0) {
                updateOrderedStudents(studentArray)
            }
        }      
        
    }

    const sortGrades = () => {
        if (bulkGradeData.length === 0) {
            updateOrderedGrades([])
        }

        if (bulkGradeData.length > 0) {
            const studentGrades = [];
            const sortedGrades = bulkGradeData;
            let gradeSum = 0;
            const gradeNum = bulkGradeData.length;

                   
            sortedGrades.sort((a, b) => { 
                if (a.added > b.added) { 
                    return 1
                }
                else {
                    return -1
                }

            })

            sortedGrades.forEach((grade) => {
                if (!studentGrades.some(g => g.name === grade.name)) {
                    studentGrades.push(grade)
                }
                gradeSum = gradeSum + grade.value
            })
           
            studentGrades.reverse();
            updateOrderedGrades(studentGrades)
            const average = (gradeSum / gradeNum).toFixed(2);
            updateBigAverage(average)
        }
    }

  /*  const calculateAverages = () => {   
      
       if (bulkStudentData.students && bulkGradeData.length > 0) {
            const newStudentData = bulkStudentData.students.map((student, index) => {
                let total = 0;
                let gradeNum = 0;
                    
                bulkGradeData.forEach(grade => {
                    if (grade.studentId === student.id) {
                        total = total + grade.value
                        gradeNum = gradeNum + 1
                    }
                })
    
                let formatedAvearage = (total / gradeNum).toFixed(2)
            
                if (!isNaN(formatedAvearage)) {
                    Object.assign(student, { average: formatedAvearage })
        
                    return student
                }
                else {
                    Object.assign(student, { average: '' })
                    return student
                }
            
            })
           newStudentData.sort()
           updateBulkStudentData(newStudentData)
        }
        
    }*/

    useEffect(getInstanceId)
    useEffect(fetchData, [InstanceId])
    useEffect(sortStudents, [bulkStudentData, sortByAverage])
    useEffect(sortGrades, [bulkGradeData])
   /* useEffect(calculateAverages, [bulkGradeData])*/
  
    const modifyGrade = (gradeId, gradeValue, studentId, gradeName) => {
        if (gradeId) {
            if (gradeValue === 0) {
                const gradeArr = [gradeId]
                fetch(`${apiAdress}/Grades/Batch`, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    body: JSON.stringify(gradeArr)
                }).then(res => {
                    if (res.ok) {
                        updateBulkGradeData(bulkGradeData.filter(grade => gradeId !== grade.id))
                    }
                }).then()
            }
            else {
                const reqBody = {
                    id: gradeId,
                    value: gradeValue,
                    subjectInstanceId: InstanceId,
                    studentId: studentId,
                    name: gradeName
                }
                fetch(`${apiAdress}/Grades`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    body: JSON.stringify(reqBody)
                }).then(res => {
                    if (res.ok) {
                        return res.json()
                    }
                }).then(data => {
                    const newGrades = [];
                    if (data.id) {
                        bulkGradeData.forEach(grade => {
                            if (grade.id === data.id) {
                                newGrades.push(data)
                            }
                            else {
                                newGrades.push(grade)
                            }
                        })
                        updateBulkGradeData(newGrades)
                    }              
                }).catch(err => {})
            }
        } else if (!gradeId) {
            
            const reqBody = {
                value: gradeValue,
                subjectInstanceId: InstanceId,
                studentId: studentId,
                name: gradeName
            }

            fetch(`${apiAdress}/Grades`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(reqBody)
            }).then().then(res => {
                if (res.ok) {
                    return res.json()
                }
            }).then(data => {
                let array;
                array = [...bulkGradeData, data]    
                return array
            }).then(array => {
               updateBulkGradeData(array)                            
            }).then().catch()


        }
    }

    const trackNewGradeValues = (grade, id) => {
        const newGrade = {
            value: grade,
            subjectInstanceId: InstanceId,
            studentId: id,
        }

        if (!newGrades.some(e => e.studentId === newGrade.studentId )) {
            newGrades.push(newGrade)
        }

        else {
            newGrades.forEach(grade => {
                if (grade.studentId === newGrade.studentId) {
                    grade.value = newGrade.value;
                }
            })
        }
    }

    const removeNewGrade = (studentId) => {
        newGrades.splice(newGrades.findIndex(grade => grade.studentId === studentId), 1)
    }

    const handleSubmitNewGrades = (newGradeName) => {
        newGrades.forEach(grade => {
            Object.assign(grade, {name: newGradeName})
        })
        fetch(`${apiAdress}/Grades/Batch`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
                // 'Content-Type': 'application/x-www-form-urlencoded',
              },
            body: JSON.stringify(newGrades)
        }).then(res => {            
            if (res.ok) {
               return res.json()
            }
        }).then(data => {

            let array;
            array = [...bulkGradeData, ...data]
    
            updateBulkGradeData(array)

        })
        
   
    }

    const onClickHeader = () => {
        updateSortByAverage(!sortByAverage)
    }
    
    if (!orderedStudents) { 
        return (
            <div>
                <div className="subject-info">
                    {(bulkStudentData ? <h1 className="subject-heading">{bulkStudentData.name}</h1> : null)}
                    {(bulkGradeData ? <h2 className="subject-average-text">Průměr: <span className="average-header-number">{bigAverage}</span></h2> : null)}
                    {(bulkStudentData.students ? <div>{`${bulkStudentData.students.length} studentů`}</div> : null)}
                </div>
                      
           
                <div className="empty-grade-table">
                    <div>Zatím žádní studenti</div>                  
                </div>
            </div>
        )
    }
    else {
        return (
            <div>
                <div className="subject-info">
                    {(bulkStudentData ? <h1 className="subject-heading">{bulkStudentData.name}</h1> : null)}
                    {(bulkGradeData ? <h2 className="subject-average-text">Průměr: <span className="average-header-number">{bigAverage}</span></h2> : null)}
                    {(bulkStudentData.students ? <div>{`${bulkStudentData.students.length} studentů`}</div> : null)}
                </div>
                      
           
                <div className="grade-table-container">
                    {(orderedStudents ? <StudentColumn students={orderedStudents} /> : null)}
                    {(orderedStudents && bulkGradeData ? <AverageColumn students={orderedStudents} grades={bulkGradeData} onClickHeader={onClickHeader} /> : null)}
                    {(orderedStudents ? <NewGradeColumn students={orderedStudents} trackNewGradeValues={trackNewGradeValues} removeNewGrade={removeNewGrade} handleSubmitNewGrades={handleSubmitNewGrades} /> : null)}
                    {(orderedStudents && orderedGrades && bulkGradeData ? <GradeDisplaySection orderedGrades={orderedGrades} orderedStudents={orderedStudents} bulkGradeData={bulkGradeData} modifyGrade={modifyGrade} /> : null)}
                    {(orderedStudents ? <FillerColumn students={orderedStudents} /> : null)}
                </div>
            </div>
        )
    }

}


export default GradePage