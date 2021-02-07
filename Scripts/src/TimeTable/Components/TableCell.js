import React from 'react'
import '../TimeTable.css'

const TableCell = ({ frame }) => {
    if (frame.timetableEntry) {
        return (
            <div className="table-cell">
                {frame.timetableEntry.subjectInstanceName}
            </div>
        )
    }
    else { 
        return (
            <div className="table-cell">
                empty
            </div>
        )
    }
 }

export default TableCell