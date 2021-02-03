import React from 'react'

const InnerTable = ({ sortedData, week }) => {
    const actualWeek = sortedData.filter(weekObject => {
        return weekObject.week === week
    })

    return (
        <div></div>
    )
}

export default InnerTable