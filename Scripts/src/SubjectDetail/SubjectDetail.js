import React, { useState, useEffect } from 'react';
import apiAddress from './variables.js'
import SubjectTitle from './SubComponents/SubjectTitle'
import StudentGrades from './SubComponents/StudentGrades'
import StudentMaterial from './SubComponents/StudentMaterial'
import './SubjectDetail.css'
import moment from 'moment';

  
function SubjectDetail() {
    //initialize state
    const [subjectId, updateSubjectId] = useState();
    const [subjectInfo, updateSubjectInfo] = useState();
    const [studentAverage, updateAverage] = useState();
    const [grades, updateGrades] = useState();
    const [material, updateMaterial] = useState();

    //get subject instance id from url
    const determineSubjectID = () => { 
        const location = window.location.href
        const subjectId = location.split("Details?id=").pop()
        updateSubjectId(subjectId)
    }

    //format grades from internal to display value
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

    //calculate student average from internal value, then store it in state
    const calculateStudentAverage = (gradeData) => {
        let sum = 0;
        let gradeNum = gradeData.length;
        gradeData.forEach(grade => {
            sum = sum + parseInt(grade.value)
            gradeNum = gradeNum++
        });
        const average = sum / gradeNum      
        const formattedAverage = 5 - (average / 25)
        updateAverage(formattedAverage)
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
                        Object.assign(grade, {addedRelative: moment(grade.added).locale('cs').fromNow()})                     
                    })
                    updateGrades(gradesWithDisplayValue)
                 })
        }        
    }
    
    //initialize effect hook chain
    useEffect(determineSubjectID, [])
    useEffect(fetchData, subjectId)

    //display everything
    return (
        <div>
            {subjectInfo && studentAverage ? <SubjectTitle info={subjectInfo} average={studentAverage} /> : null}
            
            {grades && subjectInfo?
                <div className="grades-material-container">
                    <StudentGrades grades={grades} info={subjectInfo}/>
                    <StudentMaterial material={material}/>                    
                </div>                
                : null}
        </div>
    );
}

export default SubjectDetail;
