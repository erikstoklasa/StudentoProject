import { materialsApiAdress } from "./Variables";
import moment from "moment";

export const fetchMaterials = async (userType, subjectId) => {
    const res = await fetch(`${materialsApiAdress}/${userType}/Material?subjectInstanceId=${subjectId}`)
    if (res.ok) {
        const data = await res.json()
        data.forEach(material => {
            const linkText = material.fileExt.substring(1);
            Object.assign(material, {
                addedRelative: moment.utc(material.added).locale('cs').fromNow(),
                addedDisplay: moment.utc(material.added).format("D.M.Y"),
                link: linkText
            })
        })
        const reqResponse = {
            success: true,
            data: data
        }
       
        return reqResponse
    } else {
        const reqResponse = {
            success: false            
        }
        
        return reqResponse 
    }
}

export const postMaterialGroup = async (userType, groupName) => {
    const res = await fetch(`${materialsApiAdress}/${userType}/MaterialGroup?name=${groupName}`, { method: 'POST' })
    
    if (res.ok) {
        
        const data = await res.json()
        const reqResponse = {
            success: true,
            data : data
        }
        return reqResponse
    } else {
        const reqResponse = {
            success: false           
        }
        return reqResponse
    }
}

export const postMaterial = async (subjectId, userType, material) => {
    
    const materialData = new FormData();
    materialData.append('FormFile', material.materialFile, material.materialFile.name)
    materialData.append('Material.Name', material.materialName)
    materialData.append('Material.Description', material.materialDescription)
    materialData.append('Material.SubjectInstanceId', subjectId)
    if(material.materialGroupId) materialData.append('Material.SubjectMaterialGroupId', material.materialGroupId)
    const res = await fetch(`${materialsApiAdress}/${userType}/Material`, { method: 'POST', body: materialData})
    if (res.ok) {
        const data = await res.json()
        const reqResponse = {
            success: true,
            data: data,            
        }
        return reqResponse
    } else {
        const reqResponse = {
            success: false            
        }
        return reqResponse
    }
}

export const deleteMaterial = async (userType, materialId) => {
    const res = await fetch(`${materialsApiAdress}/${userType}/Material?subjectMaterialId=${materialId}`, { method: 'DELETE' })
    return ({success: res.ok})
}