import React, { useState, useEffect } from 'react'
import { apiAdress } from './Variables.js'
import InnerTable from './Components/InnerTable.js'
import './TimeTable.css'

const TimeTable = () => {

    const [tableType, updateTableType] = useState()
    const [currentWeek, updateCurrentWeek] = useState()
    const [bulkData, updateBulkData] = useState()
    const [sortedData, updateSortedData] = useState()

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
                }).then(() => { updateCurrentWeek(weekNumber) })
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
        <div className="outer-table">
            <div className="top-bar">
                <h1 className="table-heading">Můj rozvrh</h1>
                <div className="button-container">                    
                    <div className="week-button-prev" onClick={() => { changeWeek(-1) }}><img  className="arrow flip-horizontally" src='/images/rightarrow.svg'></img></div>
                    <div className="week-button-prev" onClick={() => { changeWeek(+1)}}><img className="arrow " src='/images/rightarrow.svg'></img></div>                    
                </div>    
            </div>
            <InnerTable sortedData={sortedData} week={currentWeek} type={tableType}/>                     
        </div>
    )
 }

 
export default TimeTable