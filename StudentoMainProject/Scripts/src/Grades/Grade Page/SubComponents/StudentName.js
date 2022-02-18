import React from 'react'
import styled from 'styled-components'

const Cell = styled.div` 
    height: 40px;
    padding-top: 10px;
    padding-bottom: 10px;
    padding-left: 50px;
    padding-right: 50px;
    border-top: 1px solid lightgray;
    display: flex;
    flex-direction: column;
    justify-content: center;
    > p {
        white-space: nowrap;
        margin: 0;
        padding: 0;
    }
`

const StudentName = ({ name }) => {
    return (
        <Cell>
            <p>{name}</p>
        </Cell>
    )
}

export default StudentName