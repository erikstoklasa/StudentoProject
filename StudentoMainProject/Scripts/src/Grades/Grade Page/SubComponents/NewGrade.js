import React, { useState, useEffect, useRef } from 'react'
import { Input } from '../../../../Styles/GlobalStyles';
import { Cell } from '../SharedStyles';
import styled from 'styled-components'

const NewCell = styled(Cell)` 
    width: 100%;
`
const GradeInput = styled(Input)` 
    max-width: 40px;
    max-height: 25px;
    padding: 0.375rem 0 !important;
    -webkit-appearance: none !important;
    margin: 0;
    text-align: center;
`

const NewGrade = ({ studentId, showInput, onGradeChange, onGradeRemove, currentStudentEdited  , onClickOutside, updateCurrentStudentEdited, defaultValue }) => {
    const [newGradeValue, updateNewGradeValue] = useState("");
    const inputRef = useRef(null)

    const clearGrades = () => {
        if (showInput) {
            updateNewGradeValue('')
        }
    }

    const handleGradeChange = (value) => {

        if (value.length === 1) {
            if (parseInt(value) > 0 && parseInt(value) < 6) {
                updateNewGradeValue(value)
                onGradeChange(value, studentId)
            }
        } else if (value.length === 2) {
            if (value.charAt(1) === '+' || value.charAt(1) === '-' || value.charAt(0) === '1' && value.charAt(1) === '*') {
                updateNewGradeValue(value)
                onGradeChange(value, studentId)
            }
        } else if (value === '') {
            updateNewGradeValue(value)
            onGradeRemove(studentId);
        }    
        
    }

    const handleFocus = () => {        
        if (studentId === currentStudentEdited && showInput) {
            inputRef.current.focus()
        } 
    }

    const fillDefaultValue = () => {       
        if (defaultValue.default) {
            updateNewGradeValue(defaultValue.value)           
        }
    }
    
    useEffect(handleFocus, [currentStudentEdited])

    useEffect(clearGrades, [showInput]);
    
    useEffect(fillDefaultValue, [defaultValue])

    if (showInput) {
        return (
            <NewCell>
                <GradeInput
                    ref={inputRef}
                    maxLength="2"                   
                    value={newGradeValue}
                    onBlur={() => onClickOutside(studentId)}
                    onClick={() => updateCurrentStudentEdited(studentId)}
                    onChange={event => { handleGradeChange(event.target.value) }}></GradeInput>
            </NewCell>
        )
    }else { 
        return (
            <NewCell ref={inputRef}></NewCell>
        )
    }
    
}

export default NewGrade