import React from 'react'
import TableRow from './TableRow'
import TimeRow from './TimeRow'
import TableColumn from './TableColumn' 
import TimeColumn from './TimeColumn'

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
                <div className="inner-table">
                    <TimeRow frameArray={actualWeek[0].timeFrames[0]} />
                    <TableRow frameArray={actualWeek[0].timeFrames[0]} dayName={'Pondělí'} type={type}  dateModifier={0} dateArray={dateArray} />
                    <TableRow frameArray={actualWeek[0].timeFrames[1]} dayName={'Úterý'} type={type}  dateModifier={1} dateArray={dateArray}/>
                    <TableRow frameArray={actualWeek[0].timeFrames[2]} dayName={'Středa'} type={type}  dateModifier={2} dateArray={dateArray}/>
                    <TableRow frameArray={actualWeek[0].timeFrames[3]} dayName={'Čtvrtek'} type={type}  dateModifier={3} dateArray={dateArray}/>
                    <TableRow frameArray={actualWeek[0].timeFrames[4]} dayName={'Pátek'} type={type} dateModifier={4} dateArray={dateArray}/>
                </div>
            )
        } else { 
            return (
                <div className="inner-table inner-table-vertical">
                    <TimeColumn frameArray={actualWeek[0].timeFrames[0]} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[0]} dayName={'Pondělí'} type={type} dateModifier={0} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[1]} dayName={'Úterý'} type={type} dateModifier={1} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[2]} dayName={'Středa'} type={type} dateModifier={2} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[3]} dayName={'Čtvrtek'} type={type} dateModifier={3} dateArray={dateArray} />
                    <TableColumn frameArray={actualWeek[0].timeFrames[4]} dayName={'Pátek'} type={type} dateModifier={4} dateArray={dateArray} />
                </div>
            )
        }
    }
    else { 
        return (
        <div className="empty-container">
            <div>Nedaří se načíst týden, prosím kontaktujte správce</div>
        </div>
        )
    }
}

export default InnerTable

