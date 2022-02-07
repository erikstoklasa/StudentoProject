import React, { useState } from 'react'
import apiAddress from '../variables'
import '../SubjectDetail.css'

const GradeRow = ({ student }) => {
       
    const adress = window.location.href.substring(0, window.location.href.indexOf("T"));

    let gradesString;

    if (student.grades) {
        const gradeList = student.grades.map(grade => {
            return grade.displayValue
        })

         gradesString = gradeList.join(',')
    }   

    
    return (
        <div>
            <div className='grade-container-teacher'>
                <div className="name-container-teacher">
                        <a className="" href={`${adress}Teacher/Students/Details?id=${student.id}`}>{`${student.firstName} ${student.lastName}`}</a>
                </div>
                <div className="grade-sub-container-teacher">              
                    <p className="student-average-teacher"><b>{student.average}</b></p>
                    <p className="student-average-teacher">{ gradesString }</p>
                </div>
            </div>         
        </div>
    )    
}

export default GradeRow

