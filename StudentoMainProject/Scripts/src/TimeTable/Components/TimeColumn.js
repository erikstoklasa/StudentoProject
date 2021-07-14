import React from 'react'
import './TableCell'
import '../TimeTable.css'


const TimeColumn = ({ frameArray }) => {
    
    const cellArray = frameArray.map((frame, index) => {
        return (
            <div className="time-cell time-cell-vertical display-linebreak">
                <div className="hour-number">{`${index + 1}.`}</div>
                <div className="hour time-text2">{frame.startTime.slice(0, -3)}<br></br> - <br></br>{frame.endTime.slice(0, -3)}</div>
            </div>
        )
    })

    cellArray.unshift(<div className="day-cell-vertical hidden">
          <h5>A</h5>
            <p className="mb0">1</p>
        </div>)

    return (
        <div className="time-column">
            {cellArray}
        </div>
    )
}

export default TimeColumn