import React from 'react'
import styled from 'styled-components'

const Container = styled.div` 
    display: flex;
    flex-wrap: wrap;
`
const HeadingContainer = styled.div` 
    flex-grow: 1;
    flex-basis: 656px;  
`
const AverageContainer = styled.div` 
    flex-grow: 1;
    flex-basis: 656px;
    display: flex;
    justify-content: start;
    align-items: center;
`

const SubjectTitle = ({ info, average }) => {
    
    //display subject title, student average and teacher name
    if (info) {
        return (
            <Container>
                <HeadingContainer>
                    <h2>{info.name}</h2>                    
                </HeadingContainer>
                <AverageContainer>
                    <h2>{`Ø `}{average.toFixed(2)}</h2>
                </AverageContainer>
            </Container>
        )
    } else { 
        return(<></>)
    }
}

export default SubjectTitle