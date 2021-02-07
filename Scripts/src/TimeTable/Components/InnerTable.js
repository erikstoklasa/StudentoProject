import React from 'react'
import TableRow from './TableRow'

const InnerTable = ({ sortedData, week }) => {
    if (sortedData && week) {
        const actualWeek = sortedData.filter(weekObject => {
            return weekObject.week === week
        })
        console.log(actualWeek)
    
        return (
            <div className="inner-table">
                <TableRow frameArray={actualWeek[0].timeFrames[0]} />
                <TableRow frameArray={actualWeek[0].timeFrames[1]} />
                <TableRow frameArray={actualWeek[0].timeFrames[2]} />
                <TableRow frameArray={actualWeek[0].timeFrames[3]} />
                <TableRow frameArray={actualWeek[0].timeFrames[4]}/>
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