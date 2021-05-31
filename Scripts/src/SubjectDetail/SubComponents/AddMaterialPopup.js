import React, { useState, useEffect } from 'react'
import MaterialInputList from './MaterialInputList'

const AddMaterialPopup = ({ upload, hidePopup }) => {
    const [headingText, updateHeadingText] = useState('Přidat materiály')
    const [inputGroupName, updateInputName] = useState('')
    const [showGroupNameInput, updateShowGroupNameInput] = useState(false)
    const [showWarning, updateShowWarning] = useState(false)
    const [warningMessage, updateWarning] = useState('')
    const [inputData, updateInputData] = useState([])
    
    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    useEffect(() => {
        
        if (inputData.length > 1) {
           updateShowGroupNameInput(true) 
        } else {
            updateShowGroupNameInput(false)
        }

    }, [inputData])
    
    const trackInputData = (referenceId, data , type) => {
        const newData = [...inputData]        
        if (type === 'name') {
            Object.assign(newData[referenceId], {
                materialName: data
            })
        }
        if (type === 'description') {
            Object.assign(newData[referenceId], {
                materialDescription: data
            })
        }
        if (type === 'file') {            
            Object.assign(newData[referenceId], {
                materialFile: data
            })
        }      
        updateInputData(newData)
    } 

    const checkInputList = (data) => {
        for (let i = 0; i < data.length; i++){
            if (!data[i].materialFile || !data[i].materialName) {
                return false
            }
        }
        return true
    }

    const handleUploadClick = () => {
       /* if (inputData.length === 1) {
            if (inputData[0].materialName && inputData[0].materialFile) {
                upload(inputGroupName, inputData)
                if (showWarning) { updateShowWarning(false); updateWarning(null) }
                hidePopup()
            } else if (!inputData[0].materialName && !inputData[0].materialFile) {
                updateWarning('Prosím zadej název a vyber soubor')
                updateShowWarning(true)
            } else if (!inputData[0].materialName) {
                updateWarning('Prosím zadej název')
                updateShowWarning(true)
            } else if (!inputData[0].materialFile) {
                updateWarning('Prosím vyber soubor')
                updateShowWarning(true)
            }
        } else{
            if (checkInputList(inputData)) {
                upload(inputGroupName, inputData)
                if (showWarning) { updateShowWarning(false); updateWarning(null) }
                hidePopup()
            } else {
                updateWarning('Zadej prosím názvy a soubory ke všem materálům')
                updateShowWarning(true)
            }
        }*/

        console.log(inputData)
        
    }

    const handleFileInput = (files) => {

        //converting filelist object to a proper array
        const filesArr = [...files]
        
        const inputList = filesArr.map(file => {
            return ({
                materialName: file.name,
                materialDescription: null,
                materialFile: file
            })
        })

        const newData = inputData.concat(inputList)
        updateInputData(newData)
    }

    const changeMaterialName = ( name, index) => { 
        const newData = [...inputData]       
        Object.assign(newData[index], { materialName: name })
        updateInputData(newData)
    }
    
    const removeFile = (index) => {
        const newData = [...inputData]
        newData.splice(index, 1)
        updateInputData(newData)
    }

    return (
        <div className="add-material-container">              
            <div className="popup-inner-container">
                <div className="popup-inner-padding">
                <div className="material-popup-title-container">
                    <h4 className="popup-title">{headingText}</h4>
                        <img className="pointer" src="/images/close.svg" alt="zavřít" height="25px" onClick={() => { hidePopup()}}></img>
                    </div>
                    {inputData.length > 0 ? <MaterialInputList materials={inputData} removeFile={removeFile} changeName={changeMaterialName}/> : null}
                <div className="add-group-container">
                        {showGroupNameInput ?
                            <div>
                            <input className="form-control mb10 mt10" placeholder="Jméno skupiny materiálu, např. příprava na test" />
                                <div className="hline"></div>
                            </div>
                            : null}
                        <input className="multiple-file-input" type="file" name="materialy" id="materialy" multiple onChange={(event) => { handleFileInput(event.target.files)}}/>
                        <label className="btn btn-outline-primary w100" for="materialy">Vybrat soubory</label>    
                </div>                       
                {showWarning? 
                        <p className="add-warning-text">{warningMessage}</p>
                   : null}              
                <button className="btn btn-primary next-material-button" onClick={handleUploadClick}>Přidat</button>                    
                </div>               
            </div>           
        </div>
    )
}

export default AddMaterialPopup