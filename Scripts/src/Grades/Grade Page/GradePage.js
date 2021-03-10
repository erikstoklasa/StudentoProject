import React, { useState, useEffect} from 'react';
import StudentColumn from './SubComponents/StudentColumn.js';
import AverageColumn from './SubComponents/AverageColumn.js';
import NewGradeColumn from './SubComponents/NewGradeColumn.js';
import GradeDisplaySection from './SubComponents/GradeDisplaySection.js';
import FillerColumn from './SubComponents/FillerColumn.js'
import NotificationBar from './SubComponents/NotificationBar.js'
import apiAdress from './SubComponents/Variables'
import './GradePage.css';
const GradePage = () => {
    const [sortByAverage, updateSortByAverage] = useState(false);
    const [bulkGradeData, updateBulkGradeData] = useState([]);
    const [formattedStudentData, updateFormattedStudentData] = useState()    
    const [bigAverage, updateBigAverage] = useState([]);
    const [bulkStudentData, updateBulkStudentData] = useState([]);
    const [InstanceId, updateInstanceId] = useState();
    const [orderedStudents, updateOrderedStudents] = useState();
    const [orderedGrades, updateOrderedGrades] = useState();
    const [notificationData, updateNotificationData] = useState({
        show: false,
        text: '',
    });
    const newGrades = [];

    const getInstanceId = () => {       
        const idContainer = document.querySelector("#subjectInstanceId")
        updateInstanceId(idContainer.value);
    }       

    const fetchData = () => {
        if (InstanceId) {
            fetch(`${apiAdress}/Grades/Teacher?subjectInstanceId=${InstanceId}`, {
                method: 'GET',
                headers: {
                    'Cache-Control': 'no-cache'
                }
            }).then(res => res.json()).then(data => {
                data.forEach(grade => { 
                    const displayValue = getGradeDisplayValue(parseInt(grade.value))
                    Object.assign(grade, { displayValue: displayValue })
                })                           
                updateBulkGradeData(data)
            })

            fetch(`${apiAdress}/SubjectInstances/Teacher/${InstanceId}`, {
                method: 'GET',
            }).then(res => res.json()).then(data => { 
                
                updateBulkStudentData(data)                
            }) 
        
        }
    }

    const sortStudents = () => {      
        if (formattedStudentData && sortByAverage === false) {           
            if (formattedStudentData.length > 0) {
                updateOrderedStudents(formattedStudentData)
            }
        }
        if (formattedStudentData && sortByAverage === true) {           
            const studentArray = [...formattedStudentData]
            studentArray.sort((a, b) => (a.average > b.average) ? 1 : -1)         
            let studentsWithAverage = [];
            let studentsWithoutAverage = [];
            studentArray.forEach(student => {
                if (student.average === '') {
                    studentsWithoutAverage.push(student)
                } else {
                    studentsWithAverage.push(student)
                }
            })
            const finalStudentArray = studentsWithAverage.concat(studentsWithoutAverage);
            updateOrderedStudents(finalStudentArray)
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
            console.log('sum ' + gradeSum)
            console.log('num ' + gradeNum)
            const average =  5 - ((gradeSum / gradeNum) / 25);
            updateBigAverage(average)
        }
    }

    const calculateAverages = () => {   
        if (bulkStudentData.students && bulkGradeData.length > 0) {
            const newStudentData = [...bulkStudentData.students.map((student, index) => {
                let total = 0;
                let gradeNum = 0;
                    
                bulkGradeData.forEach(grade => {
                    if (grade.studentId === student.id) {
                        total = total + grade.value
                        gradeNum = gradeNum + 1
                    }
                })
    
                const average = 5 - (total / gradeNum) / 25
                const formatedAvearage = average.toFixed(2)
            
                if (!isNaN(formatedAvearage)) {
                    Object.assign(student, { average: formatedAvearage })
        
                    return student
                }
                else {
                    Object.assign(student, { average: '' })
                    return student
                }
            
            })]
            updateFormattedStudentData(newStudentData)
        }
        else if (bulkStudentData.students) {
            const newStudentData = [...bulkStudentData.students];
            newStudentData.forEach(student => {
                Object.assign(student, {average: ''})
            })
            updateFormattedStudentData(newStudentData)
        }
        
    }

    useEffect(getInstanceId, [])
    useEffect(fetchData, [InstanceId])
    useEffect(calculateAverages, [bulkGradeData, bulkStudentData])
    useEffect(sortStudents, [formattedStudentData, sortByAverage])
    useEffect(sortGrades, [bulkGradeData])

    const getGradeDisplayValue = (grade) => {        
        if (grade === 110) {
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
  
  
    const modifyGrade = (gradeId, gradeValue, studentId, gradeName) => {        
        if (gradeId) {
            if (gradeValue === 0) {
                const gradeArr = [gradeId]
                fetch(`${apiAdress}/Grades/Teacher/Batch`, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    body: JSON.stringify(gradeArr)
                }).then(res => {
                    if (res.ok) {
                        updateBulkGradeData(bulkGradeData.filter(grade => gradeId !== grade.id))
                        renderNotificationBar()
                    }
                })
            }
            else {                
                const reqBody = {
                    id: gradeId,
                    value: 100 - ((gradeValue * 25) - 25),
                    subjectInstanceId: InstanceId,
                    studentId: studentId,
                    name: gradeName
                }
               
                fetch(`${apiAdress}/Grades/Teacher`, {
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
                                const displayValue = getGradeDisplayValue(parseInt(data.value))
                                Object.assign(data, {displayValue : displayValue})
                                newGrades.push(data)
                            }
                            else {
                                newGrades.push(grade)
                            }
                        })
                        updateBulkGradeData(newGrades)
                        renderNotificationBar()
                    }              
                }).catch(err => {})
            }
        } else if (!gradeId) {          
            
            const reqBody = {
                value: 100 - ((gradeValue * 25) - 25),
                subjectInstanceId: InstanceId,
                studentId: studentId,
                name: gradeName
            }           

            fetch(`${apiAdress}/Grades/Teacher`, {
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
                const displayValue = getGradeDisplayValue(parseInt(data.value))
                Object.assign(data, {displayValue : displayValue})
                array = [...bulkGradeData, data]    
                return array
            }).then(array => {
                updateBulkGradeData(array)
                renderNotificationBar()
            }).catch()
        }
    }

    const trackNewGradeValues = (grade, id) => {
        const newGrade = {
            value: 100 - ((grade * 25) - 25),
            subjectInstanceId: InstanceId,
            studentId: id,
        }

        if (!newGrades.some(e => e.studentId === newGrade.studentId )) {
            newGrades.push(newGrade)
        }

        else {
            newGrades.forEach(grade => {
                if (grade.studentId === newGrade.studentId) {
                    grade.value = 100 - (newGrade.value * 25);
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
        
        fetch(`${apiAdress}/Grades/Teacher/Batch`, {
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
            data.forEach(grade => {
                const displayValue = getGradeDisplayValue(parseInt(grade.value))
                Object.assign(grade, {displayValue : displayValue})                
            })
            let array;
            array = [...bulkGradeData, ...data]    
            updateBulkGradeData(array)
            renderNotificationBar()
        })   
    }

    const onClickHeader = () => {
        updateSortByAverage(!sortByAverage)
    }

    const renderNotificationBar = () => {

        updateNotificationData({
            show:true
        })

        setTimeout(unrenderNotification, 3000)
    }

    const unrenderNotification = () => {        
        updateNotificationData({show:false})
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
                <NotificationBar data={notificationData} />
                <div className="grade-table-container">
                    {(orderedStudents ? <StudentColumn students={orderedStudents} /> : null)}
                    {(orderedStudents && bulkGradeData ? <AverageColumn students={orderedStudents} onClickHeader={onClickHeader} /> : null)}
                    {(orderedStudents ? <NewGradeColumn students={orderedStudents} trackNewGradeValues={trackNewGradeValues} removeNewGrade={removeNewGrade} handleSubmitNewGrades={handleSubmitNewGrades} /> : null)}
                    {(orderedStudents && orderedGrades && bulkGradeData ? <GradeDisplaySection orderedGrades={orderedGrades} orderedStudents={orderedStudents} bulkGradeData={bulkGradeData} modifyGrade={modifyGrade} /> : null)}
                    {(orderedStudents ? <FillerColumn students={orderedStudents} /> : null)}
                  
                </div>
               
            </div>
        )
    }

}


export default GradePage