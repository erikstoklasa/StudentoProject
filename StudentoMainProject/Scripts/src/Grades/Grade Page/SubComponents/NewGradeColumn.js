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
    const [currentStudentEdited, updateCurrentStudentEdited] = useState(students[0].id)

    const handleHeaderClick = () => {
        updateDisplayGradeNameInput(true)
        updateDisplayPopup(true)              
    }

    const updateCurrentStudent = (id) => {
        updateCurrentStudentEdited(id)        
    }  

    const handleOutsideClick = (currentId) => {
        if(currentStudentEdited === currentId) updateCurrentStudentEdited('none')
    }
    
    const getNextStudentId = (eventCode) => {
        
        let currentIndex = students.findIndex(student => student.id === currentStudentEdited)
        
        if (eventCode === "ArrowUp" && currentIndex > 0) currentIndex -= 1
        else if (eventCode === 'ArrowDown' && currentIndex < students.length -1) currentIndex += 1
        
        const id = students[currentIndex].id        
        return id
    }

    const handleArrowClick = (e) => {
        if (e.code === 'ArrowUp' || e.code === 'ArrowDown') {
            const id = getNextStudentId(e.code)
            updateCurrentStudentEdited(id)
        }
             
    } 
    
    const inputList = students.map((student, index) => {
        return <NewGrade key={index} studentId={student.id} showInput={showInput} onGradeChange={trackNewGradeValues} onGradeRemove={removeNewGrade} currentStudentEdited={currentStudentEdited} updateCurrentStudentEdited={updateCurrentStudent} onClickOutside={handleOutsideClick}/>
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
        updateShowInput(true); 
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
        <div className='grade-table-column new-grade-column' onKeyDown={handleArrowClick}>            
            <ColumnHeader title={'Přidat známku'} type={'New Grade'} handleClick={handleHeaderClick} displayInput={displayGradeNameInput} gradeName={newGradeName} displayName={displayNameInHeader} />
            {displayPopup ? <GradePopup newGrade={true} onNameChange={onGradeNameInputChange} onWeightChange={onWeightChange} closePopup={closePopup} onAddClick={onAddClick}/> : null}
            {inputList}
            {(showInput ? <div className="button-cell"><button className="btn btn-primary" onClick={() => {
                if (newGradeName && newGradeWeight) {
                    handleSubmitNewGrades(newGradeName, newGradeWeight)              
                        resetColumn()                
                }
            }}>Přidat</button></div> : null)}
            
        </div>
    )

}

export default NewGradeColumn