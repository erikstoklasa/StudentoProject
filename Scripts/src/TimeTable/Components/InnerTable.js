import React from 'react'
import TableRow from './TableRow'
import TimeRow from './TimeRow'

const InnerTable = ({ sortedData, week, type }) => {
    if (sortedData && week) {
        const actualWeek = sortedData.filter(weekObject => {
            return weekObject.week === week
        })
        const weekStart = actualWeek[0].weekStart.split('T')[0]; 
        const dateArray = weekStart.split('-').map(number => {            
            return parseInt(number)
        });        

        return (
            <div className="inner-table">
                <TimeRow frameArray={actualWeek[0].timeFrames[0]}/>
                <TableRow frameArray={actualWeek[0].timeFrames[0]} dayName={'Pondělí'} type={type} date={`${dateArray[2]}.${dateArray[1]}`} />
                <TableRow frameArray={actualWeek[0].timeFrames[1]} dayName={'Úterý'} type={type} date={`${parseInt(dateArray[2]) + 1}.${dateArray[1]}`}/>
                <TableRow frameArray={actualWeek[0].timeFrames[2]} dayName={'Středa'} type={type} date={`${dateArray[2] + 2}.${dateArray[1]}`}/>
                <TableRow frameArray={actualWeek[0].timeFrames[3]} dayName={'Čtvrtek'} type={type} date={`${dateArray[2] + 3}.${dateArray[1]}`}/>
                <TableRow frameArray={actualWeek[0].timeFrames[4]} dayName={'Pátek'} type={type} date={`${dateArray[2] + 4}.${dateArray[1]}`}/>
            </div>
        )
    }
    else { 
        return (
            <div>no week</div>
        )
    }
}

export default InnerTable