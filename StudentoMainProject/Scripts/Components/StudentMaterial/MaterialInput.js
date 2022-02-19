import React from 'react'
import { Input, StyledCloseIcon } from '../../Styles/GlobalStyles'
import styled from 'styled-components'

const StyledContainer = styled.div` 
    display: flex;
    justify-content: space-around;
    align-items: center;
    margin-bottom: 10px;
`

const StyledFileIcon = styled.img` 
    height: 50px;
    cursor: default;
`
const FileNameInput = styled(Input)` 
    margin: 0px 10px;
`
const StyledMaterialCloseIcon = styled(StyledCloseIcon)` 
    height: 25px;
` 

const MaterialInput = ({ material, changeName, index, removeFile }) => {  
    
    const fileType = material.materialFile.name.split('.')   

    return (
        <StyledContainer onClick={e=>e.preventDefault()}>
            <StyledFileIcon src={`/images/icons/${fileType[1]}.png`} alt="icon" onError={(e) => { e.target.onerror = null; e.target.src = "/images/icons/fallback.png" }}></StyledFileIcon>
            <FileNameInput maxLength="100" value={material.materialName} onChange={(event) => {                
                changeName(event.target.value, index)
            }}></FileNameInput>
            <StyledMaterialCloseIcon src="/images/icons/delete.svg" alt="zavřít" height="15px" onClick={() => { removeFile() }}></StyledMaterialCloseIcon>
        </StyledContainer>
    )
}

export default MaterialInput

