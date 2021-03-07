import React from 'react'
import GradeRow from './GradeRow'

const GradeView = ({ grades, info }) => {
    
    //create an html element for each grade recieved in props, put them into an array
    const gradeList = grades.map(grade => { 
        return <GradeRow grade={grade} info={info}/>
    })

    //display array of grade html elements
    return (
        <div className="table table-responsive table-white">
            {gradeList}
        </div>
    )

 }

export default GradeView