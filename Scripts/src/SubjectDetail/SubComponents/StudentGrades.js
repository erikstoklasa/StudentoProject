import React, {useState} from 'react'
import GradeView from './GradeView.js'

const StudentGrades = ({ grades, info }) => {
    const [showAddGrade, updateShowAddGrade] = useState(false)
    
    //filter grades for grades added by student
    const studentGrades = grades.filter(grade => { 
        return grade.addedBy === 1
    })

    //filter grades for grades added by teacher
    const teacherGrades = grades.filter(grade => { 
        return grade.addedBy === 0
    })

    const hideAddGradeMenu = () => { 
        updateShowAddGrade(false)
    }

    // display grades
    return (
        <div className="student-grades-container">
            <p className="grades-heading">Známky od vyučující/ho</p>
            <div>
                <GradeView grades={teacherGrades} info={info}/>
            </div>
            <div className="grades-heading-container">
                <p className="grades-heading">Známky přidány mnou</p>
                <a class="btn btn-primary"><img src="/images/add.svg" alt="Přidat" height="20px" class="btn-icon" onClick={() => { updateShowAddGrade( true )}}></img>Přidat známku</a>
            </div>
            <div>
                <GradeView grades={studentGrades} info={info} hideAddMenu={hideAddGradeMenu} showAddGrade={showAddGrade}/>
            </div>
        </div>
    )
}

export default StudentGrades


