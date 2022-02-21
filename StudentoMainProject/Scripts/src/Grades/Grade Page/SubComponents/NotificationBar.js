import React from 'react'
import styled from 'styled-components'

const Notification = styled.div` 
    height: 30px;
    width: 40%;
    max-width: 100px;
    margin-top: -40px;
    margin-left: auto;
    margin-bottom: 10px;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 5rem;  
    background-color: rgba(47,150,47,1)	;
    font-style: bold;
    color: #fff;
`

const NotificationBar = ({ data }) => {    
    if (data.show) {
        return (
            <Notification>
                Uloženo
            </Notification>
        )
    }
    else {
        return (
            <div hidden></div>
        )
    }
}

export default NotificationBar