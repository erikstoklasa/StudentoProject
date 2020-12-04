import React, { useState, useEffect } from 'react';
import StudentColumn from './SubComponents/StudentColumn.js';
import AverageColumn from './SubComponents/AverageColumn.js';
import NewGradeColumn from './SubComponents/NewGradeColumn.js';
import GradeDisplaySection from './SubComponents/GradeDisplaySection.js';
import apiAdress from './SubComponents/Variables'
import './GradePage.css';

const GradePage = () => {
    const [bulkGradeData, updateBulkGradeData] = useState([]);
    const [bulkStudentData, updateBulkStudentData] = useState([]);
    const [InstanceId, updateInstanceId] = useState();
    const [orderedStudents, updateOrderedStudents] = useState();
    const [orderedGrades, updateOrderedGrades] = useState();


    const getInstanceId = () => {
        //use querry selector to get id from the dom, select by subjectInstanceId
        const id = 1;
        updateInstanceId(id);
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
        if (bulkStudentData.students) {
        const studentArray = bulkStudentData.students;            
        studentArray.sort((a, b) => a.lastName.localeCompare(b.lastName)) 
        updateOrderedStudents(studentArray)
        }
        
    }

    const sortGrades = () => {
        if (bulkGradeData.length > 0) {
            const studentGrades = [];
            const sortedGrades = bulkGradeData;            
           
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
            })
           
            studentGrades.reverse();
            updateOrderedGrades(studentGrades)
        }
    }

    useEffect(getInstanceId)
    useEffect(fetchData, [InstanceId])
    useEffect(sortStudents, [bulkStudentData])
    useEffect(sortGrades, [bulkGradeData])
  
    const modifyGrade = (gradeId, gradeValue, studentId, gradeName) => {
        if (gradeId) {
            if (gradeValue === 0) {

                //call API to remove grade
            
                const newBulkGradeData = bulkGradeData.filter(grade => gradeId !== grade.id)
                updateBulkGradeData(newBulkGradeData)

            }
            else {
                const newBulkGradeData = bulkGradeData.map(grade => {
                    if (gradeId === grade.id) {
                        
                    

                        //call API to update grade

                        grade.value = gradeValue
                        return grade
                        
                    }
                    else {
                        return grade
                    }
                })
                updateBulkGradeData(newBulkGradeData)
            }
        } else if(!gradeId) {

            //call API to create new Grade then Call Fetch Data
            const grade = {
                value: gradeValue,
                subjectInstanceId: InstanceId,
                studentId: studentId,
                name: gradeName
            }


        }
    }

    const commitNewGrades = (grades) => {
        //Call API then refetch data

    }
    
    return (        
        <div className="grade-table-container">
            {(orderedStudents ? <StudentColumn students={orderedStudents} /> : null)} 
            {(orderedStudents && bulkGradeData ? <AverageColumn students={orderedStudents} grades={bulkGradeData} /> : null)}
            {(orderedStudents ? <NewGradeColumn students={orderedStudents} subjectInstanceId={InstanceId} /> : null)}
            {(orderedStudents && orderedGrades && bulkGradeData ? <GradeDisplaySection orderedGrades={orderedGrades} orderedStudents={orderedStudents} bulkGradeData={bulkGradeData} modifyGrade={modifyGrade} /> : null)}
            <button onClick={() => {console.log(bulkGradeData)}}>check data</button>
        </div>
    )

}


export default GradePage