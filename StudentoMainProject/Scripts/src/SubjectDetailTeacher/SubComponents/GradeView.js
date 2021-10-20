import React from 'react'
import GradeRow from './GradeRow'

const GradeView = ({ students, info}) => {
 
    //create an html element for each student recieved in props, put them into an array
    const studentList = students.map(student => { 
        return <GradeRow student={student} info={info}/>
    })

    if (students) {
        //display array of student html elements       
            return (
                <div className="table table-responsive table-white subject-detail-table">
                    {studentList}                   
                </div>
            )
                  
    } else {        
            return (
                <div>
                    <p class="alert alert-info my-1">Zatím žádní studenti 🙁</p>
                </div>
            )
           
    }
 }

export default GradeView