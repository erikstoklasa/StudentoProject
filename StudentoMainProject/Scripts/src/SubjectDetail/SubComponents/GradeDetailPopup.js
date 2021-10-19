import React, { useEffect } from 'react'


const GradeDetailPopup = ({ grade, deleteGrade, hidePopup }) => {   
    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    const getGradeClass = (value) => { 
        if (value >= 90) {
            return ' background-blue text-light'
        }
        if (value >= 65 && value < 90) {
            return ' background-green text-light'
        }
        if (value >= 40 && value < 65) {
            return ' background-yellow text-light'
        }
        if (value >= 15 && value < 40) {
            return ' background-red text-light'
        }
        if (value >= -10 && value < 15) {
            return ' background-black text-light'
        }
    }

    const getGradeName = (grade) => {
        if (grade.gradeGroupName) {
            return grade.gradeGroupName
        } else {
            return grade.name
        }
    }

    return (
        <div>
             <div className="add-grade-popup-container" onClick={() => { hidePopup()}}>
            <div className="popup-inner-container" onClick={(event)=>{event.stopPropagation()}}>
                <div className="popup-inner-padding">
                    <div className="popup-title-container">
                        <h4 className="detail-popup-title">{getGradeName(grade)}</h4>
                        <img className="pointer" src="/images/icons/delete.svg" alt="zavřít" height="30px" onClick={() => { hidePopup()}}></img>
                    </div>
                        <p className="detail-relative-time">{grade.addedDisplay}</p>                       
                        <div className={`detail-grade-circle ${getGradeClass(grade.value)}`}>{grade.displayValue}</div>                      
                        <div className="detail-weight-container">
                            <p className="mb0">Váha: </p>
                            <div className="detail-grade-weight-circle">{grade.gradeGroupWeight? grade.gradeGroupWeight : grade.weight}</div>
                        </div>
                        {grade.addedBy === 1 ?
                        <div className="flex-right">
                                <button onClick={() => { hidePopup(); deleteGrade(grade.id) }} class="btn btn-danger next-material-button vert-center">
                                    <img className="mr5" src="/images/icons/trash_white.svg" height="20px"/>
                                    Smazat známku</button>
                        </div> : null}
                </div>
            </div>
        </div>
        </div>
    )

}

export default GradeDetailPopup