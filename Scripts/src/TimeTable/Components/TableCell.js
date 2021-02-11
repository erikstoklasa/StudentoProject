import React from 'react'
import '../TimeTable.css'

const TableCell = ({ frame, type }) => {
   
    if (frame.timetableEntry && !frame.timetableEntryChange) {
        const heading = frame.timetableEntry.subjectInstanceName       
        if (type === "Teacher") {
            const group = frame.timetableEntry.group
            return (
                <div className="table-cell">
                    <div className="room-number">{frame.timetableEntry.room}</div>
                    <div className="subject-heading">{heading}</div>
                    <div className="teacher-name">{group}</div>   
                </div>
            )
        }
        else {
            const teacher = frame.timetableEntry.teacherFirstName.substring(0, 1).concat(`. ${frame.timetableEntry.teacherLastName}`)
            return (
                <div className="table-cell">
                    <div className="room-number">{frame.timetableEntry.room}</div>
                    <div className="subject-heading">{heading}</div>
                    <div className="teacher-name">{`${teacher}`}</div>
                </div>
            )
        }
    } else if (frame.timetableEntryChange) {
        const heading = frame.timetableEntryChange.subjectInstanceName
        if (type === "Teacher") {
            const group = frame.timetableEntry.group
            return (
                <div className="table-cell table-cell-change">
                    <div className="room-number">{frame.timetableEntry.room}</div>
                    <div className="subject-heading">{heading}</div>
                    <div className="teacher-name">{group}</div>
                </div>
            )
        } else {        
            const teacher = frame.timetableEntryChange.teacherFirstName.substring(0, 1).concat(`. ${frame.timetableEntry.teacherLastName}`)
            return (
                <div className="table-cell table-cell-change">
                    <div className="room-number">{frame.timetableEntryChange.room}</div>
                    <div className="subject-heading">{heading}</div>
                    <div className="teacher-name">{`${teacher}`}</div>
                </div>
            )
        }
     }
    else { 
        return (
            <div className="table-cell table-cell-empty">                
            </div>
        )
    }
 }

export default TableCell