import React, { useState } from 'react'
import { PrimaryButton } from '../../../../Styles/GlobalStyles'
import styled from 'styled-components'

const Header = styled.div` 
    cursor: pointer;
    height: 40px;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #fff;
    padding-bottom: 20px;
    margin-left: 20px;
    margin-right: 20px;
    > p {
        white-space: nowrap;
        margin: 0;
        padding: 0;
        user-select: none;
    }
`
const NewHeader = styled(Header)` 
    width:150px;
`

const Button = styled(PrimaryButton)` 
    display: flex;
    flex-wrap: nowrap;
    align-items: center;
    > p {
        white-space: nowrap;
        margin: 0;
        padding: 0;
        user-select: none;
    }
`
const AverageIcon = styled.img` 
    height: 18px;
    margin-right: 0.4rem;
    transform: scale(0.8);
`

const ColumnHeader = ({ title, type, handleClick, displayInput, onClickHeader, grade, gradeName, displayName, activatePopup }) => {
    const [displayArrow, updateDisplayArrow] = useState(false)    

    const handleAverageHeaderClick = () => {
        updateDisplayArrow(!displayArrow)
    }


    if (type === 'New Grade') {
        return (
            <NewHeader>            
                {(!displayInput ?                    
                    <Button onClick={handleClick}>                        
                    <p>{title}</p>
                    </Button>                       
                    : <p>{displayName? gradeName : null}</p>)}            
            </NewHeader>
        )
    }

    if (type === 'average' && !displayArrow) { 
        return (
            <Header onClick={() => {
                onClickHeader()
                handleAverageHeaderClick()
            }}>
                <AverageIcon src="/images/uparrow.svg"/>
                <p>{title}</p>
            </Header>
        ) 
    }

    if (type === 'average' && displayArrow) { 
        return (
            <Header onClick={() => {
                onClickHeader()
                handleAverageHeaderClick()
            }}>
                <AverageIcon src="/images/downarrow.svg"></AverageIcon>
                <p>{title}</p>
            </Header>
        ) 
    }

    else {
        return (
            <Header onClick={activatePopup}>
                <p>{title}</p>
            </Header>           
        )
    }
}

export default ColumnHeader

