import React, { useState } from 'react'
import GradeDetailPopup from './GradeDetailPopup'
import '../SubjectDetail.css'

const GradeRow = ({ grade, info, deleteGrade }) => {    
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
    
    const getGradeName = () => {
        if (!grade.gradeGroupName) {
            return grade.name
        } else {
            return grade.gradeGroupName
        }
    }

        return (
            <div>
                <div className='grade-container'>
                    <div className="grade-name-container">
                        <p className={`grade-child grade-value grade-circle ${getGradeClass(grade.value)}`}>{grade.displayValue}</p>
                    </div>
                    <div className="grade-sub-container" onClick={() => { updateShowDetail(true) }}>                        
                            <p className="grade-child grade-name">{getGradeName()}</p>                            
                        <p className="grade-child grade-time">{grade.addedDisplay}</p>
                    </div>
                </div>
                {showDetail? <GradeDetailPopup grade={grade} info={info} deleteGrade={deleteGrade} hidePopup={() => { updateShowDetail(false) }}/> : null}
            </div>
        )        
     
    
}

export default GradeRow

