import React, { useState } from 'react'
import GradeDetailPopup from './GradeDetailPopup'
import '../SubjectDetail.css'

const GradeRow = ({ grade, deleteGrade }) => {    
    const [showDetail, updateShowDetail] = useState(false); 
    
    //returns grades css class based on value
        const getGradeClass = (value) => { 
            if (value >= 90) {
                return ' background-blue text-light'
            }
            if (value >= 65 && value < 90) {
                return ' background-green text-light'
            }
            if (value >= 40 && value < 65) {
                return ' background-yellow text-light'
            }
            if (value >= 15 && value < 40) {
                return ' background-red text-light'
            }
            if (value >= -10 && value < 15) {
                return ' background-black text-light'
            }
    }
    
    const getGradeName = () => {
        if (!grade.gradeGroupName) {
            return grade.name
        } else {
            return grade.gradeGroupName
        }
    }

        return (
            <div>
                <div className='grade-container' onClick={() => { updateShowDetail(true) }}>                   
                    <div className={`grade-value subject-detail-grade-circle ${getGradeClass(grade.value)}`}>
                        {grade.displayValue}
                        <div className="grade-weight-circle">{grade.gradeGroupWeight? grade.gradeGroupWeight : grade.weight}</div>
                    </div>
                    <div className="grade-sub-container">                        
                            <h5 className="grade-name">{getGradeName()}</h5>                            
                            <p className="grade-time">{grade.addedRelative}</p>
                    </div>
                </div>
                {showDetail? <GradeDetailPopup grade={grade} deleteGrade={deleteGrade} hidePopup={() => { updateShowDetail(false) }}/> : null}
            </div>
        )        
     
    
}

export default GradeRow

