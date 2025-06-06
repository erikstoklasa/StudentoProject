import React from 'react'
import styled, { css } from 'styled-components'

const StyledContainer = styled.div`
    ${props => 
    props.relative ? 
        css` 
            position: relative;
            top: 40%;
            left: 50%;
            -ms-transform: translateY(-60%);
            transform: translateY(-60%);
            -ms-transform: translateX(-50%);
            transform: translateX(-50%);  
        `    
        :
        
        css` 
        position: absolute;
        top: 40%;
        left: 50%;
        -ms-transform: translateY(-60%);
        transform: translateY(-60%);
        -ms-transform: translateX(-50%);
        transform: translateX(-50%);  
        `
    }   
      
    display: flex;
    justify-content: center;
    align-items: center;
`

const StyledLoader = styled.div` 
    border: 6px solid var(--transparentGrey); 
    border-top: 6px solid var(--primaryYellowStrong);
    border-radius: 50%;
    width: 60px;
    height: 60px;
    animation: spin 2s linear infinite;
    @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
    }
`

const LoadingScreen = ({ relative = false }) => {    
    return (
        <StyledContainer relative={relative}>
            <StyledLoader></StyledLoader>
        </StyledContainer>
    )   
}

export default LoadingScreen