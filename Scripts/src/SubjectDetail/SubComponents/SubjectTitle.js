import React from 'react'
import '../SubjectDetail.css'

const SubjectTitle = ({ info, average }) => {
    if (info) {
        return (
            <div className="subject-title-container">
                <div className="heading-container">
                    <h1>{info.name}</h1>
                    <h4>{`${info.teacher.firstName} ${info.teacher.lastName}`}</h4>
                </div>
                <div className="average-container ">
                    <h3>{`Průměr: `}<b>{average.toFixed(2)}</b></h3>
                </div>
            </div>
        )
    } else { 
        return(<div></div>)
    }
}

export default SubjectTitle