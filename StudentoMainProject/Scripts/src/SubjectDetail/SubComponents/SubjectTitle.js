import React from 'react'
import '../SubjectDetail.css'

const SubjectTitle = ({ info, grades }) => {
    
    const calculateStudentAverage = (data) => {
        const getGradeWeight = (grade) => {              
            if (!grade.gradeGroupWeight) {
                if (grade.weight) return grade.weight
                else return 1
            } else {
                return grade.gradeGroupWeight
            }
        }        
        if (data.length > 0) {            
            let sum = 0;
            let gradeNum = data.reduce((sum, current)=>{return sum + getGradeWeight(current)}, 0);
            data.forEach(grade => {
                sum = sum + parseInt(grade.value)*getGradeWeight(grade)
          
            });
            const average = sum / gradeNum
            const formattedAverage = 5 - (average / 25)            
            return <h2 className="mb0"><b>{`Ø `}{formattedAverage.toFixed(2)}</b></h2>
        } else {
            return null
        }
    }
    
    //display subject title, student average and teacher name
    if (info) {
        return (
            <div className="subject-title-container">
                <div className="heading-container">
                    <h2 className="subject-detail-title"><b>{info.name}</b></h2>
                    <h5 className="mb0">{`${info.teacher.firstName} ${info.teacher.lastName}`}</h5>
                </div>
                <div className="average-container ">
                    {calculateStudentAverage(grades)}
                </div>
            </div>
        )
    } else { 
        return (<div>
        </div>)
    }
}

export default SubjectTitle