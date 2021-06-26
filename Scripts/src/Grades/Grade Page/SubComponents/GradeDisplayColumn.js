import React,{useState} from 'react'
import GradeDisplay from './GradeDisplay'
import ColumnHeader from './ColumnHeader'
import GradePopup from './GradePopup'
import '../GradePage.css'

const GradeDisplayColumn = ({ grade, students, studentGrades, modifyGrade, modifyGradeGroup }) => {
    const [displayPopup, updateDisplayPopup] = useState(false)

    const gradeList = studentGrades.filter(studentGrade => studentGrade.gradeGroupId === grade.gradeGroupId)
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
        return <GradeDisplay key={index} gradeId={gId} studentId={student.id} value={gValue} modifyGrade={modifyGrade} gradeName={grade.name} grade={grade} gradeGroupId={grade.gradeGroupId}/>
    })
   
    const activatePopup = () => {
        updateDisplayPopup(true)
    }
    const closePopup = () => {        
        updateDisplayPopup(false)
    }
    console.log('column')
    console.log(grade)

    return (
        <div className="">
            <ColumnHeader title={grade.gradeGroupName} grade={grade} activatePopup={activatePopup}/>
            {gradeDisplayList}
            {displayPopup ? <GradePopup grade={grade} newGrade={false} closePopup={closePopup} modifyGradeGroup={modifyGradeGroup}/> : null}
        </div>
    )
}

export default GradeDisplayColumn