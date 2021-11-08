import React, {useState, useEffect} from 'react'
import '../GradePage.css'

const NewGrade = ({ studentId, showInput, onGradeChange, onGradeRemove }) => {
    const [newGradeValue, updateNewGradeValue] = useState("");

    const handleInputChange = (value) => { 
        if (value > 0 && value < 6) {
            updateNewGradeValue(value)
            onGradeChange(value, studentId)
        }
        else if (value === '') {
            updateNewGradeValue(value)
            onGradeRemove(studentId);
        }
    }

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

    useEffect(clearGrades, [showInput]);

    if (showInput) {
        return (
            <div className="grade-cell new-grade-cell">
                <input maxLength="2" className="form-control grade-input" value={newGradeValue} onChange={event => {handleGradeChange(event.target.value)}}></input>
            </div>
        )
    }
    else { 
        return (
            <div className = "grade-cell new-grade-cell">
            </div>
        )
    }
    
}

export default NewGrade