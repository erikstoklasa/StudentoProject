import React,  {useEffect, useState} from 'react'
import TableCell from './TableCell'
import styled from 'styled-components'

const Column = styled.div` 
    display: flex;
    flex: 1 1 0px;
    flex-direction: column;
`
const VerticalCell = styled.div` 
    padding: 10px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;        
    min-width: 0px !important;
    > p {
        margin-bottom: 0px;
    }
`

const TableColumn = ({ frameArray, dayName, type, dateArray, dateModifier }) => { 
    const [actualDate, updateActualDate] = useState();
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame} type={type} isVertical={true}/>
    })

    const determineDate = () => { 
        if (dateArray[2] + dateModifier <= dateArray[3]) {
            updateActualDate(`${dateArray[2] + dateModifier}.${dateArray[1]}.`)
        } else {           
            const spillOverToTheNextMonth = dateArray[3] - (dateArray[2] + dateModifier)
            const nextMonthDay = 0 - spillOverToTheNextMonth;

            updateActualDate(`${nextMonthDay}.${dateArray[1] + 1}.`)
        }
    }

    useEffect(determineDate, dateArray)

    return (
        <Column>
            <VerticalCell>
                <h5>{dayName}</h5>
                <p>{actualDate}</p>
            </VerticalCell>
            {cellArray}
        </Column>
    )
}

export default TableColumn