import React, {useState} from 'react'
import ColumnHeader from './ColumnHeader';
import GradePopup from './GradePopup'
import NewGrade from './NewGrade';
import { PrimaryButton } from '../../../../Styles/GlobalStyles';
import styled from 'styled-components'

const ButtonContainer = styled.div` 
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 10px;
    margin-bottom: 10px;
`

const NewGradeColumn = ({ students, trackNewGradeValues, removeNewGrade, handleSubmitNewGrades, assignDefaults }) => {
    const [displayGradeNameInput, updateDisplayGradeNameInput] = useState(false)
    const [defaultGradeValues, updateDefaultGradeValues] = useState({
        default: false,
        value: null
    })
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
        return <NewGrade key={index} studentId={student.id} showInput={showInput} onGradeChange={trackNewGradeValues} onGradeRemove={removeNewGrade} currentStudentEdited={currentStudentEdited} updateCurrentStudentEdited={updateCurrentStudent} onClickOutside={handleOutsideClick} defaultValue={defaultGradeValues}/>
    })

    const onGradeNameInputChange = (event) => {
        updateNewGradeName(event.target.value)
    }

    const onWeightChange = (value) => {
        upadeNewGradeWeight(value)
    }

   
    const onAddClick = (defaultGrade) => {
        if (defaultGrade !== 'null') {
            assignDefaults(defaultGrade)            
            updateDefaultGradeValues({
                default: true,
                value: defaultGrade
            })            
        }
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
        <div onKeyDown={handleArrowClick}>            
            <ColumnHeader title={'Přidat známku'} type={'New Grade'} handleClick={handleHeaderClick} displayInput={displayGradeNameInput} gradeName={newGradeName} displayName={displayNameInHeader} />
            {displayPopup ? <GradePopup newGrade={true} onNameChange={onGradeNameInputChange} onWeightChange={onWeightChange} closePopup={closePopup} onAddClick={onAddClick}/> : null}
            {inputList}
            {(showInput ? <ButtonContainer>
                <PrimaryButton onClick={() => {
                if (newGradeName && newGradeWeight) {
                    handleSubmitNewGrades(newGradeName, newGradeWeight)              
                        resetColumn()                
                }
                }}>Přidat</PrimaryButton>
            </ButtonContainer> : null)}            
        </div>
    )

}

export default NewGradeColumn