import React from 'react'
import MaterialView from './MaterialView'
import InfoAlert from '../Alerts/InfoAlert'
import ErrorAlert from '../Alerts/ErrorAlert'
import { WhiteTable } from '../../Styles/GlobalStyles'
import styled from 'styled-components'

const StyledTable = styled(WhiteTable)` 
    padding: 20px 20px 10px;
    max-height: 500px;
    overflow: auto;
`

const MaterialContainer = ({ materials, deleteMaterial, info }) => {  
   
    if (materials.loaded) {
        
        if (materials.data.length > 0) {
            const materialsList = materials.data.map(material => {
                return <MaterialView material={material} deleteMaterial={deleteMaterial} user={info}/>
            })
            return (
                <StyledTable>
                    {info ? materialsList : null}
                </StyledTable>
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