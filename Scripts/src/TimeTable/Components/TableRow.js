import React from 'react'
import TableCell from './TableCell'
import '../TimeTable.css'

const TableRow = ({ frameArray, dayName, type }) => {
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame} type={type}/>
    })

    return (
        <div className="table-row">
            <h5 className="day-cell">{dayName}</h5>
            {cellArray}
        </div>
    )
}

export default TableRow