import React, { useState } from 'react'
import GradeDetailPopup from './GradeDetailPopup'
import styled, { css } from 'styled-components'

const Container = styled.div` 
    margin-bottom: 10px;
    display: flex;
    padding: 5px;
    border-radius: 10px;
    align-items: center;
    cursor: pointer;
    :hover{
        background: rgba(0,0,0,0.15);
    }
`
const Grade = styled.div` 
    margin: 0;
    height: 2.5rem;
    width: 2.5rem;
    font-size: 1.3rem;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    color: white;
    ${props =>
        {
            if (props.value >= 90) {
                return (
                    css` 
                        background: var(--pastelBlue);
                    `
                )
            }
            else if (props.value >= 65 && props.value < 90) {
                return (
                    css` 
                        background: var(--pastelGreen);
                    `
                )
            }
            else if (props.value >= 40 && props.value < 65) {
                return (
                    css` 
                        background: #FEC368;
                    `
                )
            }
            else if (props.value >= 15 && props.value < 40) {
                return (
                    css` 
                        background: var(--pastelRed);
                    `
                )
            }
            else if (props.value >= -10 && props.value < 15) {
                return (
                    css` 
                        background: var(--pastelDarkRed);                        
                    `
                )
            }
        }
  }
`
const Weight = styled.div` 
    position: absolute;
    height: 1.3rem;
    width: 1.3rem;
    border-radius: 50%;
    background-color: white;
    border: solid black 1.25px;
    display: flex;
    justify-content: center;
    align-items: center;
    bottom: -5px;
    right: -5px;
    color: black;
    font-size: 0.75rem;
`
const GradeInfo = styled.div` 
    min-width: 100px;
    margin-left: 20px;  
`
const GradeName = styled.h5` 
    white-space: nowrap;  
    margin-bottom: 0px;
`
const GradeTime = styled.p` 
    white-space: nowrap;
    margin-bottom: 0px;
    color: var(--grey);
`

const GradeRow = ({ grade, deleteGrade }) => {    
    const [showDetail, updateShowDetail] = useState(false); 
    
    const getGradeName = () => {
        if (!grade.gradeGroupName) {
            return grade.name
        } else {
            return grade.gradeGroupName
        }
    }

    return (
        <>
            <Container onClick={() => { updateShowDetail(true) }}>                   
                <Grade value={grade.value}>
                    {grade.displayValue}
                    <Weight>{grade.gradeGroupWeight? grade.gradeGroupWeight : grade.weight}</Weight>
                </Grade>
                <GradeInfo>                        
                    <GradeName>{getGradeName()}</GradeName>                            
                    <GradeTime>{grade.addedRelative}</GradeTime>                        
                </GradeInfo>
            </Container>
            {showDetail? <GradeDetailPopup grade={grade} deleteGrade={deleteGrade} hidePopup={() => { updateShowDetail(false) }}/> : null}
        </>
    )   
}

export default GradeRow

