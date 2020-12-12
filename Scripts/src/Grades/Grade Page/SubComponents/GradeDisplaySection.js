import React from 'react'
import GradeDisplayColumn from './GradeDisplayColumn'
import '../GradePage.css'

const GradeDisplaySection = ({ orderedGrades, orderedStudents, bulkGradeData, modifyGrade }) => { 
 
        
    const gradeColumnList = orderedGrades.map((grade, index) => {
            return <GradeDisplayColumn key={index} grade={grade} students={orderedStudents} studentGrades={bulkGradeData} modifyGrade={modifyGrade} />
    })        
           
    return (
        <div className="grade-column-section">
            {gradeColumnList}
        </div>
    )
}

export default GradeDisplaySection