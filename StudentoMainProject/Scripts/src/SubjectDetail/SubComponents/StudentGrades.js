import React from 'react'
import GradeView from './GradeView.js'
import ErrorAlert from '../../../Components/Alerts/ErrorAlert.js'
import InfoAlert from '../../../Components/Alerts/InfoAlert.js'

const StudentGrades = ({ grades, showPopup, deleteGrade }) => {
    
        if (grades.loaded) {
            //filter grades for grades added by student
            const studentGrades = grades.data.filter(grade => {
                return grade.addedBy === 1
            })

            //filter grades for grades added by teacher
            const teacherGrades = grades.data.filter(grade => {
                return grade.addedBy === 0
            })
        
            return (
                <div className="student-grades-container">
                    <p className="grades-heading">Známky od vyučující/ho</p>
                    <GradeView grades={teacherGrades} type={'teacherGrades'} />
                    <div className="grades-heading-container">
                        <p className="grades-heading">Známky přidáné mnou</p>
                        <a class="btn btn-primary" onClick={() => { showPopup() }}><img src="/images/add.svg" alt="Přidat" height="20px" class="btn-icon" ></img>Přidat známku</a>
                    </div>
                    <GradeView grades={studentGrades} type={'studentGrades'} deleteGrade={deleteGrade} />
                </div>
            )

        } else {
            return (
                <div className="student-grades-container">
                    <p className="grades-heading">Známky</p>
                    <ErrorAlert text={'Nepodařilo se načíst známky 🙁'} />
                </div>
            )
        }
    
}

export default StudentGrades


