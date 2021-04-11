import React, { useState, useEffect } from 'react'
import '../SubjectDetail.css'

const addGradePopup = ({ addGrade, hidePopup }) => {    
    const [gradeName, updateGradeName] = useState()
    const [gradeValue, updateGradeStateValue] = useState()
    const [showWarning, updateShowWarning] = useState(false)

    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    const onAddClick = () => {        
        if (gradeName && gradeValue) {
            addGrade(gradeName, gradeValue)
            hidePopup()
            if (showWarning) { updateShowWarning(false) }
        }
        else {
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

    return (
        <div className="add-grade-popup-container">
            <div className="popup-inner-container">
                <div className="popup-inner-padding">
                    <div className="popup-title-container">
                        <h4 className="popup-title">Nová známka</h4>
                        <img className="pointer" src="/images/close.svg" alt="zavřít" height="25px" onClick={() => { hidePopup()}}></img>
                    </div>              
                    <div className="popup-input-container">               
                        <input className="name-input form-control" placeholder="Test" value={gradeName} onChange={(event) => {updateGradeName(event.target.value)}}></input>
                        <input className="number-input form-control" placeholder="1" value={gradeValue} maxlength="2" onChange={(event) => { updateGradeValue(event.target.value)} }></input>
                    </div>
                    { showWarning? 
                        <p className="add-warning-text">Chybí údaje</p>
                   : null}
                        <button className="btn btn-primary next-material-button" onClick={onAddClick}>Přidat</button>
                </div>
            </div>
        </div>
    )
}

export default addGradePopup