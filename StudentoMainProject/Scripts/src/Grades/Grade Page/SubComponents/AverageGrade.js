import React from 'react'
import '../GradePage.css';

const AverageGrade = ({ grade }) => {
    return (
    <div className="grade-cell">
            {(grade ? <p className="average-grade-text">{grade}</p> : <p className="grade-text"></p>)}           
    </div>   
    )
}

export default AverageGrade