import React from 'react'
import styled from 'styled-components'

const Container = styled.div` 
    margin-bottom: 10px;
    display: flex;
    justify-content: space-between; 
    align-items: center; 
`
const NameContainer = styled.div` 
    display: inline-block;
    min-width: 150px;   
    max-width: 600px;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    margin-top: auto;
    margin-bottom: auto;
`
const GradesContainer = styled.div` 
    display: flex;
    flex-wrap: nowrap;
    align-items: center;
    min-width: 150px;
    width: 30%;
    justify-content: space-between;  
`
const Average = styled.p` 
    margin: 0 5% 0 5%;
    font-weight: bold;
`
const Grades = styled.p` 
    margin: 0 5% 0 5%;
`

const GradeRow = ({ student }) => {
       
    const adress = window.location.href.substring(0, window.location.href.indexOf("T"));

    let gradesString;

    if (student.grades) {
        const gradeList = student.grades.map(grade => {
            return grade.displayValue
        })

         gradesString = gradeList.join(',')
    } 
    
    return (        
        <Container>
            <NameContainer>
                <a href={`${adress}Teacher/Students/Details?id=${student.id}`}>{`${student.firstName} ${student.lastName}`}</a>
            </NameContainer>
            <GradesContainer>              
                <Average>{student.average}</Average>
                <Grades>{ gradesString }</Grades>
            </GradesContainer>
        </Container>         
        
    )    
}

export default GradeRow

