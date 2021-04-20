import React from 'react'
import '../TimeTable.css'

const TableCell = ({ frame, type, extraClasses = '' }) => {
    if (frame.timetableEntryChange && frame.timetableEntryChange.cancelled) {
        //return canceled
        return (
            <div className={`table-cell table-cell-change ${extraClasses}`}>

            </div>
        )
    } else if (frame.timetableEntryChange && !frame.timetableEntryChange.cancelled) {
        //return change

        if (parseInt(frame.timetableEntryChange.subjectInstanceId > 0)) {
            if (type === "Teacher") {
                const group = frame.timetableEntry.group
                return (
                    <div className={`table-cell table-cell-change ${extraClasses}`}>
                        <div className="room-number">{frame.timetableEntry.room}</div>
                        <div className="subject-heading">{heading}</div>
                        <div className="teacher-name">{group}</div>
                    </div>
                )
            }
            else {
                const teacher = frame.timetableEntryChange.teacherFirstName.substring(0, 1).concat(`. ${frame.timetableEntry.teacherLastName}`)
                return (
                    <div className={`table-cell table-cell-change ${extraClasses}`}>
                        <div className="room-number">{frame.timetableEntryChange.room}</div>
                        <div className="subject-heading">{heading}</div>
                        <div className="teacher-name">{`${teacher}`}</div>
                    </div>
                )
            }
        } else {
            return (
            
                <div className={`table-cell table-cell-empty ${extraClasses}`}>                
                </div>
            
        )
        }

        
    } else if (frame.timetableEntry) {
        //return normal entry
        const heading = frame.timetableEntry.subjectInstanceName
        if (type === "Teacher") {
            const group = frame.timetableEntry.group
            return (
                <div className={`table-cell ${extraClasses}`}>
                    <div className="room-number">{frame.timetableEntry.room}</div>
                    <div className="subject-heading">{heading}</div>
                    <div className="teacher-name">{group}</div>
                </div>
            )
        }
        else {
            const teacher = frame.timetableEntry.teacherFirstName.substring(0, 1).concat(`. ${frame.timetableEntry.teacherLastName}`)
            return (
                <div className={`table-cell ${extraClasses}`}>
                    <div className="room-number">{frame.timetableEntry.room}</div>
                    <div className="subject-heading">{heading}</div>
                    <div className="teacher-name">{`${teacher}`}</div>
                </div>
            )
        }
    } else { 
        return (
            
                <div className={`table-cell table-cell-empty ${extraClasses}`}>                
                </div>
            
        )
    }   
  
 }

export default TableCell