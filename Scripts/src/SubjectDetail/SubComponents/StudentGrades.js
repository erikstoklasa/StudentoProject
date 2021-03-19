import React, {useState} from 'react'
import GradeView from './GradeView.js'

const StudentGrades = ({ grades, info, showPopup, deleteGrade }) => {    
    
    //filter grades for grades added by student
    const studentGrades = grades.filter(grade => { 
        return grade.addedBy === 1
    })

    //filter grades for grades added by teacher
    const teacherGrades = grades.filter(grade => { 
        return grade.addedBy === 0
    })   

    // display grades
    return (
        <div className="student-grades-container">
            <p className="grades-heading">Známky od vyučující/ho</p>
            <div>
                <GradeView grades={teacherGrades} info={info} type={'teacherGrades'}/>
            </div>
            <div className="grades-heading-container">
                <p className="grades-heading">Známky přidány mnou</p>
                <a class="btn btn-primary" onClick={() => { showPopup() }}><img src="/images/add.svg" alt="Přidat" height="20px" class="btn-icon" ></img>Přidat známku</a>
            </div>            
            <div>
                <GradeView grades={studentGrades} info={info} type={'studentGrades'} deleteGrade={ deleteGrade }/>
            </div>
        </div>
    )
}

export default StudentGrades


