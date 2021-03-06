import React from 'react'

const GradeView = ({ grades }) => {
    
    //create an html element for each grade recieved in props, put them into an array
    const gradeList = grades.map(grade => { 
        return (
            <div className="grade-container">
                
                <p className="grade-child grade-value">{grade.displayValue}</p>
                <div className="grade-sub-container">
                <p className="grade-child">{grade.name}</p>
                
                    <p className="grade-child grade-time">{grade.addedRelative}</p>
                </div>
            </div> 
        )
    })

    //display array of grade html elements
    return (
        <div className="table table-responsive table-white">
            {gradeList}
        </div>
    )

 }

export default GradeView