import React from 'react'
import './Alerts.css'

const ErrorTable = ({ text }) => {
    return (
        <p className="alert alert-danger alert-custom">
            {text}
        </p>
    )    
}

export default ErrorTable