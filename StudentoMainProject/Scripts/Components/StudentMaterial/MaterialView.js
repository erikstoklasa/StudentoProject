import React from 'react'


const MaterialView = ({ material, deleteMaterial, user }) => {
  
    
    const getCanDelete = () => {
        if (user.data.typeName === "Teacher") {
            return true
        } else {
            if (material.addedById === user.data.userId && material.addedBy !== 0) {
                return true
            } else {
                return false
            }
        }
    }
    
   

    return (
        <div>
        <div className="material-view-container">
            <div className="material-heading-container">               
                    <img src={`/images/icons/${material.link}.png`} alt="icon" height="50px" onError={(e) => { e.target.onerror = null; e.target.src = "/images/icons/fallback.png" }}></img>
                 <div className="material-name-container">
                    <p className="material-name">{material.name}</p>
                    <p className="material-added">{material.addedRelative}</p>
                </div>                
            </div>
            <div className="material-button-container">
                    { getCanDelete() ? <div class="btn btn-danger rm" onClick={() => { deleteMaterial(material.id) }}>Smazat</div> : null}
                <a className="btn btn-primary" href={`/Student/Subjects/Materials/Details?id=${material.id}`}>Stáhnout</a>
            </div>
            </div>
            </div>
    )
}

export default MaterialView