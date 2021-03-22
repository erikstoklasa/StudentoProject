import React from 'react'
import '../SubjectDetail.css'

const SubjectTitle = ({ info, average }) => {
    
    //display subject title, student average and teacher name
    if (info) {
        return (
            <div className="subject-title-container">
                <div className="heading-container">
                    <h1>{info.name}</h1>                    
                </div>
                <div className="average-container ">
                    <h3>{`Ø `}{average.toFixed(2)}</h3>
                </div>
            </div>
        )
    } else { 
        return(<div></div>)
    }
}

export default SubjectTitle