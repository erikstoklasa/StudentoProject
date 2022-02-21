import React from 'react'
import ErrorAlert from '../../../Components/Alerts/ErrorAlert'
import styled from 'styled-components'

const TitleContainer = styled.div` 
    display: flex;
    flex-wrap: wrap;
    margin-bottom: 10px;  
`
const HeadingContainer = styled.div` 
    flex-basis: 760px; 
    flex-wrap: wrap; 
`
const AverageContainer = styled.div` 
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap; 
`
const Title = styled.h2` 
    margin-bottom: 0;
    margin-right: 10px;
`
const Average = styled.h2` 
    margin-bottom: 0;
    white-space: nowrap;
`
const Teacher = styled.h5` 
    margin-bottom: 0;
`

const SubjectTitle = ({ info, grades }) => {
    
    const calculateStudentAverage = (data) => {       
        const getGradeWeight = (grade) => {              
            if (!grade.gradeGroupWeight) {
                if (grade.weight) return grade.weight
                else return 1
            } else {
                return grade.gradeGroupWeight
            }
        }        
        if (data.length > 0) {            
            let sum = 0;
            let gradeNum = data.reduce((sum, current)=>{return sum + getGradeWeight(current)}, 0);
            data.forEach(grade => {
                sum = sum + parseInt(grade.value)*getGradeWeight(grade)
          
            });
            const average = sum / gradeNum
            const formattedAverage = 5 - (average / 25)            
            return <Average>{`Ø `}{formattedAverage.toFixed(2)}</Average>
        } else {
            return null
        }
    }
    
    //display subject title, student average and teacher name
            
    if(info.loaded){
        return (
            <TitleContainer>
                <HeadingContainer>
                    <AverageContainer>
                        <Title>{info.data.name}</Title>                        
                        { grades.loaded? calculateStudentAverage(grades.data) : null}                        
                    </AverageContainer>                    
                    <Teacher>{`${info.data.teacher.firstName} ${info.data.teacher.lastName}`}</Teacher>
                </HeadingContainer>                                
            </TitleContainer>
            )
        } else {
            return (                
                    <ErrorAlert text={'Nepodařilo se načíst předmět 🙁'} />
                )
        }     
}

export default SubjectTitle