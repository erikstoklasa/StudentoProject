import React from 'react'
import TableRow from './TableRow'
import TimeRow from './TimeRow'
import TableColumn from './TableColumn' 
import TimeColumn from './TimeColumn'
import DayColumn from './DayColumn.js'
import styled from 'styled-components'

const Table = styled.div` 
    width: 100%;
    margin-left: auto;
    margin-right: auto;
    overflow: auto;
    display: flex;
`

const Horizontal = styled.div` 
    overflow: auto;
    width: 100%;
    overflow-x: overlay;
`
const Vertical = styled.div` 
    display: flex;
    width: 100%;
    overflow-x: overlay;
`
const Error = styled.div` 
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 1.5rem;
    margin-top: 150px;
    margin-bottom: 150px;
    color: lightgrey;
`

const InnerTable = ({ sortedData, week, type, displayVertical }) => {
    if (sortedData && week) {
        const actualWeek = sortedData.filter(weekObject => {
            return weekObject.week === week
        })
        const weekStart = actualWeek[0].weekStart.split('T')[0]; 
        const dateArray = weekStart.split('-').map(number => {            
            return parseInt(number)
        });
        
        //add number of days in a given month to date array
        dateArray.push(new Date(dateArray[1], dateArray[0], 0).getDate())
       


        if (!displayVertical) {
            return (
                <Table>                  
                    
                        <DayColumn data={actualWeek} dates={dateArray} />
                        <Horizontal>
                        <TimeRow frameArray={actualWeek[0].timeFrames[0]} /> 
                        <TableRow frameArray={actualWeek[0].timeFrames[0]} dayName={'Pondělí'} type={type}  dateModifier={0} dateArray={dateArray} />
                        <TableRow frameArray={actualWeek[0].timeFrames[1]} dayName={'Úterý'} type={type}  dateModifier={1} dateArray={dateArray}/>
                        <TableRow frameArray={actualWeek[0].timeFrames[2]} dayName={'Středa'} type={type}  dateModifier={2} dateArray={dateArray}/>
                        <TableRow frameArray={actualWeek[0].timeFrames[3]} dayName={'Čtvrtek'} type={type}  dateModifier={3} dateArray={dateArray}/>
                        <TableRow frameArray={actualWeek[0].timeFrames[4]} dayName={'Pátek'} type={type} dateModifier={4} dateArray={dateArray} />
                        </Horizontal>
                    
                </Table>
            )
        } else { 
            return (
                <Table>
                    <TimeColumn frameArray={actualWeek[0].timeFrames[0]} />
                    <Vertical>
                    <TableColumn frameArray={actualWeek[0].timeFrames[0]} dayName={'Pondělí'} type={type} dateModifier={0} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[1]} dayName={'Úterý'} type={type} dateModifier={1} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[2]} dayName={'Středa'} type={type} dateModifier={2} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[3]} dayName={'Čtvrtek'} type={type} dateModifier={3} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[4]} dayName={'Pátek'} type={type} dateModifier={4} dateArray={dateArray} />
                    </Vertical>
                </Table>
            )
        }
    }
    else { 
        return (
        <Error>
            Nedaří se načíst týden, prosím kontaktujte správce
        </Error>
        )
    }
}

export default InnerTable

