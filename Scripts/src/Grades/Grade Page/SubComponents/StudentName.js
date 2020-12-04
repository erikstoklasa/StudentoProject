import React from 'react'
import '../GradePage.css';

const StudentName = ({ name }) => {
    return (
        <div className="student-name-cell">
            <p className="student-name-text">{name}</p>
        </div>
    )
}

export default StudentName