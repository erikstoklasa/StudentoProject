import React from 'react'
import FillerGrade from './FillerGrade'
import ColumnHeader from './ColumnHeader'
import styled from 'styled-components'

const Column = styled.div` 
    width: 100%;
`

const FillerColumn = ({ students }) => {
    const cellArray = students.map(student => { 
        return <FillerGrade key={student.id} id={student.id}/>
    })
    return (
        <Column>
            <ColumnHeader title={''}/>
            {cellArray}
        </Column>
    )
}
export default FillerColumn