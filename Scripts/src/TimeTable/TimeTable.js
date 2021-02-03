import React, { useState, useEffect } from 'react'
import { apiAdress } from './Variables.js'
//import moment from 'moment'
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

    const getWeek = () => { 
        //const now = moment().week()
        updateCurrentWeek(6)
        fetchData(6, true)      
    }  
    
    const fetchData = (weekNumber, wantMultiple) => {          
        if (tableType) {            
            fetch(`${apiAdress}/Timetable/${tableType}?week=${weekNumber}&wantMultipleWeeks=${wantMultiple}`)
                .then(res => res.json())
                .then(data => {
                    updateBulkData(data)
                })
        }
    }

    const sortData = () => {        
        if (bulkData) {            
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
                    day.sort((a, b) => {a.startTime.localeCompare(b.startTime) })
                })
                weekObject.timeFrames = daysSorted
            });

            updateSortedData(newData)
        }
    }

    useEffect(getType, [])
    useEffect(getWeek, tableType)
    useEffect(sortData, bulkData)

    
    return (
        <div className="outer-table">
            <InnerTable sortedData={sortedData} week={ currentWeek}/>
        </div>
    )
 }

 
export default TimeTable