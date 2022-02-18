import React, { useState, useEffect } from 'react'
import { PrimaryButton } from '../../../Styles/GlobalStyles'
import styled from 'styled-components'
import moment from 'moment'

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
    width: 100%;
    max-width: 400px;    
    margin: 20px 20px 0px 20px;
    padding:15px 20px 15px 20px;  
    background-color: white;
    border: none;
    border-radius: 10px;
    overflow: hidden;  
`
const TitleContainer = styled.div` 
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
`
const Title = styled.h4` 
    margin: 0;
`
const CloseIcon = styled.img` 
    cursor: pointer;
`
const InputContainer = styled.div` 
    margin: 20px 0px 10px 0px; 
`
const InputRow = styled.div`
    justify-content: space-between;
    display: flex;
    margin-bottom: 15px;
`
const Label = styled.p` 
    display: flex;
    align-items: center;
    color: var(--grey);
    margin: 0px 10px 0px 0px;
    white-space: nowrap;
`
const InputField = styled.input` 
    max-width: 50%;
    background: #FFFFFF;
    box-shadow: 1px 1px 8px rgba(0, 0, 0, 0.1);
    border: transparent;
    outline: none;
    border-radius: 10px;
    height: calc(1.5em + 0.75rem + 2px);
    padding: 0.375rem 0.75rem;
`
const ShortInputField = styled(InputField)` 
    max-width: 42px;
`
const InputSubContainer = styled.div` 
    display: flex;
    justify-content: space-between;
    min-width: 50%;
`
const WeightInput = styled.div`
    display: flex;
    justify-content: space-between;
`
const WarningText = styled.p` 
    color: var(--pastelRed);
    font-size: 1rem;
    font-weight: 600;  
    margin: 10px 0px 10px 0px;
`
const ButtonContainer = styled.div`
    display: flex;
    justify-content: flex-end;
`
const Info = styled.div` 
    font-size: 0.8rem;
    border-radius: 10px;
    margin-top: 20px;
    width: 100%;
    padding: 15px;
    max-width: 400px;    
    font-weight: 600; 
    color: #0c5460;
    background-color: #d1ecf1;
    border-color: #bee5eb;
`

const addGradePopup = ({ addGrade, hidePopup }) => {    
    const [gradeName, updateGradeName] = useState()
    const [gradeValue, updateGradeStateValue] = useState('')
    const [gradeWeight, updateGradeWeight] = useState('')
    const [gradeDate, updateGradeDate] = useState()
    const [showWarning, updateShowWarning] = useState(false)
    const [warningMessage, updateWarning] = useState('')    

    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return () => document.body.style.overflow = 'unset';
    }, []);

    const setInitialDate = () => {       
        const today = moment(new Date()).format("YYYY-MM-DD")        
        updateGradeDate(today)
    }

    useEffect(setInitialDate, [])

    const onAddClick = () => {        
        if (gradeName && gradeValue && gradeWeight) {
            addGrade(gradeName, gradeValue, gradeWeight, gradeDate)
            hidePopup()
            if (showWarning) { updateShowWarning(false) }
        }
        else {
            if (!gradeName && !gradeValue || !gradeName && !gradeWeight || !gradeValue && !gradeWeight || !gradeWeight && !gradeValue && !gradeName)  {
                updateWarning('Prosím zadej všechny údaje')
            } else if (!gradeName) {
                updateWarning('Prosím zadej název známky')
            } else if (!gradeValue) {
                updateWarning('Prosím zadej známku')
            } else if (!gradeWeight) {
                updateWarning('Prosím zadej váhu známky')
            }
                updateShowWarning(true)          
        }
    }
    
    const updateGradeValue = (value) => {        
        if (value.length === 1) {
            if (parseInt(value) > 0 && parseInt(value) < 6) {
                
                updateGradeStateValue(value)
            }
        } else if (value.length === 2) {
            if (value.charAt(1) === '+' || value.charAt(1) === '-' || value.charAt(0) === '1' && value.charAt(1) === '*') {
                updateGradeStateValue(value)            
            }
        } else if (value === '') {           
            updateGradeStateValue('')
        }   
    }

    const setGradeName = (val) => {
        if (val.length <= 27) {
            updateGradeName(val)
        }
    }

    const checkWeightValue = (value) => {
        const val = parseInt(value)      
        if (val > 0 && val <= 10) {          
            updateGradeWeight(val)
        } else if (value === '') {
            updateGradeWeight(value)
        }
    }

    return (
       
        <Container onClick={hidePopup}>
            <InnerContainer onClick={(e=> {e.stopPropagation()})}>               
                    <TitleContainer>
                        <Title>Přidat známku</Title>
                        <CloseIcon src="/images/icons/delete.svg" alt="zavřít" height="30px" onClick={() => { hidePopup()}}/>
                    </TitleContainer>              
                    <InputContainer>
                        <InputRow>
                            <Label>Název známky</Label>
                            <InputField value={gradeName} onChange={(event) => {setGradeName(event.target.value)}}/>
                        </InputRow>
                        <InputRow>
                            <Label>Známka</Label>
                            <InputSubContainer>                               
                                <ShortInputField value={gradeValue} onChange={(event) => { updateGradeValue(event.target.value) }}/>
                                <WeightInput>
                                    <Label>Váha</Label>
                                    <ShortInputField value={gradeWeight} onChange={(event) => { checkWeightValue(event.target.value) }}/>
                                </WeightInput>
                            </InputSubContainer>
                        </InputRow>
                        <InputRow>
                            <Label>Datum</Label>
                            <InputField type="date" value={gradeDate} onChange={(event) => {updateGradeDate(event.target.value)}}/>
                        </InputRow>
                    </InputContainer>
                    { showWarning? 
                        <WarningText>{warningMessage}</WarningText>
                        : null}
                    <ButtonContainer>
                        <PrimaryButton onClick={onAddClick}>Přidat</PrimaryButton>
                    </ButtonContainer>
            </InnerContainer>
            <Info>
                Tuhle známku učitel neuvidí, je jen pro tebe, nemá vliv na průměr, který vidí učitel.
            </Info>
        </Container>
        
    )
}

export default addGradePopup