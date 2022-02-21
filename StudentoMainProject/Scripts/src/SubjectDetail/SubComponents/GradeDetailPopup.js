import React, { useEffect } from 'react'
import { DangerButton } from '../../../Styles/GlobalStyles'
import styled, { css } from 'styled-components'

const Container = styled.div` 
    min-width: 250px;
    position: fixed;
    width: 100%;
    height: 100%;
    right: 0;
    top: 0;
    left: 0;
    bottom: 0;
    margin: auto;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    padding: 15px;
    z-index: 5;
`
const InnerContainer = styled.div` 
    max-height: 95vh;
    width: 100%;
    max-width: 400px;
    margin: 20px;
    padding: 15px;   
    background-color: white;
    border: none;
    border-radius: 10px;
    overflow-y: auto; 
`
const TitleContainer = styled.div` 
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
`
const Title = styled.h4`
    max-width: 70%;
    overflow: hidden;
    text-overflow: ellipsis;
`
const CloseIcon = styled.img` 
     cursor: pointer;
     height: 30px;
`
const Time = styled.p` 
     margin: 0px 0px 10px 0px;
`
const Grade = styled.div` 
    font-size: 1.5rem;
    height: 3rem;
    width: 3rem;
    margin-bottom: 10px;
    display: flex;
    justify-content: center;
    align-items: center;    
    border-radius: 50%;
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
const WeightContainer = styled.div`
    display: flex;
    margin-top: 10px;
    margin-bottom: 10px;
    > p {
        margin-bottom: 0;
    }
`
const WeightCircle = styled.div`
    height: 1.5rem;
    width: 1.5rem;
    margin-left: 5px;
    border-radius: 50%;
    background-color: white;
    border: solid black 1.25px;
    display: flex;
    justify-content: center;
    align-items: center;
    color: black;
    line-height: initial;
    font-size: 0.75rem;
`
const ButtonContainer = styled.div`
    display: flex;
    justify-content: flex-end;
`
const TrashIcon = styled.img` 
    margin-right: 5px;
    height: 20px;
`

const GradeDetailPopup = ({ grade, deleteGrade, hidePopup }) => {   
    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);    

    const getGradeName = (grade) => {
        if (grade.gradeGroupName) {
            return grade.gradeGroupName
        } else {
            return grade.name
        }
    }

    return (       
        <Container onClick={() => { hidePopup()}}>
            <InnerContainer onClick={(event)=>{event.stopPropagation()}}>                
                    <TitleContainer>
                        <Title>{getGradeName(grade)}</Title>
                        <CloseIcon src="/images/icons/delete.svg" alt="zavřít" onClick={() => { hidePopup()}}></CloseIcon>
                    </TitleContainer>
                        <Time>{grade.addedDisplay}</Time>                       
                        <Grade value={grade.value}>{grade.displayValue}</Grade>                      
                        <WeightContainer>
                            <p>Váha: </p>
                            <WeightCircle>{grade.gradeGroupWeight? grade.gradeGroupWeight : grade.weight}</WeightCircle>
                        </WeightContainer>
                        {grade.addedBy === 1 ?
                        <ButtonContainer>
                                <DangerButton onClick={() => { hidePopup(); deleteGrade(grade.id) }}>
                                    <TrashIcon src="/images/icons/trash_white.svg"/>
                                    Smazat známku</DangerButton>
                        </ButtonContainer> : null}
                
            </InnerContainer>
        </Container>        
    )

}

export default GradeDetailPopup