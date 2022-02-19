import React from 'react'
import { StyledErrorAlert } from '../../Styles/GlobalStyles'

const ErrorTable = ({ text }) => {
    return (
        <StyledErrorAlert>
            {text}
        </StyledErrorAlert>
    )    
}

export default ErrorTable