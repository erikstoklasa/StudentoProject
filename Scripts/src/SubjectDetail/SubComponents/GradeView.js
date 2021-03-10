import React from 'react'
import GradeRow from './GradeRow'

const GradeView = ({ grades, info, hideAddMenu, showAddGrade}) => {
    
    //create an html element for each grade recieved in props, put them into an array
    const gradeList = grades.map(grade => { 
        return <GradeRow grade={grade} info={info}/>
    })

    //display array of grade html elements
    if (!showAddGrade) {
        return (
            <div className="table table-responsive table-white">
                {gradeList}
            </div>
        )
    } else { 
        return (
            <div className="table table-responsive table-white">
                
            </div>
        )
    }
 }

export default GradeView