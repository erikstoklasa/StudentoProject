import React, {useState, useEffect} from 'react'
import MaterialContainer from './MaterialContainer'
import AddMaterialPopup from './AddMaterialPopup'
import './StudentMaterial.css'
import moment from 'moment';

const StudentMaterial = () => {
    const subjectId = window.location.href.split("Details?id=").pop();
    const [adress, updateAdress] = useState()
    const [showMaterialPopup, updateShowMaterialPopup] = useState(false);
    const [studentInfo, updateStudentInfo] = useState()
    const [material, updateMaterials] = useState();
    

    const getApiAdress = () => {       
        const getUserType = (data) => {           
            if (data.userType === 'student')    return 'Student'           
            else return 'Teacher'
        }
        fetch(`${window.location.origin}/api/Auth/GetUserInfo`)
        .then(res => res.json())
            .then(data => {
                updateStudentInfo(data)

                const apiString = `${window.location.origin}/api/SubjectMaterials/${getUserType(data)}`                
                updateAdress(apiString)
        })
    }

    const fetchMaterials = () => {      
        if (adress) {
            fetch(`${adress}/Material?subjectInstanceId=${subjectId}`)
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

    useEffect(getApiAdress, [])
    useEffect(fetchMaterials, [adress])
    
    const uploadMaterials = (groupName, materials) => {
        const materialList = [...materials]
        if (groupName) {
            fetch(`${adress}/MaterialGroup?name=${groupName}`, {
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
                            fetch(`${adress}/Material`, {
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
                
                fetch(`${adress}/Material`, {
                    method: 'POST',
                    body: formData
                }).then(res => res.json())
                    .then(data => { fetchMaterials() })
            })
        }
    }

    const deleteMaterial = (id) => {
        fetch(`${adress}/Material?subjectMaterialId=${id}`, {
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
                <p class="table-heading">Studijní materiály</p>
                <a class="btn btn-primary" onClick={displayMaterialPopup}><img src="/images/add.svg" alt="Přidat" height="18px" class="btn-icon" />Přidat studijní materiál</a>
            </div>
            {material ? <MaterialContainer materials={material} info={studentInfo} deleteMaterial={deleteMaterial} /> : <p class="alert alert-dark my-1 w-100">Zatím jsi nepřidal/a žádné studijní materiály 🙁</p>}
            {showMaterialPopup ? <AddMaterialPopup upload={uploadMaterials} hidePopup={hideMaterialPopup} /> : null}
        </div>
        
    )

}

export default StudentMaterial