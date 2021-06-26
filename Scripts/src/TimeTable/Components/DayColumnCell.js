import React, {useState, useEffect} from 'react'

const DayColumnCell = ({ dayName, dateArray, dateModifier }) => {
    const [actualDate, updateActualDate] = useState();

    const determineDate = () => {
        //check if all days fit into current month
        if (dateArray[2] + dateModifier <= dateArray[3]) {
            updateActualDate(`${dateArray[2] + dateModifier}.${dateArray[1]}`)
        } else {
            //not fit, determine date
            const spillOverToTheNextMonth = dateArray[3] - (dateArray[2] + dateModifier)
            const nextMonthDay = 0 - spillOverToTheNextMonth;

            updateActualDate(`${nextMonthDay}.${dateArray[1] + 1}`)
        }
    }

    useEffect(determineDate, dateArray)


    return (
        <div className="day-cell day-cell-horizontal">
        <h5 >{dayName}</h5>
        <p className="row-date">{actualDate}</p>
        </div>
    )
}

export default DayColumnCell