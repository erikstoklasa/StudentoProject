import React from 'react'
import DayColumnCell from './DayColumnCell'

const DayColumn = ({ dates }) => {    
    return (
        <div className="day-column-container">
            <div className="day-column-cell-empty">
                <div class="hour-number">1.</div>
                <div class="hour">08:30</div>
            </div>
            <DayColumnCell dayName={'Pondělí'} dateModifier={0} dateArray={dates} />
            <DayColumnCell dayName={'Úterý'} dateModifier={1} dateArray={dates} />
            <DayColumnCell dayName={'Středa'} dateModifier={2} dateArray={dates} />
            <DayColumnCell dayName={'Čtvrtek'} dateModifier={3} dateArray={dates} />
            <DayColumnCell  dayName={'Pátek'} dateModifier={4} dateArray={dates} />
        </div>
    )
}

export default DayColumn