import React from 'react'

const MaterialView = ({ material, deleteMaterial}) => {  

    return (
        <div className="material-view-container">
            <div className="material-heading-container">               
                <img src={`/images/icons/${material.link}.png`} alt="icon" height="50px" onError={(e) => { e.target.onerror = null; e.target.src = "/images/icons/fallback.png" }}></img>
                 <div className="material-name-container">
                    <p className="material-name">{material.name}</p>
                    <p className="material-added">{material.addedRelative}</p>
                </div>                
            </div>
            <div className="material-button-container">
            <div class="btn btn-danger rm" onClick={() => {deleteMaterial(material.id)}}>Smazat</div>
                <a className="btn btn-primary" href={`/Teacher/Subjects/Materials/Details?id=${material.id}`}>Stáhnout</a>
            </div>
        </div>
    )
}

export default MaterialView