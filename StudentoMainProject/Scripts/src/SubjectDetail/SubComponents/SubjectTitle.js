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
            return <h2 className="subject-heading-text">{`Ø `}{formattedAverage.toFixed(2)}</h2>
        } else {
            return null
        }
    }
    
    //display subject title, student average and teacher name
    if (info) {
        return (
            <div className="subject-title-container">
                <div className="heading-container">
                    <div className="average-container">
                        <h2 className="subject-detail-title">{info.name}</h2>                        
                        { grades.success? calculateStudentAverage(grades.data) : null}                        
                    </div>                    
                    <h5 className="mb0">{`${info.teacher.firstName} ${info.teacher.lastName}`}</h5>
                </div>                                
            </div>
        )
    } else { 
        return (<div>
        </div>)
    }
}

export default SubjectTitle