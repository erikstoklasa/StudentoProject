import React,{ useState, useEffect} from 'react'

const GradePopup = ({ grade, onNameChange, onWeightChange, newGrade, closePopup, onAddClick, modifyGradeGroup, deleteGradeGroup }) => {
    const [gradeName, updateGradeName] = useState('')
    const [defaultNewValue, updateDefaultNewValue] = useState('null')
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
                onAddClick(defaultNewValue)
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

    const handleDeleteClick = () => {
        deleteGradeGroup(grade.gradeGroupId)
        closePopup()
    }

    return (
        <div className="popup-outer">
            <div className="popup-inner">
                <div className="popup-title-container">
                    {newGrade ? <h4 className="popup-title">Přidat známku</h4> : <h4 className="popup-title">Upravit známku</h4>}
                    <img className="pointer" src="/images/close.svg" alt="zavřít" height="25px" onClick={closePopup}></img>
                </div>
                <div className="popup-input-container mb10">
                    <input className="form-control" placeholder="Jméno známky" maxLength="50" value={gradeName} onChange={(event) => { updateGradeName(event.target.value); if (newGrade) { onNameChange(event) }}}/>
                    <input className="form-control weight-input" placeholder="Váha" value={gradeWeight} onChange={(event) => { handleWeightChange(event.target.value) }} />                    
                </div>
                {newGrade ? <div className='grade-page-popup-default-container mb10'>
                    <label htmlFor='default-grade'>Předvyplnit známky</label>
                    <select id="default-grade" className="grade-page-default-grade-input" id="cars" name="cars" value={defaultNewValue} onChange={event => updateDefaultNewValue(event.target.value)}>
                        <option value={1}>1</option>
                        <option value={2}>2</option>
                        <option value={3}>3</option>
                        <option value={4}>4</option>
                        <option value={5}>5</option>
                        <option value="null">Žádné</option>
                    </select>
                </div> : null}
                {displayWarning ? <p class="add-warning-text">Prosím zadej název a váhu</p> : null}
                
                {newGrade ? <button className="btn btn-primary next-material-button" onClick={handleAddClick}>Přidat</button> : 
                    <div className="grade-page-popup-button-container">
                        <button className="btn btn-primary" onClick={handleAddClick}>Uložit</button>
                        <button className="btn btn-danger" onClick={handleDeleteClick}>Smazat</button>
                    </div>
                }
                
            </div>
        </div>
    )

}

export default GradePopup