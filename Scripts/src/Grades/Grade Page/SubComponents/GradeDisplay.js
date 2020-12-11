import React, {useState} from 'react'
import '../GradePage.css'

const GradeDisplay = ({ gradeId, studentId, value, modifyGrade, gradeName }) => {
    const [initialValue, updateInitialValue] = useState(value);
    const [gradeValue, updateGradeValue] = useState(value);
    const [displayInput, updateDisplayInput] = useState(false);

    const handleClick = () => {       
        updateDisplayInput(true)        
    }

    const hideInput = () => {
        updateDisplayInput(false)
        if (gradeValue > 0 && gradeValue < 6 && gradeValue !== initialValue) {
            updateInitialValue(gradeValue);
            modifyGrade(gradeId, gradeValue, studentId, gradeName)
        }
        else if (gradeValue === '' && gradeValue !== initialValue) {
            if (gradeId) {
                updateInitialValue(gradeValue)
                modifyGrade(gradeId, 0)
            }
        }
    }

    const handleGradeChange = (value) => {
        const intValue = parseInt(value)
        if (value > 0 && value < 6) {
            updateGradeValue(intValue)
        }
        else if (value === '') {
            updateGradeValue('')
        }
    }

    if (!displayInput) {
        return (
            <div className="grade-cell">
                <div className="grade-text" onClick={handleClick}>{gradeValue}</div>
            </div>
        )
    }
    if (displayInput) {
        return (
            <div className="grade-cell">
                <input className="form-control grade-input" maxLength="1" tabIndex="0" autoFocus onBlur={hideInput} value={gradeValue} onChange={(event) => { handleGradeChange(event.target.value) }}></input>
            </div>
        )
    }

}

export default GradeDisplay