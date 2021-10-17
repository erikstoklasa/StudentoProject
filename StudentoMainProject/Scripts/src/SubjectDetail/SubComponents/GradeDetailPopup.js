import React, {useEffect} from 'react'



const GradeDetailPopup = ({ grade, info, deleteGrade, hidePopup }) => {
   
    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    const getGradeClass = (value) => { 
        if (value >= 90) {
            return ' background-green text-dark'
        }
        if (value >= 65 && value < 90) {
            return ' background-grey text-dark'
        }
        if (value >= 40 && value < 65) {
            return ' background-yellow text-dark'
        }
        if (value >= 15 && value < 40) {
            return ' background-red text-dark'
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
                        <div className="detail-circles-container">
                            <p className={`detail-grade-circle ${getGradeClass(grade.value)}`}>{grade.displayValue}</p>                           
                        </div>
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