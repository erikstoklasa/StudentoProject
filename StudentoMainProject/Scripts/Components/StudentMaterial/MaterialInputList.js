import React from 'react'
import MaterialInput from './MaterialInput'
import styled from 'styled-components'

const StyledList = styled.div` 
    margin-top: 10px;
`

const MaterialInputList = ({ materials, removeFile, changeName }) => {
    
    const materialList = materials.map((material, i) => {
        return (
            <MaterialInput material={material} removeFile={removeFile} changeName={changeName} index={i}/>
        )
    })

    return (
        <StyledList>
            {materialList}
        </StyledList>
    )

}

export default MaterialInputList