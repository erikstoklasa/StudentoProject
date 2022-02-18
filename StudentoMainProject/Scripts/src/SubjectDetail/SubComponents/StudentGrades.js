import React from 'react'
import GradeView from './GradeView.js'
import ErrorAlert from '../../../Components/Alerts/ErrorAlert.js'
import { PrimaryButton } from '../../../Styles/GlobalStyles.js'
import styled from 'styled-components'

const Grades = styled.div` 
    flex-basis: 500px;
    flex-grow: 1; 
`
const Heading = styled.p` 
    margin-top: 10px;
    margin-bottom: 10px;
    white-space: nowrap;    
    color: var(--grey);
    text-align: start;
`
const HeadingContainer = styled.div` 
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    align-items: center;     
    margin: 10px 0px 10px 0px;
`

const StudentGrades = ({ grades, showPopup, deleteGrade }) => {
    
        if (grades.loaded) {
            //filter grades for grades added by student
            const studentGrades = grades.data.filter(grade => {
                return grade.addedBy === 1
            })

            //filter grades for grades added by teacher
            const teacherGrades = grades.data.filter(grade => {
                return grade.addedBy === 0
            })
        
            return (
                <Grades>
                    <Heading>Známky od vyučující/ho</Heading>
                    <GradeView grades={teacherGrades} type={'teacherGrades'} />
                    <HeadingContainer>
                        <Heading>Známky přidáné mnou</Heading>
                        <PrimaryButton onClick={() => { showPopup() }}>Přidat známku</PrimaryButton>
                    </HeadingContainer>
                    <GradeView grades={studentGrades} type={'studentGrades'} deleteGrade={deleteGrade} />
                </Grades>
            )

        } else {
            return (
                <Grades>
                    <Heading>Známky</Heading>
                    <ErrorAlert text={'Nepodařilo se načíst známky 🙁'} />
                </Grades>
            )
        }
    
}

export default StudentGrades


