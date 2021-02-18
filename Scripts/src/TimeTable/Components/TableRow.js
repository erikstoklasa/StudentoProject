import React from 'react'
import TableCell from './TableCell'
import '../TimeTable.css'

const TableRow = ({ frameArray, dayName, type, date }) => {  
   
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame} type={type}/>
    })

    return (
        <div className="table-row">
        <div className="day-cell">
                <h5 >{dayName}</h5>
                <p className="row-date">{date}</p>
        </div>
            {cellArray}
        </div>
    )
}

export default TableRow