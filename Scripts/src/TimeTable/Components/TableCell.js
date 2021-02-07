import React from 'react'
import '../TimeTable.css'

const TableCell = ({ frame }) => {
   
    if (frame.timetableEntry) {
        const heading = frame.timetableEntry.subjectInstanceName.substring(0,3)
        return (
            <div className="table-cell">
                <div className="subject-heading">{heading}</div>
            </div>
        )
    }
    else { 
        return (
            <div className="table-cell">
                <div className="subject-heading"></div>
            </div>
        )
    }
 }

export default TableCell