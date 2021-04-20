import React, {useState} from 'react'
import GradeView from './GradeView.js'

const StudentGrades = ({ students, info }) => {
    
    // display grades
    return (
        <div className="student-grades-container">
            <div className="student-heading-container">
                <p className="grades-heading">Studenti</p>
                <div className="student-heading-sub-container">
                    <a class="btn btn-outline-primary rm" href={`/Teacher/Subjects/Details?id=${info.id}&amp;handler=print`}><img src="/images/print.svg" alt="Vytisknout" height="20px" class="btn-icon" />Tisk výpisu</a>
                    <a class="btn btn-primary display-grades-link" href={`/Teacher/Grades?SubjectInstanceId=${info.id}`}>Zobrazit známky</a>
                </div>
            </div>
            <div>
                <GradeView students={students} info={info} type={'teacherGrades'}/>
            </div>            
        </div>
    )
}

export default StudentGrades


