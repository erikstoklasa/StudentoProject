import React from 'react'
import GradeDisplayColumn from './GradeDisplayColumn'
import '../GradePage.css'

const GradeDisplaySection = ({ orderedGrades, orderedStudents, bulkGradeData, modifyGrade, modifyGradeGroup }) => { 
 
        
    const gradeColumnList = orderedGrades.map((grade, index) => {
        return <GradeDisplayColumn key={index} grade={grade} students={orderedStudents} studentGrades={bulkGradeData} modifyGrade={modifyGrade} modifyGradeGroup={modifyGradeGroup}/>
    })        
           
    return (
        <div className="grade-column-section">
            {gradeColumnList}
        </div>
    )
}

export default GradeDisplaySection