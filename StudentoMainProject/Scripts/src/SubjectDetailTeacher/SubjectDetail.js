import React, { useState, useEffect } from 'react';
import apiAddress from './variables.js'
import SubjectTitle from './SubComponents/SubjectTitle'
import StudentGrades from './SubComponents/StudentGrades'
import StudentMaterial from '../../Components/StudentMaterial/StudentMaterial'
import LoadingScreen from '../../Components/Loading/LoadingScreen.js';
import styled from 'styled-components'

const Container = styled.div` 
    display: flex;         
    justify-content: space-between;
    gap: 20px;
    @media(max-width: 1120px){   
        flex-wrap: wrap;
    }
`

const SubjectDetail = () => {
    //initialize state
    const [subjectId, updateSubjectId] = useState(window.location.href.split("Details?id=").pop());
    const [subjectInfo, updateSubjectInfo] = useState();
    const [studentAverage, updateAverage] = useState();   
    const [students, updateStudents] = useState();     

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
    
    //initialize effect hook chain    
    useEffect(fetchData, subjectId)    
    useEffect(fetchGrades, students)   

    //display everything
    return (
        <>
            {subjectInfo && studentAverage ? <SubjectTitle info={subjectInfo} average={studentAverage} /> : null}            
            {students && subjectInfo?
                <Container>
                    <StudentGrades students={students} info={subjectInfo} />
                    <StudentMaterial/>
                </Container>                
            : <LoadingScreen/>}
            <a href="/Teacher">Všechny předměty</a>            
        </>        
    );
}

export default SubjectDetail