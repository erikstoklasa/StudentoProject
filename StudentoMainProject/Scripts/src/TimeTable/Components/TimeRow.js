import React from 'react'
import './TableCell'
import styled from 'styled-components'

const Row = styled.div` 
    display: flex;
    max-height: 80px;
`
const Cell = styled.div` 
    margin: 3px;
    border: 2px solid transparent;
    padding: 10px 10px 5px 10px;
    display: flex;
    flex-direction: column;
    flex: 1 1 0px;
    min-width: 100px;
    justify-content: center;
    align-items: center;
`
const Hour = styled.div` 
    font-weight: bold;
    font-weight: 1rem;
`
const Time = styled.div` 
    font-size: 12px;
    margin-top: 5px;
    min-width: 0;
`

const TimeRow = ({ frameArray }) => {
    
    const cellArray = frameArray.map((frame, index) => {
        return (
            <Cell>
                <Hour>{`${index + 1}.`}</Hour>
                <Time>{`${frame.startTime.slice(0, -3)} - ${frame.endTime.slice(0, -3)}`}</Time>
            </Cell>
        )
    })

    return (
        <Row>
            {cellArray}
        </Row>
    )
}

export default TimeRow