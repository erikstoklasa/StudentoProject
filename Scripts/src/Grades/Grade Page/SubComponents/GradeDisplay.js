import React, {useState} from 'react'
import '../GradePage.css'

const GradeDisplay = ({ gradeId, studentId, value, modifyGrade, gradeName }) => {
    const [gradeValue, updateGradeValue] = useState(value);
    const [displayInput, updateDisplayInput] = useState(false);
    const handleClick = () => {
        updateDisplayInput(true)
    }

    const hideInput = () => {
        updateDisplayInput(false)
    }

    const handleGradeChange = (value) => {
        const intValue = parseInt(value)
        if (value > 0 && value < 6) {
            updateGradeValue(intValue)
            modifyGrade(gradeId, intValue, studentId, gradeName)
        }
        else if (value === '') {
            updateGradeValue('')
            modifyGrade(gradeId, 0)
        }
    }

    if (!displayInput) {
        return (
            <div className="grade-cell" onClick={handleClick}>
                <p className="grade-text" >{gradeValue}</p>
            </div>
        )
    }
    if (displayInput) {
        return (
            <div className="grade-cell">
                <input className="new-grade-input" type="number" maxLength="1" tabIndex="0" autoFocus onBlur={hideInput} value={gradeValue} onChange={(event) => { handleGradeChange(event.target.value) }}></input>
            </div>
        )
    }

}

export default GradeDisplay