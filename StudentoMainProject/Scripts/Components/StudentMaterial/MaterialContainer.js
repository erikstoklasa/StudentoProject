import React from 'react'
import MaterialView from './MaterialView'
import InfoAlert from '../Alerts/InfoAlert'
import ErrorAlert from '../Alerts/ErrorAlert'

const MaterialContainer = ({ materials, deleteMaterial, info }) => {        
   
    if (materials.loaded) {
        
        if (materials.data.length > 0) {
            const materialsList = materials.data.map(material => {
                return <MaterialView material={material} deleteMaterial={deleteMaterial} user={info}/>
            })
            return (
                <div className="table table-responsive table-white subject-detail-table">
                    {info ? materialsList : null}
                </div>
            )
        } else {
            return (
                <InfoAlert text={"Zatím jsi nepřidal/a žádné studijní materiály 🙁"}/>                
            )
        }
    } else {
        return (
            <ErrorAlert text={"Nepodařilo se načíst materiály 🙁"}/>                
        ) 
    }
}

export default MaterialContainer