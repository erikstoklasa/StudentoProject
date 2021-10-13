import React, { useState, useEffect } from 'react'
import MaterialInputList from './MaterialInputList'

const AddMaterialPopup = ({ upload, hidePopup }) => {    
    const [inputGroupName, updateInputName] = useState('')
    const [showGroupNameInput, updateShowGroupNameInput] = useState(false)
    const [showWarning, updateShowWarning] = useState(false)
    const [warningMessage, updateWarning] = useState('')
    const [inputData, updateInputData] = useState([])
    const [showHelpText, updateShowHelp] = useState(true)
    
    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return ()=> document.body.style.overflow = 'unset';
    }, []);

    useEffect(() => {
        if (inputData.length > 0) {
            updateShowHelp(false)
        } else if (!showHelpText) {
                updateShowHelp(true)
      }
    }
    ,[inputData]);

    useEffect(() => {
        
        if (inputData.length > 1) {
           updateShowGroupNameInput(true) 
        } else {
            updateShowGroupNameInput(false)
        }

    }, [inputData]) 


    const checkInputList = (data) => {
        for (let i = 0; i < data.length; i++){
            if (!data[i].materialFile || !data[i].materialName) {
                return false
            }
        }
        return true
    }

    const handleUploadClick = () => {
     if (inputData.length === 1) {
            if (inputData[0].materialName && inputData[0].materialFile) {
                upload(inputGroupName, inputData)
                if (showWarning) { updateShowWarning(false); updateWarning(null) }
                hidePopup()
            } else if (!inputData[0].materialName) {
                updateWarning('Prosím zadej název souboru')
                updateShowWarning(true)
            } 
        } else if(inputData.length > 1){
         if (checkInputList(inputData)) {
             if (inputGroupName) {
                 upload(inputGroupName, inputData)
                 if (showWarning) { updateShowWarning(false); updateWarning(null) }
                    hidePopup()
             } else {
                 updateShowWarning(true)
                 updateWarning('Prosím zadej jméno skupiny materiálu')
             }
                
            } else {
                updateWarning('Zadej prosím názvy ke všem novým materiálům')
                updateShowWarning(true)
            }
     } else {
        updateWarning('Prosím vyber alespoň jeden soubor')
        updateShowWarning(true)
    }
        
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
        if (showWarning && warningMessage === 'Prosím vyber alespoň jeden soubor') {
            updateShowWarning(false)
            updateWarning(null)
        }
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
        if (newData.length === 0) {
            updateShowWarning(false)
            updateWarning(null)
        }
    }

    return (
        <div className="add-material-container" onClick={hidePopup}>              
            <div className="material-popup-inner-container" onClick={e=> {e.stopPropagation()}}>                
                <div className="material-popup-title-container">
                    <h4 className="popup-title">Přidat soubory</h4>
                        <img className="pointer" src="/images/icons/delete.svg" alt="zavřít" height="30px" onClick={hidePopup}></img>
                </div>               
                            
                <div className="add-group-container">
                        
                        <input className="multiple-file-input" type="file" name="materialy" id="materialy" multiple onChange={(event) => { handleFileInput(event.target.files)}}/>
                    
                    <label className="file-input-label w100" for="materialy">
                    {showHelpText ? <p className="material-popup-help-text">Přetáhni sem soubory, které chceš sdílet se svými spolužáky</p> : null}
                    {showGroupNameInput ?
                            <div>
                            <input className="form-control mb10 mt10" placeholder="Jméno skupiny materiálu, např. příprava na test" onChange={(event)=>{updateInputName(event.target.value)}}/>
                                <div className="hline"></div>
                            </div>
                            : null}
                    {inputData.length > 0 ? <MaterialInputList materials={inputData} removeFile={removeFile} changeName={changeMaterialName}/> : null}
                    </label>
                </div>                       
                {showWarning? 
                        <p className="add-warning-text">{warningMessage}</p>
                   : null}  
                <button className="btn btn-primary next-material-button" onClick={handleUploadClick}>Přidat</button>                    
                             
            </div>           
        </div>
    )
}

export default AddMaterialPopup