import React from 'react'
import './Alerts.css'

const InfoTable = ({ text }) => {
    return (
        <p className="alert alert-info alert-custom">
            {text}
        </p>
    )    
}

export default InfoTable