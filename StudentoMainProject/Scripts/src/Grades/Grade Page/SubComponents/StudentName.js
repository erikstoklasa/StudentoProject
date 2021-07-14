import React from 'react'
import '../GradePage.css';

const StudentName = ({ name }) => {
    return (
        <div className="student-name-cell">
            <div className="student-name-text">{name}</div>
        </div>
    )
}

export default StudentName