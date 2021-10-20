import React from 'react'
import './LoadingScreen.css'

const LoadingScreen = ({ relative }) => {
    if(!relative){
    return (
        <div className="loader-container">
            <div className="loader"></div>
        </div>
        )
    } else {
        return (
            <div className="loader-container relative">
                <div className="loader"></div>
            </div>
        )
    }
}

export default LoadingScreen