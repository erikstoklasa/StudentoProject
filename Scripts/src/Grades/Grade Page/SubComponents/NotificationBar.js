import React from 'react'
import '../GradePage.css'

const NotificationBar = ({ data }) => {    
    if (data.show) {
        return (
            <div className="notification bar container">
                text
            </div>
        )
    }
    else {
        return (
            <div hidden></div>
        )
    }
}

export default NotificationBar