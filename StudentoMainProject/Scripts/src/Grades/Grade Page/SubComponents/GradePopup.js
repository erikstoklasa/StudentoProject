import React, { useState, useEffect } from 'react'
import {Input, PrimaryButton, DangerButton} from '../../../../Styles/GlobalStyles'
import styled from 'styled-components'

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
    justify-content: center;
    align-items: center;
    z-index: 5;
`
const InnerContainer = styled.div` 
    width: 100%;
    max-width: 400px;
    margin: 20px;
    padding: 15px 15px 15px 15px;
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
    > h4 {
        margin: 0;
    }
`
const InputContainer = styled.div` 
    display: flex;
    margin-bottom: 10px;
    margin-top: 10px;
    margin: 10px 0px;
`
const CloseIcon = styled.img` 
    cursor: pointer;
    height: 18px;
`
const WeightInput = styled(Input)` 
    max-width: 62px;
    margin-left: 10px;
`
const GradePresetContainer = styled.div` 
    display: flex;
    margin-bottom: 10px;
    align-items: center;
    justify-content: space-between;
    align-items: center;
    > label {
    margin-bottom: 0;
    }
`
const DefaultInput = styled.select`
    display: block;
    width: 100px;
    height: calc(1.5em + 0.75rem + 2px);
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #495057;
    border: 1px solid rgb(206, 212, 218) !important;
    background-color: #fff;
    background-clip: padding-box;    
    border-radius: 0.25rem;
    transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    outline: none !important;
    :hover{
        background: none;
    }
`
const Warning = styled.p` 
    color: var(--pastelRed);
    font-size: 1rem;
    font-weight: 600;
    margin: 0px 0px 10px 0px;
`
const AddButton = styled(PrimaryButton)` 
    padding: 0.4rem 0.7rem !important;
    width: 100% !important;
`

const ButtonContainer = styled.div`   
    display: flex;
    gap: 15px;
    justify-content: space-between;
    > div {
        flex-grow: 1;
    }
`

const GradePopup = ({ grade, onNameChange, onWeightChange, newGrade, closePopup, onAddClick, modifyGradeGroup, deleteGradeGroup }) => {
    const [gradeName, updateGradeName] = useState('')
    const [defaultNewValue, updateDefaultNewValue] = useState('null')
    const [gradeWeight, updateGradeWeight] = useState('')
    const [displayWarning, updateDisplayWarning] = useState(false)

    useEffect(() => {
        if (!newGrade) {           
            updateGradeName(grade.gradeGroupName)
            updateGradeWeight(grade.gradeGroupWeight)
        }
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    const handleWeightChange = (input) => {
        const i = parseInt(input)
        if (i > 0 && i <= 10) {
            updateGradeWeight(i)
            if(newGrade) onWeightChange(i)
        } else if (input === '') {
            updateGradeWeight(input)
            if(newGrade) onWeightChange(input)
        }
    }

    const handleAddClick = () => {
        if (newGrade) {
            if (gradeName && gradeWeight) {
                onAddClick(defaultNewValue)
                updateDisplayWarning(false)
            } else {
                updateDisplayWarning(true)
            }
        } else {
            if (gradeName && gradeWeight) {
                modifyGradeGroup(grade.gradeGroupId, gradeName, gradeWeight, grade)
                updateDisplayWarning(false)
                closePopup()
            } else {
                updateDisplayWarning(true)
            }
        }
    }

    const handleDeleteClick = () => {
        deleteGradeGroup(grade.gradeGroupId)
        closePopup()
    }

    return (
        <Container>
            <InnerContainer>
                <TitleContainer>
                    {newGrade ? <h4>Přidat známku</h4> : <h4>Upravit známku</h4>}
                    <CloseIcon src="/images/close.svg" alt="" onClick={closePopup}/>
                </TitleContainer>
                <InputContainer>
                    <Input placeholder="Jméno známky" maxLength="50" value={gradeName} onChange={(event) => { updateGradeName(event.target.value); if (newGrade) { onNameChange(event) }}}/>
                    <WeightInput placeholder="Váha" value={gradeWeight} onChange={(event) => { handleWeightChange(event.target.value) }} />                    
                </InputContainer>
                {newGrade ? <GradePresetContainer>
                    <label htmlFor='default-grade'>Předvyplnit známky</label>
                    <DefaultInput id="default-grade" id="cars" name="cars" value={defaultNewValue} onChange={event => updateDefaultNewValue(event.target.value)}>
                        <option value={1}>1</option>
                        <option value={2}>2</option>
                        <option value={3}>3</option>
                        <option value={4}>4</option>
                        <option value={5}>5</option>
                        <option value="null">Žádné</option>
                    </DefaultInput>
                </GradePresetContainer> : null}
                {displayWarning ? <Warning>Prosím zadej název a váhu</Warning> : null}                
                {newGrade ?
                    <AddButton onClick={handleAddClick}>Přidat</AddButton> : 
                    <ButtonContainer>
                        <PrimaryButton onClick={handleAddClick}>Uložit</PrimaryButton>
                        <DangerButton onClick={handleDeleteClick}>Smazat</DangerButton>
                    </ButtonContainer>}                
            </InnerContainer>
        </Container>
    )

}

export default GradePopup