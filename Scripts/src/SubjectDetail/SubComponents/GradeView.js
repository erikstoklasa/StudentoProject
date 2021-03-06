import React from 'react'

const GradeView = ({ grades }) => {
    console.log(grades)
    const gradeList = grades.map(grade => { 
        return (
            <div className="grade-container">
                <p className="grade-child">{grade.displayValue}</p>
                <p className="grade-child">{grade.name}</p>
                <p className="grade-child">{grade.addedRelative}</p>
            </div>
        )
    })

    return (
        <div className="table table-responsive table-white grade-table">
            {gradeList}
        </div>
    )

 }

export default GradeView