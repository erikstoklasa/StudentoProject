import React, { useState, useEffect } from 'react'



const GradeDetailPopup = ({ grade, info, deleteGrade, hidePopup }) => {
    
    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);
    return (
        <div>
             <div className="add-grade-popup-container">
            <div className="popup-inner-container">
                <div className="popup-inner-padding">
                    <div className="popup-title-container">
                        <h4 className="detail-popup-title">{grade.gradeGroupName}</h4>
                        <img className="pointer" src="/images/icons/delete.svg" alt="zavřít" height="30px" onClick={() => { hidePopup()}}></img>
                    </div>
                        <p className="detail-relative-time">{grade.addedRelative}</p>
                        <p class="detail-grade-circle">{grade.displayValue}</p>
                        {grade.addedBy === 1 ?
                        <div>
                            <button onClick={() => { hidePopup(); deleteGrade(grade.id) }} class="btn btn-danger next-material-button">Smazat</button>
                        </div> : null}
                </div>
            </div>
        </div>
        </div>
    )

}

export default GradeDetailPopup