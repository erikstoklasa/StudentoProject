import React, {useState, useEffect} from 'react'
import MaterialContainer from './MaterialContainer'
import AddMaterialPopup from './AddMaterialPopup'
import './StudentMaterial.css'
import moment from 'moment';

const StudentMaterial = () => {
    const subjectId = window.location.href.split("Details?id=").pop();
    const [address, updateAddress] = useState()
    const [showMaterialPopup, updateShowMaterialPopup] = useState(false);
    const [userInfo, updateUserInfo] = useState()
    const [material, updateMaterials] = useState();
    

    const getApiAddress = () => {       
        const getUserType = (data) => {           
            if (data.userType === 'student')    return 'Student'           
            else if(data.userType === 'teacher') return 'Teacher'
        }
        fetch(`${window.location.origin}/api/Auth/GetUserInfo`)
        .then(res => res.json())
            .then(data => {
                const type = getUserType(data)
                Object.assign(data, {typeName: type})
                updateUserInfo(data)
                const apiString = `${window.location.origin}/api/SubjectMaterials/${type}`                
                updateAddress(apiString)
        })
    }

    const fetchMaterials = () => {      
        if (address) {
            fetch(`${address}/Material?subjectInstanceId=${subjectId}`)
                .then(res => res.json())
                .then(data => {
                    data.forEach(material => {
                        const linkText = material.fileExt.substring(1);
                        Object.assign(material, {
                            addedRelative: moment.utc(material.added).locale('cs').fromNow(),
                            addedDisplay: moment.utc(material.added).format("D.M.Y"),
                            link: linkText
                        })
                    })
                    if (data.length > 0) {
                        updateMaterials(data)
                    } else {
                        updateMaterials(null)
                    }
                })
        }
    }   

    useEffect(getApiAddress, [])
    useEffect(fetchMaterials, [address])
    
    const uploadMaterials = (groupName, materials) => {
        const materialList = [...materials]
        if (groupName) {
            fetch(`${address}/MaterialGroup?name=${groupName}`, {
                method: 'POST',
            })
                .then(res => res.json())
                .then(
                    data => {
                        materialList.forEach(material => {
                            Object.assign(material, { materialGroupId: data.id })
                        })
                        materialList.forEach(material => {
                            const formData = new FormData();
                            formData.append('FormFile', material.materialFile, material.materialFile.name)
                            formData.append('Material.Name', material.materialName)
                            formData.append('Material.Description', material.materialDescription)
                            formData.append('Material.SubjectInstanceId', subjectId)
                            formData.append('Material.SubjectMaterialGroupId', material.materialGroupId)
                            fetch(`${address}/Material`, {
                                method: 'POST',
                                body: formData
                            }).then(res => res.json())
                                .then(data => { fetchMaterials() })
                        })
                    })
        } else {
            materialList.forEach(material => {
                
                const formData = new FormData();
                formData.append('FormFile', material.materialFile, material.materialFile.name)
                formData.append('Material.Name', material.materialName)
                formData.append('Material.Description', material.materialDescription)
                formData.append('Material.SubjectInstanceId', subjectId)
                
                fetch(`${address}/Material`, {
                    method: 'POST',
                    body: formData
                }).then(res => res.json())
                    .then(data => { fetchMaterials() })
            })
        }
    }

    const deleteMaterial = (id) => {
        fetch(`${address}/Material?subjectMaterialId=${id}`, {
            method: 'DELETE'
        }).then(res => {
            if (res.ok) {
                fetchMaterials()
            }
        }) 
        
    }    

    const hideMaterialPopup = () => {
        updateShowMaterialPopup(false)
    }

    const displayMaterialPopup = () => {
        updateShowMaterialPopup(true)
    }

    return (
       
        <div class="material-container">
            <div className="material-container-heading-container">
                <p class="material-table-heading">Studijní materiály</p>
                <a class="btn btn-primary" onClick={displayMaterialPopup}><img src="/images/add.svg" alt="Přidat" height="18px" class="btn-icon" />Přidat studijní materiál</a>
            </div>
            {material ? <MaterialContainer materials={material} info={userInfo} deleteMaterial={deleteMaterial} /> : <p class="alert alert-dark my-1 w-100">Zatím jsi nepřidal/a žádné studijní materiály 🙁</p>}
            {showMaterialPopup ? <AddMaterialPopup upload={uploadMaterials} hidePopup={hideMaterialPopup} /> : null}
        </div>
        
    )

}

export default StudentMaterial