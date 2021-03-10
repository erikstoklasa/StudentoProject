import React, { useState } from 'react'
import apiAddress from '../variables'
import '../SubjectDetail.css'

const GradeRow = ({ grade, info }) => {    
    const [showDetail, updateShowDetail] = useState(false);
    
    //returns grades css class based on value
    const getGradeClass = (value) => { 
        if (value >= 90) {
            return ' background-green text-dark'
        }
        if (value >= 65 && value < 90) {
            return ' background-grey text-dark'
        }
        if (value >= 40 && value < 65) {
            return ' background-yellow text-dark'
        }
        if (value >= 15 && value < 40) {
            return ' background-red text-dark'
        }
        if (value >= -10 && value < 15) {
            return ' background-black text-light'
        }
    } 

    const deleteStudentGrade = () => {        
        const reqBody = [grade.id];
        fetch(`${apiAddress}/Grades/Student/Batch`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(reqBody)
        })
    }

    if (!showDetail) {
        return (
            <div>
                <div className='grade-container'>
                    <div className="grade-name-container">
                        <p className={`grade-child grade-value grade-circle ${getGradeClass(grade.value)}`}>{grade.displayValue}</p>
                    </div>
                    <div className="grade-sub-container" onClick={() => { updateShowDetail(true) }}>
                        
                            <p className="grade-child grade-name">{grade.name}</p>
                       
                        <p className="grade-child grade-time">{grade.addedRelative}</p>
                    </div>
                </div>
         
            </div>
        )
    } else { 
        return (
            <div className="grade-info-container">
                <p className="grade-info-name">{grade.name}</p>
                <div className="grade-info-information-container">
                    <div>
                        <p className="grade-child">Známka :</p>
                        <p className="grade-child">Předmět :</p>
                        <p className="grade-child">Datum přidání :</p>
                        <p className="grade-child">Vyučujicí :</p>
                    </div>    
                    <div className="grade-info-sub-container" onClick={() => { updateShowDetail(true) }}>
                        <p className="grade-child">{grade.displayValue}</p>
                        <p className="grade-child">{info.name}</p>
                        <p className="grade-child">{grade.addedDisplay}</p>
                        <p className="grade-child">{`${info.teacher.firstName} ${info.teacher.lastName}`}</p>
                    </div> 
                </div>
                <div className="close-button-container">
                    <div class="btn btn-danger rm" onClick={() => { deleteStudentGrade() }}>Smazat</div>
                    <div class="btn btn-primary" onClick={() => { updateShowDetail(false) }}>Zavřít</div>
                </div>
            </div>
        )
    }
}

export default GradeRow

