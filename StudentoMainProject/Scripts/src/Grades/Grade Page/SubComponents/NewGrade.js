import React, {useState, useEffect, useRef} from 'react'
import '../GradePage.css'

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
            <div className="grade-cell new-grade-cell" >
                <input
                    ref={inputRef}
                    maxLength="2"
                    className="form-control grade-input"
                    value={newGradeValue}
                    onBlur={() => onClickOutside(studentId)}
                    onClick={() => updateCurrentStudentEdited(studentId)}
                    onChange={event => { handleGradeChange(event.target.value) }}></input>
            </div>
        )
    }     
    else { 
        return (
            <div className = "grade-cell new-grade-cell" ref={inputRef}>
            </div>
        )
    }
    
}

export default NewGrade