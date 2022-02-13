import React, { useState, useEffect} from 'react';
import StudentColumn from './SubComponents/StudentColumn.js';
import AverageColumn from './SubComponents/AverageColumn.js';
import NewGradeColumn from './SubComponents/NewGradeColumn.js';
import GradeDisplaySection from './SubComponents/GradeDisplaySection.js';
import FillerColumn from './SubComponents/FillerColumn.js'
import NotificationBar from './SubComponents/NotificationBar.js'
import apiAdress from './SubComponents/Variables'
import moment from 'moment';
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
    const [newGrades, updateNewGrades] =useState([])  

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
            updateBigAverage('Zatím žádné známky')
        }

        if (bulkGradeData.length > 0) {
            const studentGrades = [];
            const sortedGrades = bulkGradeData;
            let gradeSum = 0;
            const gradeNum = bulkGradeData.length;
            
            sortedGrades.sort((a, b) => {               

                return b.gradeGroupAdded.localeCompare(a.gradeGroupAdded)
  

            })

            sortedGrades.forEach((grade) => {
                if (!studentGrades.some(g => g.gradeGroupId === grade.gradeGroupId)) {
                    studentGrades.push(grade)
                }
                gradeSum = gradeSum + parseInt(grade.value)
            })


           
            studentGrades.reverse();
            updateOrderedGrades(studentGrades)            
            const average =  5 - ((gradeSum / gradeNum) / 25);
            updateBigAverage(average.toFixed(2))
        }
    }

    const calculateAverages = () => {        
       
        if (bulkStudentData.students && bulkGradeData.length > 0) {
            const newStudentData = [...bulkStudentData.students.map((student) => {
                let total = 0;
                let gradeNum = 0;
                    
                bulkGradeData.forEach(grade => {                   
                    
                    if (grade.studentId === student.id) {
                       
                        total = total + parseInt(grade.value)*grade.gradeGroupWeight
                        
                        gradeNum = gradeNum + 1*grade.gradeGroupWeight
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
    
    const getInternalGradeValue = (displayValue) => {
        
        if (displayValue === '1+') return 110
        if (displayValue === '1*') return 110
        if (displayValue === '1') return 100
        if (displayValue === '1-') return 90
        if (displayValue === '2+') return 85
        if (displayValue === '2') return 75
        if (displayValue === '2-') return 65
        if(displayValue === '3+') return 60
        if (displayValue === '3') return 50
        if (displayValue === '3-') return 40
        if (displayValue === '4+') return 35
        if (displayValue === '4') return 25
        if (displayValue === '4-') return 15
        if (displayValue === '5+') return 10
        if (displayValue === '5') return 0
        if (displayValue === '5-') return -10
        
    }
  
    const modifyGrade = (gradeId, gradeValue, studentId, gradeName, grade, gradeGroupId) => {

        const checkIfBulkContains = (grade) => {
            let result = false
            bulkGradeData.forEach(bulkGrade => {
                if (grade.gradeGroupId === bulkGrade.gradeGroupId) {
                    if (grade.id !== bulkGrade.id) {
                        result = true
                    }
                }
            })
            return result
        }
     
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
                        
                        
                        if (!checkIfBulkContains(grade)) {                      
                            
                            fetch(`${apiAdress}/Grades/Teacher/GradeGroup/Batch`, {
                                method: 'DELETE',
                                headers: {
                                    'Content-Type': 'application/json'
                                    // 'Content-Type': 'application/x-www-form-urlencoded',
                                },
                                body: JSON.stringify([grade.gradeGroupId]) 
                            })
                        }
                    }
                })
            }
            else {               
                const reqBody = {
                    id: gradeId,
                    value: getInternalGradeValue(gradeValue),
                    subjectInstanceId: InstanceId,
                    studentId: studentId,
                    name: gradeName,
                    added: moment().toISOString(),
                    gradeGroupId: grade.gradeGroupId,                
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
                    const newResponseGrades = [];                 
                    if (data.id) {
                        bulkGradeData.forEach(grade => {                            
                            if (grade.id === data.id) {                                
                                const displayValue = getGradeDisplayValue(parseInt(data.value))
                                Object.assign(grade, { displayValue: displayValue })
                                Object.assign(grade, reqBody)
                             
                                newResponseGrades.push(grade)
                            }
                            else {
                                newResponseGrades.push(grade)
                            }
                        })
                        updateBulkGradeData(newResponseGrades)
                        renderNotificationBar()
                    }              
                }).catch(err => {})
            }
        } else if (!gradeId) {
            
            
            const reqBody = {
                value: getInternalGradeValue(gradeValue),
                subjectInstanceId: InstanceId,
                studentId: studentId,
                name: gradeName,
                gradeGroupId: gradeGroupId,
                gradeGroupAdded: grade.gradeGroupAdded,
                weight: grade.gradeGroupWeight,
                added: grade.gradeGroupAdded,
                gradeGroupWeight: grade.gradeGroupWeight
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
                Object.assign(data, { displayValue: displayValue, gradeGroupName: grade.gradeGroupName, gradeGroupAdded: grade.gradeGroupAdded, gradeGroupWeight: grade.gradeGroupWeight })           
                array = [...bulkGradeData, data]    
                return array
            }).then(array => {
                updateBulkGradeData(array)
                renderNotificationBar()
            }).catch()
        }
    }

    const modifyGradeGroup = (id, name, weight, grade) => {
        fetch(`${apiAdress}/Grades/Teacher/GradeGroup`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify({
                id: id,
                weight: weight,
                name: name,
                added: grade.added,
                addedBy: 1
            })
        })
        .then((res) => {
            if (res.ok) {
                return res.json()
            }
        })
            .then((data) => {
                const newGrades = [...bulkGradeData]
                newGrades.forEach(grade => {
                    if (grade.gradeGroupId === data.id) {
                        Object.assign(grade, {
                            gradeGroupName: name,
                            gradeGroupWeight: weight
                        })
                    }
                })
                updateBulkGradeData(newGrades)
        })
    }

    const deleteGradeGroup = async (id) => {
        console.log(id)
        const idList = []
        bulkGradeData.forEach(gradeObj => {
            if (gradeObj.gradeGroupId === id) {
                idList.push(gradeObj.id)
            }
        })
       
       
        const response = await fetch(`${apiAdress}/Grades/Teacher/Batch`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(idList)
        })
        if (response.ok) {            
            const newBulkGrades = bulkGradeData.filter(gradeObj => gradeObj.gradeGroupId !== id)
            updateBulkGradeData(newBulkGrades)
            const response = await fetch(`${apiAdress}/Grades/Teacher/GradeGroup/Batch`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                    // 'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: JSON.stringify([id])
            })
        }
    }

    const trackNewGradeValues = (grade, id) => { 

        const newGrade = {
            value: getInternalGradeValue(grade),
            subjectInstanceId: InstanceId,
            studentId: id,
        }        

        if (!newGrades.some(e => e.studentId === newGrade.studentId)) {
            const newArr = [...newGrades, newGrade]            
            updateNewGrades(newArr)            
           
        } else {
            const newArr = [...newGrades]
            newArr.forEach(gradeObj => {
                if (gradeObj.studentId === newGrade.studentId) {
                   
                    Object.assign(gradeObj, {value: getInternalGradeValue(grade)})
                    
                }
            })
            
            updateNewGrades(newArr)
        }
    }

    const assignDefaultNewGrades = (defaultGrade) => {
        const defaultGradeArr = orderedStudents.map(student => {
            return ({
                value: getInternalGradeValue(defaultGrade),
                subjectInstanceId: InstanceId,
                studentId: student.id,
            })
        })
        updateNewGrades(defaultGradeArr)
    }

    const removeNewGrade = (studentId) => {
        const newArr = [...newGrades]
        newArr.splice(newGrades.findIndex(grade => grade.studentId === studentId), 1)
        updateNewGrades(newArr)
    }

    const handleSubmitNewGrades = (newGradeName, newGradeWeight) => {           
            const newArr = [...newGrades]
            newArr.forEach(grade => {
                Object.assign(grade, { name: newGradeName})
            })
            if (newArr.length > 0) {
                fetch(`${apiAdress}/Grades/Teacher/GradeGroup`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    body: JSON.stringify({
                        weight: newGradeWeight,
                        name: newGradeName,                        
                        added: moment().toISOString(),
                    })
                })
                .then((res) => {
                        if (res.ok) {
                            return res.json()
                        }
                    }
                ).then(data => {                    
                    newArr.forEach(grade => {
                        Object.assign(grade, {
                            gradeGroupId: data.id,
                            added: moment().toISOString()                            
                        })
                    })
                    fetch(`${apiAdress}/Grades/Teacher/Batch`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                            // 'Content-Type': 'application/x-www-form-urlencoded',
                        },
                        body: JSON.stringify(newArr)
                    }).then(res => {
                        if (res.ok) {
                            return res.json()
                        }
                    }).then(data => {
                        data.forEach(grade => {
                            const displayValue = getGradeDisplayValue(parseInt(grade.value))                            
                            Object.assign(grade, { displayValue: displayValue, gradeGroupName : newGradeName, gradeGroupWeight: newGradeWeight, gradeGroupAdded: moment().toISOString(), })
                        })
                        let array;
                        array = [...bulkGradeData, ...data]
                        
                        updateBulkGradeData(array)
                        updateNewGrades([])
                        renderNotificationBar()                       
                    })
                    
                })
                } else {
                    return 'failed'
                }       
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
                    {(orderedStudents ? <NewGradeColumn students={orderedStudents} trackNewGradeValues={trackNewGradeValues} removeNewGrade={removeNewGrade} handleSubmitNewGrades={handleSubmitNewGrades} assignDefaults={assignDefaultNewGrades}/> : null)}
                    {(orderedStudents && orderedGrades && bulkGradeData ? <GradeDisplaySection orderedGrades={orderedGrades} orderedStudents={orderedStudents} bulkGradeData={bulkGradeData} modifyGrade={modifyGrade} modifyGradeGroup={modifyGradeGroup} deleteGradeGroup={deleteGradeGroup}/>: null)}
                    {(orderedStudents ? <FillerColumn students={orderedStudents} /> : null)}
                </div>
           
            </div>
        )
    }
}


export default GradePage