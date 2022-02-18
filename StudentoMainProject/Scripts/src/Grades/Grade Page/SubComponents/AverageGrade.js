import React from 'react'
import { Cell } from '../SharedStyles'
import styled from 'styled-components'

const Average = styled.p`  
    font-weight: bold;
    margin: 0;
    padding: 0;
`

const AverageGrade = ({ grade }) => {
    return (
    <Cell>
        {grade ? <Average>{grade}</Average> : null}           
    </Cell>   
    )
}

export default AverageGrade