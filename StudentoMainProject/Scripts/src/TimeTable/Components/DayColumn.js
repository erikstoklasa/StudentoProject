import React from 'react'
import DayColumnCell from './DayColumnCell'
import styled from 'styled-components'

const Column = styled.div` 
    display: flex;
    flex-direction: column;
`
const EmptyCell = styled.div` 
    margin: 3px;
    border: 2px solid transparent;
    padding: 10px 10px 5px 10px;
    display: flex;
    flex-direction: column;    
    justify-content: center;
    align-items: center;
    visibility: hidden;
`
const Number = styled.div` 
    font-weight: bold;    
`
const Time = styled.div` 
    font-size: 12px;
    margin-top: 5px;
    min-width: 0;
`

const DayColumn = ({ dates }) => {    
    return (
        <Column>
            <EmptyCell>
                <Number>1.</Number>
                <Time>08:30</Time>
            </EmptyCell>
            <DayColumnCell dayName={'Pondělí'} dateModifier={0} dateArray={dates} />
            <DayColumnCell dayName={'Úterý'} dateModifier={1} dateArray={dates} />
            <DayColumnCell dayName={'Středa'} dateModifier={2} dateArray={dates} />
            <DayColumnCell dayName={'Čtvrtek'} dateModifier={3} dateArray={dates} />
            <DayColumnCell  dayName={'Pátek'} dateModifier={4} dateArray={dates} />
        </Column>
    )
}

export default DayColumn