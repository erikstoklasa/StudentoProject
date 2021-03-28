import React from 'react'
import MaterialView from './MaterialView'

const MaterialContainer = ({ materials, info }) => {
    
    const materialsList = materials.map(material => {
        return <MaterialView material={ material } info={ info }/>
    })

    return (
        <div className="table table-responsive table-white">
            {materialsList}
        </div>
    )
}

export default MaterialContainer