import React, {useState} from 'react'
import ColumnHeader from './ColumnHeader';
import NewGrade from './NewGrade';
import apiAdress from './Variables.js'
import '../GradePage.css'

const NewGradeColumn = ({ students, trackNewGradeValues, removeNewGrade, handleSubmitNewGrades }) => {
    const [displayGradeNameInput, updateDisplayGradeNameInput] = useState(false)
    const [newGradeName, updateNewGradeName] = useState('')
    const [showInput, updateShowInput] = useState(false)

    const handleHeaderClick = () => {
        updateShowInput(!showInput);
        updateDisplayGradeNameInput(!displayGradeNameInput)
    }    
    
    const inputList = students.map((student, index) => {
        return <NewGrade key={index} studentId={student.id} showInput={showInput} onGradeChange={trackNewGradeValues} onGradeRemove={removeNewGrade}/>
    })

    const onGradeNameInputChange = (event) => {
        updateNewGradeName(event.target.value)
    }

    return (
        <div className='grade-table-column new-grade-column'>            
            <ColumnHeader title={'Přidat známku'} type={'New Grade'} handleClick={handleHeaderClick} displayInput={displayGradeNameInput} onInputChange={ onGradeNameInputChange} />
            {inputList}
            {(displayGradeNameInput ? <div className="button-cell"><button className="btn btn-primary" onClick={() => {
                handleHeaderClick();
                handleSubmitNewGrades(newGradeName)
            }}>Přidat</button></div> : null)}
            
        </div>
    )

}

export default NewGradeColumn