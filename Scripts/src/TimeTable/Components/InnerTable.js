import React from 'react'
import TableRow from './TableRow'
import TimeRow from './TimeRow'

const InnerTable = ({ sortedData, week, type }) => {
    if (sortedData && week) {
        const actualWeek = sortedData.filter(weekObject => {
            return weekObject.week === week
        })
        console.log(actualWeek)
        return (
            <div className="inner-table">
                <TimeRow frameArray={actualWeek[0].timeFrames[0]}/>
                <TableRow frameArray={actualWeek[0].timeFrames[0]} dayName={'Pondělí'} type={type}/>
                <TableRow frameArray={actualWeek[0].timeFrames[1]} dayName={'Úterý'} type={type}/>
                <TableRow frameArray={actualWeek[0].timeFrames[2]} dayName={'Středa'} type={type}/>
                <TableRow frameArray={actualWeek[0].timeFrames[3]} dayName={'Čtvrtek'} type={type}/>
                <TableRow frameArray={actualWeek[0].timeFrames[4]} dayName={'Pátek'} type={type}/>
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