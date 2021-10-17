import React, { useState, useEffect } from 'react'
import '../SubjectDetail.css'
import moment from 'moment'

const addGradePopup = ({ addGrade, hidePopup }) => {    
    const [gradeName, updateGradeName] = useState()
    const [gradeValue, updateGradeStateValue] = useState('')
    const [gradeWeight, updateGradeWeight] = useState('')
    const [gradeDate, updateGradeDate] = useState()
    const [showWarning, updateShowWarning] = useState(false)
    const [warningMessage, updateWarning] = useState('')    

    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return () => document.body.style.overflow = 'unset';
    }, []);

    const setInitialDate = () => {       
        const today = moment(new Date()).format("YYYY-MM-DD")        
        updateGradeDate(today)
    }

    useEffect(setInitialDate, [])

    const onAddClick = () => {        
        if (gradeName && gradeValue && gradeWeight) {
            addGrade(gradeName, gradeValue, gradeWeight, gradeDate)
            hidePopup()
            if (showWarning) { updateShowWarning(false) }
        }
        else {
            if (!gradeName && !gradeValue || !gradeName && !gradeWeight || !gradeValue && !gradeWeight || !gradeWeight && !gradeValue && !gradeName)  {
                updateWarning('Prosím zadej všechny údaje')
            } else if (!gradeName) {
                updateWarning('Prosím zadej název známky')
            } else if (!gradeValue) {
                updateWarning('Prosím zadej známku')
            } else if (!gradeWeight) {
                updateWarning('Prosím zadej váhu známky')
            }
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

    const setGradeName = (val) => {
        if (val.length <= 27) {
            updateGradeName(val)
        }
    }

    const checkWeightValue = (value) => {
        const val = parseInt(value)      
        if (val > 0 && val <= 10) {          
            updateGradeWeight(val)
        } else if (value === '') {
            updateGradeWeight(value)
        }
    }


    return (
        <div>
            <div className="add-grade-popup-container" onClick={hidePopup}>
            <div className="add-grade-popup-inner-container" onClick={(e=> {e.stopPropagation()})}>               
                    <div className="popup-title-container">
                        <h4 className="popup-title">Přidat známku</h4>
                        <img className="pointer" src="/images/icons/delete.svg" alt="zavřít" height="30px" onClick={() => { hidePopup()}}></img>
                    </div>              
                    <div className="popup-input-container">
                        <div className="popup-input-row">
                            <p className="input-label">Název známky</p>
                            <input className="popup-input-field" value={gradeName} onChange={(event) => {setGradeName(event.target.value)}}></input>
                        </div>
                        <div className="popup-input-row">
                            <p className="input-label">Známka</p>
                            <div className="input-grade-value-weight">                               
                                <input className="popup-input-field popup-input-small" value={gradeValue} onChange={(event) => { updateGradeValue(event.target.value) }}></input>
                                <div className="popup-weight-input">
                                <p className="input-label">Váha</p>
                                <input className="popup-input-field popup-input-small" value={gradeWeight} onChange={(event) => { checkWeightValue(event.target.value) }}></input>
                                </div>
                            </div>
                        </div>
                        <div className="popup-input-row relative">
                            <p className="input-label">Datum</p>
                            <input type="date" className="popup-input-field" value={gradeDate} onChange={(event) => {updateGradeDate(event.target.value)}}></input>
                        </div>
                    </div>
                    { showWarning? 
                        <p className="add-warning-text">{warningMessage}</p>
                        : null}
                    <div className="flex-right">
                        <button className="btn btn-primary next-material-button" onClick={onAddClick}>Přidat</button>
                    </div>
            </div>
            <div className="grade-info-box">
                Tuhle známku učitel neuvidí, je jen pro tebe, nemá vliv na průměr, který vidí učitel.
            </div>
        </div>
        </div>
    )
}

export default addGradePopup