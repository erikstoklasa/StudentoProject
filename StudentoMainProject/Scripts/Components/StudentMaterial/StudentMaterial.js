import React, {useState, useEffect, createContext} from 'react'
import MaterialContainer from './MaterialContainer'
import AddMaterialPopup from './AddMaterialPopup'
import { fetchMaterials, postMaterialGroup, postMaterial, deleteMaterial } from '../../Services/MaterialService'
import { fetchUserInfo } from '../../Services/AuthService'
import LoadingScreen from '../Loading/LoadingScreen'
import './StudentMaterial.css'
import moment from 'moment';

export const MaterialContext = createContext()

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
            if (data.userType === 'student')    return 1          
            else if(data.userType === 'teacher') return 0
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
           
            const newMaterial = {
                id: res.data.id,
                name: materialProp.materialFile.name,                
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

    return (
        <MaterialContext.Provider value={authors}>
            <div class="material-container">
                {materialData && userInfo ?
                <div>
                <div className="material-container-heading-container">
                <p class="material-table-heading">Studijní materiály</p>
                {materialData.loaded? <a class="btn btn-primary" onClick={displayMaterialPopup}><img src="/images/add.svg" alt="Přidat" height="18px" class="btn-icon" />Přidat studijní materiál</a> : null}
                </div>
                        
                <MaterialContainer materials={materialData} info={userInfo} deleteMaterial={deleteMaterials} /> 
                {showMaterialPopup ? <AddMaterialPopup upload={uploadMaterials} hidePopup={hideMaterialPopup} /> : null}
                </div>
                    : <LoadingScreen relative={true} />}            
            </div>
        </MaterialContext.Provider>        
    )
}

export default StudentMaterial