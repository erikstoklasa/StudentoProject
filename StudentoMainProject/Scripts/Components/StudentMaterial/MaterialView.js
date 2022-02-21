import React, { useContext } from 'react'
import { MaterialContext } from './StudentMaterial'
import styled from 'styled-components'

const StyledContainer = styled.div` 
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
    gap: 20px;
`
const HeadingContainer = styled.div` 
    display: flex;
    align-items: center;     
    overflow: hidden;
    text-overflow: ellipsis;
    flex-grow: 1;
`
const StyledFileIcon = styled.img` 
    height: 50px;
    cursor: default;
`
const StyledNameContainer = styled.div` 
    margin-left: 10px;
`
const StyledName = styled.p`
    margin: 0 !important;
    font-weight: 500;
    min-width: 30px;    
    overflow: hidden;
    text-overflow: ellipsis;
`
const StyledDate = styled.p`
    margin: 0;
    font-size: 0.8rem;    
    color: var(--grey);
`
const StyledIconContainer = styled.div`     
    display: flex;
    gap: 15px;
`

const StyledIcon = styled.img` 
    cursor: pointer;
    width: 1.5rem;
`

const MaterialView = ({ material, deleteMaterial, user }) => {    
    const authors = useContext(MaterialContext)      
    
    const getCanDelete = () => {
        if (user.data.typeName === "Teacher") {
            return true
        } else {
            if (material.addedById === user.data.userId && material.addedBy !== 0) {
                return true
            } else {
                return false
            }
        }
    }  

    const getAuthorName = () => {        
        if (user.data.typeName === "Student") {
            if (material.addedBy === 0) {
                return `${authors.teacher.firstName} ${authors.teacher.lastName}`
            } else {
                const authorId = material.addedById                
                const author = authors.students.find(student => student.id === authorId)
                if (author) {
                    return `${author.firstName} ${author.lastName}`
                } else {
                    return ''
                }
            }            
        } else {
            //teacher always returns his name
            return `${user.data.firstName} ${user.data.lastName}`
        }
    }    

    return (       
        <StyledContainer>
            <HeadingContainer>               
                <StyledFileIcon src={`/images/icons/${material.link}.png`} alt="icon" onError={(e) => { e.target.onerror = null; e.target.src = "/images/icons/fallback.png" }}></StyledFileIcon>
                 <StyledNameContainer>
                    <StyledName>{material.name}</StyledName>
                    <StyledDate>{`${getAuthorName()} · ${material.addedRelative}`}</StyledDate>
                </StyledNameContainer>                
            </HeadingContainer>
            <StyledIconContainer>                    
                {getCanDelete() ?
                    <a>
                        <StyledIcon src="/images/icons/trash.svg" onClick={() => { deleteMaterial(material.id) }} />
                    </a> : null} 
                <a href={`/${user.data.typeName}/Subjects/Materials/Details?id=${material.id}`} download>
                    <StyledIcon src="/images/icons/download.svg" />
                </a>
            </StyledIconContainer>
        </StyledContainer>
        
    )
}

export default MaterialView