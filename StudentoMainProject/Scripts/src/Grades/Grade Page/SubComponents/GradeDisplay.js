import React, {useState, useEffect, useRef} from 'react'
import '../GradePage.css'

const GradeDisplay = ({ gradeId, studentId, value, modifyGrade, gradeName, grade, gradeGroupId, currentStudentEdited, updateCurrentStudentEdited, onClickOutside }) => {
    const [initialValue, updateInitialValue] = useState();
    const [gradeValue, updateGradeValue] = useState();
    const [displayInput, updateDisplayInput] = useState(false);
    const gradeValueRef = useRef(gradeValue)
    
    useEffect(() => {        
        if (studentId === currentStudentEdited) {
            updateDisplayInput(true)
        }
    })

    useEffect(() => {
        updateInitialValue(value)
        updateGradeValue(value)
    }, [value])  

    const setGradeValue = (value) => {
        updateGradeValue(value)
        gradeValueRef.current = value
    }

    const handleClick = () => {       
        updateDisplayInput(true)
        updateCurrentStudentEdited(studentId)
        addEventListener("keypress", onEnterPress)
    }

    const onEnterPress = (event) => {       
        if (event.key === "Enter") {
            removeEventListener('keypress', onEnterPress)
            hideInput()
        }
    }

    const commitGrade = () => {
        if (parseInt(gradeValueRef.current) > 0 && parseInt(gradeValueRef.current) < 6 && gradeValueRef.current !== initialValue) {          
            updateInitialValue(gradeValueRef.current);
            modifyGrade(gradeId, gradeValueRef.current, studentId, gradeName, grade, gradeGroupId)
        }
        else if (gradeValueRef.current === '' && gradeValueRef.current !== initialValue) {
            if (gradeId) {
                updateInitialValue(gradeValueRef.current)
                modifyGrade(gradeId, 0, studentId, gradeName, grade)
            }
        } 
    }

    const hideInput = () => {
        updateDisplayInput(false)
        onClickOutside(studentId)
        commitGrade()
        removeEventListener('keypress', onEnterPress);
    }

    const handleGradeChange = (value) => {

        if (value.length === 1) {
            if (parseInt(value) > 0 && parseInt(value) < 6) {
                setGradeValue(value)
            }
        } else if (value.length === 2) {
            if (value.charAt(1) === '+' || value.charAt(1) === '-' || value.charAt(0) === '1' && value.charAt(1) === '*') {
                setGradeValue(value)
            }
        } else if (value === '') {
            setGradeValue('')
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