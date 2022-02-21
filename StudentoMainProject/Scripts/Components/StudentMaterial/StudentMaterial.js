import React, {useState, useEffect, createContext} from 'react'
import MaterialContainer from './MaterialContainer'
import AddMaterialPopup from './AddMaterialPopup'
import { fetchMaterials, postMaterialGroup, postMaterial, deleteMaterial } from '../../Services/MaterialService'
import { fetchUserInfo } from '../../Services/AuthService'
import LoadingScreen from '../Loading/LoadingScreen'
import { PrimaryButton } from '../../Styles/GlobalStyles'
import styled from 'styled-components'
import moment from 'moment';

export const MaterialContext = createContext()

const StyledContainer = styled.div` 
    flex-basis: 500px;
    flex-grow: 1;  
`
const StyledHeadingContainer = styled.div` 
    margin: 10px 0px 10px 0px;
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    align-items: center; 
`
const StyledHeading = styled.p` 
    margin: 0;   
    white-space: nowrap;    
    color: var(--grey);
    text-align: start; 
    margin-top: 10px;
    margin-bottom: 10px;   
`

const StudentMaterial = ({ authors }) => {
    
    const getSubjectId = () => {
        const queryString = window.location.search
        const urlParams = new URLSearchParams(queryString);
        return urlParams.get('id').split('/')[0]
    }    

    const subjectId = getSubjectId()   
    const [showMaterialPopup, updateShowMaterialPopup] = useState(false);
    const [userInfo, updateUserInfo] = useState()
    const [materialData, updateMaterials] = useState();        

    const getApiAddress = () => {       
        const getUserType = (data) => {           
            if (data.userType === 'student')    return 'Student'           
            else if (data.userType === 'teacher') return 'Teacher'
            else if(data.userType === 'teacher classmaster') return 'Teacher'
        }

        const getTypeNum = (data) => {            
            if (data.userType === 'student') return 1          
            else return 0
        }

        const getUserData = async () => {
            const res = await fetchUserInfo()
            if (res.success) {
                const type = getUserType(res.data)
                const typeNum = getTypeNum(res.data)
                const newData = Object.assign(res.data, { typeName: type, typeNum: typeNum })                
                const newUserInfo = {
                    loaded: true,
                    data: newData                    
                }
                updateUserInfo(newUserInfo)                
            }
        }
        getUserData()        
    }   

    const fetchData = () => {        
        if (userInfo?.loaded) {           
            const getMaterials = async () => {                
                const res = await fetchMaterials(userInfo.data.typeName, subjectId)
                
                if (res.success) {                    
                    updateMaterials({
                        loaded: true,
                        data: res.data
                    })
                } else {
                    updateMaterials({
                        loaded: false,                        
                    })
                }
            }
            getMaterials()          
        }
    }   

    useEffect(getApiAddress, [])
    useEffect(fetchData, [userInfo])

    const uploadMaterial = async (materialProp) => {        
        const res = await postMaterial(subjectId, userInfo.data.typeName, materialProp)
        if (res.success) {            
            //materialProp.materialFile.name
            const newMaterial = {
                id: res.data.id,
                name: materialProp.materialName,                
                addedBy: userInfo.data.typeNum,
                addedById: userInfo.data.userId,
                description: '',
                link: materialProp.materialFile.type.split('/')[1],
                fileType: materialProp.materialFile.type,
                fileExt: `.${materialProp.materialFile.type.split('/')[1]}`,
                subjectInstanceId: subjectId,
                added: moment().utc(),
                addedDisplay: moment.utc(moment.utc()).format("D.M.Y"),
                addedRelative: moment.utc(moment.utc()).locale('cs').fromNow()         
                
            }         
            
            return newMaterial
        }
    }
    
    const uploadMaterials = async (groupName, materials) => {        
        if (groupName) {            
            const res = await postMaterialGroup(userInfo.data.typeName, groupName)
            if (res.success) {
                materials.forEach(material => {
                    Object.assign(material, { materialGroupId: res.data.id })                    
                })
                const newMatArr = []
                for (let material of materials) {
                    const newMat = await uploadMaterial(material)
                    newMatArr.push(newMat)

                }
                const newMaterialData = {
                    loaded: true,
                    data: [...newMatArr, ...materialData.data]
                }
                updateMaterials(newMaterialData)
            }
                
        } else {           
            const newMat = await uploadMaterial(materials[0])
            const newMaterialData = {
                loaded: true,
                data: [newMat, ...materialData.data]
            }
            updateMaterials(newMaterialData)
        }          
    }

    const deleteMaterials = async (id) => {
        const res = await deleteMaterial(userInfo.data.typeName,id)
        if (res.success) {            
            const newMatArr = materialData.data.filter(material =>  material.id !== id )            
            updateMaterials(
                {
                    loaded: true,
                    data: newMatArr
                }
            )
        }        
    }    

    const hideMaterialPopup = () => {
        updateShowMaterialPopup(false)
    }

    const displayMaterialPopup = () => {
        updateShowMaterialPopup(true)
    }

    const isLoaded = () => {
        if (materialData && userInfo) {
            if (userInfo.data.typeName === 'Teacher') {
                return true
            } else if (userInfo.data.typeName === 'Student' && authors) {
                return true
            } else {
                return false
            }
        }
        else return false
    }    

    return (
        <MaterialContext.Provider value={authors}>
            <StyledContainer>
                {isLoaded() ?
                <>
                <StyledHeadingContainer>
                <StyledHeading>Studijní materiály</StyledHeading>
                {materialData.loaded? <PrimaryButton onClick={displayMaterialPopup}>Přidat studijní materiál</PrimaryButton> : null}
                </StyledHeadingContainer>                        
                <MaterialContainer materials={materialData} info={userInfo} deleteMaterial={deleteMaterials} /> 
                {showMaterialPopup ? <AddMaterialPopup upload={uploadMaterials} hidePopup={hideMaterialPopup} /> : null}
                </>
                : <LoadingScreen relative={true} />}            
            </StyledContainer>
        </MaterialContext.Provider>        
    )
}

export default StudentMaterial