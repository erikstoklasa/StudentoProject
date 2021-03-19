import React, { useState, useEffect } from 'react'
import '../SubjectDetail.css'

const addGradePopup = ({ addGrade, hidePopup }) => {    
    const [gradeName, updateGradeName] = useState()
    const [gradeValue, updateGradeStateValue] = useState()

    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    const onAddClick = () => {        
        if (gradeName && gradeValue) {
            addGrade(gradeName, gradeValue)
        }
    }
    
    const updateGradeValue = (value) => {
        if (parseInt(value) > 0 && parseInt(value) < 6) {
            updateGradeStateValue(value)
        }
    }

    return (
        <div className="add-grade-popup-container">
            <div className="popup-inner-container">
                <div className="popup-title-container">
                    <h4 className="popup-title">Nová známka</h4>
                    <img src="/images/close.svg" alt="zavřít" height="25px" onClick={() => { hidePopup()}}></img>
                </div>              
                <div className="popup-input-container">               
                    <input className="name-input form-control" placeholder="Test" value={gradeName} onChange={(event) => {updateGradeName(event.target.value)}}></input>
                    <input className="number-input form-control" type="number" placeholder="1" value={gradeValue} onChange={(event) => { updateGradeValue(event.target.value)} }></input>
                </div>
                <div className="add-grade-div">
                    <p className="add-grade-text" onClick={onAddClick}>Přidat</p>
                </div>
            </div>
        </div>
    )
}

export default addGradePopup