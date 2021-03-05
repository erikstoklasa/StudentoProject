import React,  {useEffect, useState} from 'react'
import TableCell from './TableCell'
import '../TimeTable.css'

const TableColumn = ({ frameArray, dayName, type, dateArray, dateModifier }) => { 
    const [actualDate, updateActualDate] = useState();
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame} type={type} extraClasses={'table-cell-vertical'}/>
    })

    const determineDate = () => { 
        if (dateArray[2] + dateModifier <= dateArray[3]) {
            updateActualDate(`${dateArray[2] + dateModifier}.${dateArray[1]}`)
        } else {           
            const spillOverToTheNextMonth = dateArray[3] - (dateArray[2] + dateModifier)
            const nextMonthDay = 0 - spillOverToTheNextMonth;

            updateActualDate(`${nextMonthDay}.${dateArray[1] + 1}`)
        }
    }

    useEffect(determineDate, dateArray)

    return (
        <div className="table-column">
            <div className="day-cell day-cell-vertical">
                <h5 >{dayName}</h5>
                <p className="row-date">{actualDate}</p>
        </div>
            {cellArray}
        </div>
    )
}

export default TableColumn