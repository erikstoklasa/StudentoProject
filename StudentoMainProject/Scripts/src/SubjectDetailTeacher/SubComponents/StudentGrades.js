import React from 'react'
import GradeView from './GradeView.js'
import { PrimaryButton, SecondaryButton } from '../../../Styles/GlobalStyles.js'
import styled from 'styled-components'

const Container = styled.div` 
    flex-grow: 1;
    flex-basis: 500px;
    @media(max-width: 1332px){
        max-width: 1300px;
    }  
`
const InnerContainer = styled.div` 
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    align-items: center;
    margin: 10px 0px 10px 0px;
`
const Heading = styled.p` 
    margin-top: 10px;
    margin-bottom: 10px;
    white-space: nowrap;
    margin-right: 25px;
    color: var(--grey); 
    flex-grow: 1; 
`
const ButtonContainer = styled.div` 
    display: flex;
    flex-grow: 1;
    justify-content: flex-end;
`
const Link = styled.a` 
    padding: 0;
    :hover{
        box-shadow: none;
        background: none;
    }
`
const PrintLink = styled(Link)`
    margin-right: 10px;
`

const StudentGrades = ({ students, info }) => {
    
    // display grades
    return (
        <Container>
            <InnerContainer>
                <Heading>Studenti</Heading>
                <ButtonContainer>
                    <PrintLink href={`/Teacher/Subjects/Details?id=${info.id}&amp;handler=print`}><SecondaryButton><img src="/images/print.svg" alt="Vytisknout" height="20px" class="btn-icon" />Tisk výpisu</SecondaryButton></PrintLink>
                    <Link href={`/Teacher/Grades?SubjectInstanceId=${info.id}`}><PrimaryButton>Zobrazit známky</PrimaryButton></Link>
                </ButtonContainer>
            </InnerContainer>            
            <GradeView students={students} info={info} type={'teacherGrades'}/>                      
        </Container>
    )
}

export default StudentGrades


