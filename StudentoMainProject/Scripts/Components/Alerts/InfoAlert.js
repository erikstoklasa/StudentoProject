import React from 'react'
import { StyledInfoAlert } from '../../Styles/GlobalStyles'

const InfoTable = ({ text }) => {
    return (
        <StyledInfoAlert>
            {text}
        </StyledInfoAlert>
    )    
}

export default InfoTable