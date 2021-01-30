import React, { useState, useEffect } from 'react'
import { apiAdress } from './Variables.js'

const TimeTable = () => {

    const [tableType, updateTableType] = useState()

    const determineTypeFromUrl = () => {
        const adress = window.location.pathname
        const isItStudent = adress.includes('Student')
        if (isItStudent) { 
            updateTableType('student')
        }
        if (!isItStudent) {
            updateTableType('teacher')
        }
    }
    
    const fetchData = () => {
        fetch(adress, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(weekNumber)
            
        }).then(res => res.json()).then(data => console.log(data))
    }

    useEffect(fetchData, tableType)

    

    //fetchData();
    return (
        <div>Table
            <button onClick={() => { console.log(adress)}}>Check</button>
        </div>
    )
 }

 
export default TimeTable