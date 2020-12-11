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

    useEffect(clearGrades, [showInput]);

    if (showInput) {
        return (
            <div className="grade-cell new-grade-cell">
                <input maxLength="1" className="form-control grade-input" value={newGradeValue} onChange={event => {handleInputChange(event.target.value)}}></input>
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