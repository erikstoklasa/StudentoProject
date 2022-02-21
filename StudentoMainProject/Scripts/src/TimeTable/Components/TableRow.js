import React from 'react'
import TableCell from './TableCell'
import styled from 'styled-components'

const Row = styled.div` 
    display: flex;
`

const TableRow = ({ frameArray, type}) => {  
   
    const cellArray = frameArray.map(frame => {
        return <TableCell frame={frame} type={type}/>
    })  

    return (
        <Row>       
            {cellArray}
        </Row>
    )
}

export default TableRow