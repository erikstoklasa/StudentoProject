import React, {useState, useEffect} from 'react'
import GradeDisplay from './GradeDisplay'
import ColumnHeader from './ColumnHeader'
import GradePopup from './GradePopup'
import '../GradePage.css'

const GradeDisplayColumn = ({ grade, students, studentGrades, modifyGrade, modifyGradeGroup, deleteGradeGroup }) => {
    const [displayPopup, updateDisplayPopup] = useState(false)
    const [currentStudentEdited, updateCurrentStudentEdited] = useState('none')
    const gradeList = studentGrades.filter(studentGrade => studentGrade.gradeGroupId === grade.gradeGroupId)

    const updateCurrentStudent = (id) => {
        updateCurrentStudentEdited(id)        
    }   
    
    const handleOutsideClick = (currentId) => {
        if(currentStudentEdited === currentId) updateCurrentStudentEdited('none')
    }

    const gradeDisplayList = students.map((student, index) => {
            let gId;
            let gValue = '';
            gradeList.forEach(grade => {
                if (grade.studentId === student.id) {
                    gId = grade.id;
                    gValue = grade.displayValue;
                }        
            }           
        )       
        return <GradeDisplay key={index} gradeId={gId} studentId={student.id} value={gValue} modifyGrade={modifyGrade} gradeName={grade.name} grade={grade} gradeGroupId={grade.gradeGroupId} currentStudentEdited={currentStudentEdited} updateCurrentStudentEdited={updateCurrentStudent} onClickOutside={handleOutsideClick}/>
    }) 
   
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
   
    const activatePopup = () => {
        updateDisplayPopup(true)
    }
    const closePopup = () => {        
        updateDisplayPopup(false)
    }   

    return (
        <div className="" onKeyDown={handleArrowClick}>
            <ColumnHeader title={grade.gradeGroupName} grade={grade} activatePopup={activatePopup}/>
            {gradeDisplayList}
            {displayPopup ? <GradePopup grade={grade} newGrade={false} closePopup={closePopup} modifyGradeGroup={modifyGradeGroup}  deleteGradeGroup={deleteGradeGroup}/>: null}
        </div>
    )
}

export default GradeDisplayColumn