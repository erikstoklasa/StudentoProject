import React from 'react'
import TableCell from './TableCell'
import '../TimeTable.css'

const TableRow = ({ frameArray }) => {
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame}/>
    })

    return (
        <div className="table-row">
            {cellArray}
        </div>
    )
}

export default TableRow