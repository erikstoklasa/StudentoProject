import React from 'react'
import { WhiteTable } from '../../../Styles/GlobalStyles'
import InfoAlert from '../../../Components/Alerts/InfoAlert'
import GradeRow from './GradeRow'
import styled from 'styled-components'

const Table = styled(WhiteTable)` 
    padding: 20px 15px 20px 15px;
    max-height: 500px;
    overflow: auto;
`

const GradeView = ({ students, info}) => {
 
    //create an html element for each student recieved in props, put them into an array
    const studentList = students.map(student => { 
        return <GradeRow student={student} info={info}/>
    })

    if (students) {
        //display array of student html elements       
            return (
                <Table>
                    {studentList}                   
                </Table>
            )
                  
    } else {        
            return (                
                <InfoAlert text={'Zatím žádní studenti 🙁'}/>                
            )
           
    }
 }

export default GradeView