import React from 'react'

const MaterialInput = ({ material, changeName, index, removeFile }) => {
   
    
    const fileType = material.materialFile.name.split('.')   

    return (
        <div className="single-material-input-container" onClick={e=>e.preventDefault()}>
            <img src={`/images/icons/${fileType[1]}.png`} alt="icon" height="50px" onError={(e) => { e.target.onerror = null; e.target.src = "/images/icons/fallback.png" }}></img>
            <input className="form-control material-name-input" maxLength="100" value={material.materialName} onChange={(event) => {
                
                changeName(event.target.value, index)
            }}></input>
            <img className="pointer" src="/images/icons/delete.svg" alt="zavřít" height="15px" onClick={() => {              

                removeFile()
            }}></img>
        </div>
    )
}

export default MaterialInput

