import React from 'react'
import MaterialView from './MaterialView'

const MaterialContainer = ({ materials, deleteMaterial, info }) => {        
    const materialsList = materials.map(material => {
        return <MaterialView material={material} deleteMaterial={deleteMaterial} user={info}/>
    })

    return (
        <div className="table table-responsive table-white">
            {info ? materialsList : null}
        </div>
    )
}

export default MaterialContainer