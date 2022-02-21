import React from 'react'
import './TableCell'
import styled from 'styled-components'

const HiddenCell = styled.div` 
    padding: 10px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;        
    min-width: 0px !important;
    visibility: hidden;
    > p {
        margin-bottom: 0px;
    }
`

const Column = styled.div` 
    display: flex;
    flex-direction: column;
`

const TimeCellVertical = styled.div` 
    margin: 3px;
    border: 2px solid transparent;
    padding: 10px 10px 5px 10px;
    display: flex;
    flex-direction: column;
    flex: 1 1 0px;
    min-width: 100px;
    justify-content: center;
    align-items: center;
    min-width: 0px !important;
    flex: 0.7 1 0px;
`
const Hour = styled.div` 
    font-weight: bold;
    font-weight: 1rem;
`
const Time = styled.div` 
    font-size: 12px;
    margin-top: 5px;
    min-width: 0;
    word-wrap: break-word !important;
    text-align: center;
    margin: 0;
    min-width: 0px;
`

const TimeColumn = ({ frameArray }) => {
    
    const cellArray = frameArray.map((frame, index) => {
        return (
            <TimeCellVertical>
                <Hour>{`${index + 1}.`}</Hour>
                <Time>{frame.startTime.slice(0, -3)}<br></br> - <br></br>{frame.endTime.slice(0, -3)}</Time>
            </TimeCellVertical>
        )
    })

    cellArray.unshift(
        <HiddenCell>
            <h5>A</h5>
            <p>1</p>
        </HiddenCell>
    )

    return (
        <Column>
            {cellArray}
        </Column>
    )
}

export default TimeColumn