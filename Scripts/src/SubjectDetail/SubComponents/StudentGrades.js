
import React from 'react'
import GradeView from './GradeView.js'

const StudentGrades = ({ grades }) => {    
    const studentGrades = grades.filter(grade => { 
        return grade.addedBy === 1
    })

    const teacherGrades = grades.filter(grade => { 
        return grade.addedBy === 0
    })

    return (
        <div>
            <p className="grades-heading">Známky od vyučující/ho</p>
            <div>
                <GradeView grades={teacherGrades}/>
            </div>
            <div className="grades-heading-container">
                <p className="grades-heading">Známky přidány mnou</p>
                <a class="btn btn-primary" href="/Student/Grades/Create?SubjectInstanceId=1"><img src="/images/add.svg" alt="Přidat" height="20px" class="btn-icon"></img>Přidat známku</a>
            </div>
            <div>
                <GradeView grades={studentGrades} />
            </div>
        </div>
    )
}

export default StudentGrades


