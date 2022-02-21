import React from 'react'
import StudentName from './StudentName.js'
import ColumnHeader from './ColumnHeader.js';

const StudentNames = ({ students }) => {    
    const NameList = students.map(student => {
        return <StudentName key={student.id} name={`${student.firstName} ${student.lastName}`}  />
    })    

    return (
        <div>
            <ColumnHeader title={''} /> 
            <div>
             {NameList}
            </div>
        </div>
    )
}

export default StudentNames