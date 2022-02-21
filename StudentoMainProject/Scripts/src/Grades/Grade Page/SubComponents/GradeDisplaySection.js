import React from 'react'
import GradeDisplayColumn from './GradeDisplayColumn'
import styled from 'styled-components'

const Section = styled.div` 
    display: flex;
`

const GradeDisplaySection = ({ orderedGrades, orderedStudents, bulkGradeData, modifyGrade, modifyGradeGroup, deleteGradeGroup }) => { 
 
        
    const gradeColumnList = orderedGrades.map((grade, index) => {
        return <GradeDisplayColumn key={index} grade={grade} students={orderedStudents} studentGrades={bulkGradeData} modifyGrade={modifyGrade} modifyGradeGroup={modifyGradeGroup} deleteGradeGroup={deleteGradeGroup}/>
    })        
           
    return (
        <Section>
            {gradeColumnList}
        </Section>
    )
}

export default GradeDisplaySection