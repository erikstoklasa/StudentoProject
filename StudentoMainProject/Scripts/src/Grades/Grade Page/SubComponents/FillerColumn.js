import React from 'react'
import FillerGrade from './FillerGrade'
import ColumnHeader from './ColumnHeader'
import '../GradePage.css'

const FillerColumn = ({ students }) => {
    const cellArray = students.map(student => { 
        return <FillerGrade key={student.id} id={student.id}/>
    })
    return (
        <div className="filler-column">
            <ColumnHeader title={''}/>
            {cellArray}
        </div>
    )
}
export default FillerColumn