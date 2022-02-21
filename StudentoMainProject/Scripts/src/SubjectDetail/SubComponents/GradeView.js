import React from 'react'
import GradeRow from './GradeRow'
import InfoAlert from '../../../Components/Alerts/InfoAlert'
import styled from 'styled-components'
import { WhiteTable } from '../../../Styles/GlobalStyles'

const Table = styled(WhiteTable)`
        padding: 20px 20px 10px;
`

const GradeView = ({ grades, type, deleteGrade }) => {  
 
    //create an html element for each grade recieved in props, put them into an array
    const gradeList = grades.map(grade => { 
        return <GradeRow grade={grade} type={type} deleteGrade={ deleteGrade}/>
    })

    if (grades.length > 0) {
        //display array of grade html elements
       
            return (
                <Table>
                    {gradeList}                   
                </Table>
            )
                  
    } else {
        if (type === 'teacherGrades') {
            return (
                <InfoAlert text={'Zatím ti vyučující nepřidal žádné známky🙁'}/>                
            )
        }
        if (type === 'studentGrades') {
            return (
                <InfoAlert text={'Tady si můžeš přidat svoje známky. Budou se ti počítat jen do tvého průměru (učitel tyhle známky neuvidí).'}/>                
            )
        }
    }
 }

export default GradeView