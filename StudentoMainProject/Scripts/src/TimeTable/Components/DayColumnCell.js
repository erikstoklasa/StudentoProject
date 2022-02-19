import React, { useState, useEffect } from 'react'
import styled from 'styled-components'

const Cell = styled.div` 
    position: relative;
    margin-right: 12px;
    flex: 0.7 1 0;
    min-width: 75px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    > p {
        position: absolute;
        bottom: 0px;
    }
`

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
        <Cell>
            <h5>{dayName}</h5>
            <p>{actualDate}</p>
        </Cell>
    )
}

export default DayColumnCell