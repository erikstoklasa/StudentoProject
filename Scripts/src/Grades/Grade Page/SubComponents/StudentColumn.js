import React from 'react'
import StudentName from './StudentName.js'
import ColumnHeader from './ColumnHeader.js';

const StudentNames = ({ students }) => {
    
    const NameList = students.map(student => {
        return <StudentName key={student.id} name={`${student.firstName} ${student.lastName}`}  />
    })   

    

    return (
        <div className="grade-table-column student-column">
            <ColumnHeader title={''} /> 
            <div className="grade-shadow-left">
             {NameList}
            </div>
        </div>
    )
}

export default StudentNames