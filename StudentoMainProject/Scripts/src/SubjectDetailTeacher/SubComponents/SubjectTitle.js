import React from 'react'
import '../SubjectDetail.css'

const SubjectTitle = ({ info, average }) => {
    
    //display subject title, student average and teacher name
    if (info) {
        return (
            <div className="subject-title-container">
                <div className="heading-container">
                    <h2>{info.name}</h2>                    
                </div>
                <div className="average-container ">
                    <h2>{`Ø `}{average.toFixed(2)}</h2>
                </div>
            </div>
        )
    } else { 
        return(<div></div>)
    }
}

export default SubjectTitle