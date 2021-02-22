import React from 'react'
import TableCell from './TableCell'
import '../TimeTable.css'

const TableColumn = ({frameArray, dayName, type, date }) => { 
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame} type={type} extraClasses={'table-cell-vertical'}/>
    })
    return (
        <div className="table-column">
            <div className="day-cell day-cell-vertical">
                <h5 >{dayName}</h5>
                <p className="row-date">{date}</p>
        </div>
            {cellArray}
        </div>
    )
}

export default TableColumn