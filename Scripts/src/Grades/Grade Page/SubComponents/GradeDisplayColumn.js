import React from 'react'
import GradeDisplay from './GradeDisplay'
import ColumnHeader from './ColumnHeader'
import '../GradePage.css'

const GradeDisplayColumn = ({ grade, students, studentGrades, modifyGrade }) => {
  
    const gradeList = studentGrades.filter(studentGrade => studentGrade.name === grade.name)
    const gradeDisplayList = students.map((student, index) => {
            let gId;
            let gValue = '';
            gradeList.forEach(grade => {
                if (grade.studentId === student.id) {
                    gId = grade.id;
                    gValue = grade.value;
                }        
            }           
        )
       
        return <GradeDisplay key={index} gradeId={gId} studentId={student.id} value={gValue} modifyGrade={modifyGrade} gradeName={grade.name}/>
    })
   
    
    return (
        <div className="">
            <ColumnHeader title={grade.name} />
            {gradeDisplayList}
        </div>
    )
}

export default GradeDisplayColumn