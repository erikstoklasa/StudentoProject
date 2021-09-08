import React,{ useState, useEffect} from 'react'

const GradePopup = ({ grade, onNameChange, onWeightChange, newGrade, closePopup, onAddClick, modifyGradeGroup }) => {
    const [gradeName, updateGradeName] = useState('')
    const [gradeWeight, updateGradeWeight] = useState('')
    const [displayWarning, updateDisplayWarning] = useState(false)

    useEffect(() => {
        if (!newGrade) {           
            updateGradeName(grade.gradeGroupName)
            updateGradeWeight(grade.gradeGroupWeight)
        }
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    const handleWeightChange = (input) => {
        const i = parseInt(input)
        if (i > 0 && i <= 10) {
            updateGradeWeight(i)
            if(newGrade) onWeightChange(i)
        } else if (input === '') {
            updateGradeWeight(input)
            if(newGrade) onWeightChange(input)
        }
    }

    const handleAddClick = () => {
        if (newGrade) {
            if (gradeName && gradeWeight) {
                onAddClick()
                updateDisplayWarning(false)
            } else {
                updateDisplayWarning(true)
            }
        } else {
            if (gradeName && gradeWeight) {
                modifyGradeGroup(grade.gradeGroupId, gradeName, gradeWeight, grade)
                updateDisplayWarning(false)
                closePopup()
            } else {
                updateDisplayWarning(true)
            }
        }
    }

    return (
        <div className="popup-outer">
            <div className="popup-inner">
                <div className="popup-title-container">
                    {newGrade ? <h4 className="popup-title">Přidat známku</h4> : <h4 className="popup-title">Změnit známku</h4>}
                    <img className="pointer" src="/images/close.svg" alt="zavřít" height="25px" onClick={closePopup}></img>
                </div>
                <div className="popup-input-container">
                    <input className="form-control mb10" placeholder="Jméno známky" maxLength="50" value={gradeName} onChange={(event) => { updateGradeName(event.target.value); if (newGrade) { onNameChange(event) }}}/>
                    <input className="form-control mb10 weight-input" placeholder="Váha" value={gradeWeight} onChange={(event) => { handleWeightChange(event.target.value) }} />
                </div>
                {displayWarning? <p class="add-warning-text">Prosím zadej název a váhu</p> : null}
                <button className="btn btn-primary next-material-button" onClick={handleAddClick}>Přidat</button>
            </div>
        </div>
    )

}

export default GradePopup