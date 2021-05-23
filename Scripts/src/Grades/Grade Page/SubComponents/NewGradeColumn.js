import React, {useState} from 'react'
import ColumnHeader from './ColumnHeader';
import GradePopup from './GradePopup'
import NewGrade from './NewGrade';
import '../GradePage.css'

const NewGradeColumn = ({ students, trackNewGradeValues, removeNewGrade, handleSubmitNewGrades }) => {
    const [displayGradeNameInput, updateDisplayGradeNameInput] = useState(false)
    const [displayNameInHeader, updateDisplayName] = useState(false)
    const [newGradeName, updateNewGradeName] = useState('')
    const [newGradeWeight, upadeNewGradeWeight] = useState('')
    const [displayPopup, updateDisplayPopup] = useState(false)
    const [showInput, updateShowInput] = useState(false)

    const handleHeaderClick = () => {
        updateDisplayGradeNameInput(true)
        updateDisplayPopup(true)
        updateShowInput(!showInput);        
    }    
    
    const inputList = students.map((student, index) => {
        return <NewGrade key={index} studentId={student.id} showInput={showInput} onGradeChange={trackNewGradeValues} onGradeRemove={removeNewGrade}/>
    })

    const onGradeNameInputChange = (event) => {
        updateNewGradeName(event.target.value)
    }

    const onWeightChange = (value) => {
        upadeNewGradeWeight(value)
    }

   
    const onAddClick = () => {
        updateDisplayName(true)
        updateDisplayPopup(false)
    }

    const closePopup = () => {       
        resetColumn()
        updateDisplayPopup(false)
    }

    const resetColumn = () => {        
        updateShowInput(false)
        updateDisplayName(false)
        updateDisplayGradeNameInput(false)
        updateDisplayPopup(false)
        updateNewGradeName('')
        upadeNewGradeWeight(null)
    }

    return (
        <div className='grade-table-column new-grade-column'>            
            <ColumnHeader title={'Přidat známku'} type={'New Grade'} handleClick={handleHeaderClick} displayInput={displayGradeNameInput} gradeName={newGradeName} displayName={displayNameInHeader} />
            {displayPopup ? <GradePopup newGrade={true} onNameChange={onGradeNameInputChange} onWeightChange={onWeightChange} closePopup={closePopup} onAddClick={onAddClick}/> : null}
            {inputList}
            {(displayGradeNameInput ? <div className="button-cell"><button className="btn btn-primary" onClick={() => {
                if (newGradeName && newGradeWeight) {
                    handleSubmitNewGrades(newGradeName, newGradeWeight)              
                        resetColumn()                
                }
            }}>Přidat</button></div> : null)}
            
        </div>
    )

}

export default NewGradeColumn