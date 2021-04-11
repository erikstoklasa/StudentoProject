import React, {useState, useEffect} from 'react'
import '../GradePage.css'

const GradeDisplay = ({ gradeId, studentId, value, modifyGrade, gradeName }) => {
    const [initialValue, updateInitialValue] = useState();
    const [gradeValue, updateGradeValue] = useState();
    const [displayInput, updateDisplayInput] = useState(false);

    useEffect(() => {
        updateInitialValue(value)
        updateGradeValue(value)
    }, [value])

    const handleClick = () => {       
        updateDisplayInput(true)        
    }

    const hideInput = () => {
        updateDisplayInput(false)
        if (parseInt(gradeValue) > 0 && parseInt(gradeValue) < 6 && gradeValue !== initialValue) {
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

        if (value.length === 1) {
            if (parseInt(value) > 0 && parseInt(value) < 6) {
                updateGradeValue(value)
            }
        } else if (value.length === 2) {
            if (value.charAt(1) === '+' || value.charAt(1) === '-' || value.charAt(0) === '1' && value.charAt(1) === '*') {
                updateGradeValue(value)
            }
        } else if (value === '') {
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
                <input className="form-control grade-input" maxLength="2" tabIndex="0" autoFocus onBlur={hideInput} value={gradeValue} onChange={(event) => { handleGradeChange(event.target.value) }}></input>
            </div>
        )
    }

}

export default GradeDisplay