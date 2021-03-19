import React from 'react'
import GradeRow from './GradeRow'

const GradeView = ({ grades, info, type, deleteGrade}) => {
 
    //create an html element for each grade recieved in props, put them into an array
    const gradeList = grades.map(grade => { 
        return <GradeRow grade={grade} info={info} type={type} deleteGrade={ deleteGrade}/>
    })

    if (grades) {
        //display array of grade html elements
       
            return (
                <div className="table table-responsive table-white">
                    {gradeList}                   
                </div>
            )
                  
    } else {
        if (type === 'teacherGrades') {
            return (
                <div>
                    <p class="alert alert-info my-1">Zatím ti vyučující nepřidal žádné známky 🙁</p>
                </div>
            )
        }
        if (type === 'studentGrades') {
            <div>
                <p class="alert alert-info mb-4">Tady si můžeš přidat svoje známky. Budou se ti počítat jen do tvého průměru (učitel tyhle známky neuvidí).</p>
            </div>
        }
    }
 }

export default GradeView