import React from 'react'
import './TableCell'
import '../TimeTable.css'


const TimeRow = ({ frameArray }) => {
    
    const cellArray = frameArray.map((frame, index) => {
        return (
            <div className="time-cell">
                <div className="hour-number">{`${index + 1}.`}</div>
                <div className="hour">{`${frame.startTime.slice(0, -3)} - ${frame.endTime.slice(0, -3)}`}</div>
            </div>
        )
    })

    return (
        <div className="time-row">
            {cellArray}
        </div>
    )
}

export default TimeRow