import React, { useContext } from 'react'
import { MaterialContext } from './StudentMaterial'

const MaterialView = ({ material, deleteMaterial, user }) => {
    const authors = useContext(MaterialContext)    
    
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

    const getAuthorName = () => {        
        if (user.data.typeName === "Student") {
            const userId = material.addedById
            const author = authors.find(student => student.id === userId)
            return `${author.firstName} ${author.lastName}`
        } else {
            return `${authors.firstName} ${authors.lastName}`
        }
    }    

    return (       
        <div className="material-view-container">
            <div className="material-heading-container">               
                    <img src={`/images/icons/${material.link}.png`} alt="icon" height="50px" onError={(e) => { e.target.onerror = null; e.target.src = "/images/icons/fallback.png" }}></img>
                 <div className="material-name-container">
                    <p className="material-name">{material.name}</p>
                    <p className="material-added">{`${getAuthorName()} · ${material.addedRelative}`}</p>
                </div>                
            </div>
            <div className="material-button-container">
                    { getCanDelete() ? <div class="btn btn-danger rm" onClick={() => { deleteMaterial(material.id) }}>Smazat</div> : null}
                <a className="btn btn-primary" href={`/Student/Subjects/Materials/Details?id=${material.id}`}>Stáhnout</a>
            </div>
        </div>
        
    )
}

export default MaterialView