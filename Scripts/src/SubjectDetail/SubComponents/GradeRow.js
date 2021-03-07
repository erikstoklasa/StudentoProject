import React, {useState} from 'react'
import '../SubjectDetail.css'

const GradeRow = ({ grade, info }) => {
    console.log(info)
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

    return (
        <div>
            <div className='grade-container'>                
                <p className={`grade-child grade-value grade-circle ${getGradeClass(grade.value)}`}>{grade.displayValue}</p>
                <div className="grade-sub-container">
                    <a className="grade-child grade-name">{grade.name}</a>                    
                    <p className="grade-child grade-time">{grade.addedRelative}</p>
                </div>            
            </div>
         
        </div>    
    )
}

export default GradeRow

/*
   {showDetail ?
            
                <div className="grade-info-container">
                    <div className="grade-info-row">
                        <p className="grade-child">Předmět :</p>                        
                        <p className="grade-child grade-info-detail-text">{info.name}</p>                        
                    </div>
                </div>
                
            : null}
             onClick={() => { updateShowDetail(!showDetail) }
            */
