import React from 'react'
import apiAddress from '../variables'

const MaterialView = ({ material, info }) => {  

    return (
        <div className="material-view-container">
            <div className="material-heading-container">               
                <img src={`/images/icons/${material.link}.png`} alt="icon" height="50px"></img>
                 <div className="material-name-container">
                    <p className="material-name">{material.name}</p>
                    <p className="material-added">{material.addedRelative}</p>
                </div>                
            </div>
            <div className="material-button-container">
               
                <a className="btn btn-primary" href={`/Teacher/Subjects/Materials/Details?id=${material.id}`}>Stáhnout</a>
            </div>
        </div>
    )
}

export default MaterialView