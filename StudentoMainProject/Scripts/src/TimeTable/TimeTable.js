import React, { useState, useEffect } from 'react'
import { PrimaryButton } from '../../Styles/GlobalStyles.js'
import { apiAdress } from './Variables.js'
import InnerTable from './Components/InnerTable.js'
import styled from 'styled-components'

const Table = styled.div` 
    background-color: white;
    padding-top: 20px;
    padding-left: 15px;
    padding-right: 15px;
    padding-bottom: 20px;
    box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16);
    font-size: 14px;
`
const Control = styled.div` 
    display: flex;
    flex-wrap: nowrap;
    width: 100%;
    justify-content: flex-end;
    align-items: center; 
`
const Heading = styled.h1` 
    margin: 0px 0px 0px 15px;    
    @media(max-width: 450px){
        display:none;
    }
`
const ButtonContainer = styled.div` 
    margin-right: auto;
    margin-left: 0px;
    display: flex;
    flex-wrap: nowrap;
`
const ArrowContainer = styled.div` 
    display: flex;
    align-items: center;
    margin: 0px 0px 0px 10px !important;
`
const Button = styled(PrimaryButton)` 
    margin: 0px 10px;
`
const Arrow = styled.img` 
    padding: 0px;
    cursor: pointer;
    height: 25px;
    width: 25px;
    user-select: none;
    @media(min-width: 450px){
        margin-right: 10px;
    }
`

const LeftArrow = styled(Arrow)` 
    transform: scaleX(-1);
`

const TimeTable = () => {

    const [displayVertical, updateDisplayVertical] = useState(true);
    const [tableType, updateTableType] = useState();
    const [currentWeek, updateCurrentWeek] = useState();
    const [bulkData, updateBulkData] = useState();
    const [sortedData, updateSortedData] = useState();
    const [initialWeek, updateInitialWeek] = useState();
    

    const getType = () => {
        const adress = window.location.pathname
        const isItStudent = adress.includes('Student')
        if (isItStudent) {
            updateTableType('Student')
        }
        if (!isItStudent) {
            
            updateTableType('Teacher')
        }
    }

    const fetchData = (weekNumber) => {
        if (tableType && !weekNumber) {
            fetch(`${apiAdress}/Timetable/${tableType}?week=&wantMultipleWeeks=true`)
                .then(res => res.json())
                .then(data => {
                    updateCurrentWeek(data[0].week)
                    updateBulkData(data)
                })
        } else if (tableType && weekNumber) {
            
            fetch(`${apiAdress}/Timetable/${tableType}?week=${weekNumber}&wantMultipleWeeks=false`)
            .then(res => res.json())
                .then(data => {
                    sortData(data)
                }).then(() => {                    
                    updateCurrentWeek(weekNumber)
                })
        }

    }

    const sortData = (data) => {
       
        if (bulkData && !data) {
            //clone bulk adn edit start and end times
            const newData = bulkData.map(weekObject => {
                weekObject.timeFrames.forEach(timeFrame => {
                    timeFrame.startTime = timeFrame.startTime.substring(timeFrame.startTime.indexOf("T") + 1);
                    timeFrame.endTime = timeFrame.endTime.substring(timeFrame.endTime.indexOf("T") + 1);
                });
                return weekObject
            })
            //sort by time and days
            newData.forEach(weekObject => {
                const daysSorted = [[], [], [], [], []]
                weekObject.timeFrames.forEach(timeFrame => {
                    daysSorted[timeFrame.dayOfWeek - 1].push(timeFrame)
                })
                daysSorted.forEach(day => {
                    day.sort((a, b) => { a.startTime.localeCompare(b.startTime) })
                })
                weekObject.timeFrames = daysSorted
            });
            updateSortedData(newData)
            updateInitialWeek(newData[0].week)
        } else if (data) {            
            const newData = data.map(weekObject => {
                weekObject.timeFrames.forEach(timeFrame => {
                    timeFrame.startTime = timeFrame.startTime.substring(timeFrame.startTime.indexOf("T") + 1);
                    timeFrame.endTime = timeFrame.endTime.substring(timeFrame.endTime.indexOf("T") + 1);
                });
                return weekObject
            })
            //sort by time and days
            newData.forEach(weekObject => {
                const daysSorted = [[], [], [], [], []]
                weekObject.timeFrames.forEach(timeFrame => {
                    daysSorted[timeFrame.dayOfWeek - 1].push(timeFrame)
                })
                daysSorted.forEach(day => {
                    day.sort((a, b) => { a.startTime.localeCompare(b.startTime) })
                })
                weekObject.timeFrames = daysSorted
            });

            const finalData = sortedData.concat(newData) 
            updateSortedData(finalData)            
        }
    }
    
    const changeWeek = (number) => { 
       
        const nextWeek = (currentWeek + number)
        if (sortedData.some(weekObject => weekObject.week === nextWeek)) {
            updateCurrentWeek(nextWeek)
        } else { 
            fetchData(nextWeek)
           
        }
    }

    useEffect(getType, [])
    useEffect(fetchData, tableType)
    useEffect(sortData, bulkData)

    
    return (
        <Table>            
            <Control>
                <Heading>Můj rozvrh</Heading>
                <ButtonContainer>
                    <Button onClick={() => { updateDisplayVertical(!displayVertical) }}>Změnit zobrazení</Button>                    
                    {currentWeek !== initialWeek ? <PrimaryButton onClick={() => { updateCurrentWeek(initialWeek) }}>Aktualní týden</PrimaryButton> : null}
                </ButtonContainer>
                <ArrowContainer>                    
                    <LeftArrow  src='/images/rightarrow.svg' onClick={() => { changeWeek(-1) }}/>                 
                    <Arrow src='/images/rightarrow.svg' onClick={() => {changeWeek(+1)}}/>                  
                </ArrowContainer>    
            </Control>
            <InnerTable sortedData={sortedData} week={currentWeek} type={tableType} displayVertical={displayVertical}/>                     
        </Table>
    )
 }

 
export default TimeTable