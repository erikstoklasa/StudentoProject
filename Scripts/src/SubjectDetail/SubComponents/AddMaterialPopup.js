import React, {useState, useEffect} from 'react'
import InputMaterialList from './InputMaterialList'


const AddMaterialPopup = ({ upload, hidePopup }) => {
    const [currentDisplay, updateCurrentDisplay] = useState('selection')
    const [headingText, updateHeadingText] = useState('Přidat materiály')
    const [inputGroupName, updateInputName] = useState('')
    const [showWarning, updateShowWarning] = useState(false)
    const [warningMessage, updateWarning] = useState('')
    const [inputData, updateInputData] = useState(
    [
        {
            materialName: null,
            materialDescription: null,
            materialFile: null
        }
    ])
        useEffect(() => {
            document.body.style.overflow = 'hidden';
            return ()=> document.body.style.overflow = 'unset';
        }, []);
    
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

    const addNextMaterial = () => {   
        if (inputData.length <= 5) {
            if (checkSingularInput(inputData[inputData.length-1])) {
                const newInputList = [...inputData,
                {
                    materialName: null,
                    materialDescription: null,
                    materialFile: null
                }
                ]
                updateInputData(newInputList)
                if (showWarning) updateShowWarning(false);
            } else {
                updateWarning('Vyplň prosím údaje k předchozím materiálům')
                updateShowWarning(true)
            }
        } else {
            updateWarning('Najednou lze přidat pouze 5 materiálu')
            updateShowWarning(true)
        }
    }

    const checkInputList = (data) => {
        for (let i = 0; i < data.length; i++){
            if (!data[i].materialFile || !data[i].materialName) {
                return false
            }
        }
        return true
    }

    const checkSingularInput = (element) => {
        if (element.materialName && element.materialFile) {
            return true
        } else {
            return false
        }
    }

    const handleUploadClick = () => {
        if (inputData.length === 1) {
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
        }
        
    }

    return (
        <div className="add-material-container">              
            <div className="popup-inner-container">
                <div className="popup-inner-padding">
                <div className="material-popup-title-container">
                    <h4 className="popup-title">{headingText}</h4>
                        <img className="pointer" src="/images/close.svg" alt="zavřít" height="25px" onClick={() => { hidePopup()}}></img>
                </div> 
                {currentDisplay === 'selection' ?
                    <div className="add-selection-container">
                        <div className="btn btn-primary add-selection-button mr5" onClick={() => { updateCurrentDisplay('material-group'); updateHeadingText('Nová skupina')}}>Skupina materiálu</div>
                            <div className="btn btn-outline-primary add-selection-button ml5" onClick={() => { updateCurrentDisplay('material-single')}}>Samotný materiál</div>                           
                    </div>
                : null}
                {currentDisplay === 'material-group' ?
                    <div className="add-group-container">
                            <input className="material-group-name-input form-control" placeholder="Jméno skupiny" onChange={(event) => { updateInputName(event.target.value)}}></input>
                            <InputMaterialList data={inputData} trackInputData={trackInputData} type={'group'}/>
                        <button className="btn btn-outline-primary next-material-button" onClick={addNextMaterial}>Další materiál</button>
                    </div>
                    : null}
                {currentDisplay === 'material-single' ?
                    <div className="add-group-container">               
                        <InputMaterialList data={inputData} trackInputData={trackInputData} type={'single'}/>                
                    </div>
                        : null}
                 { showWarning? 
                        <p className="add-warning-text">{warningMessage}</p>
                   : null}
                {currentDisplay !== 'selection' ?
                    <button className="btn btn-primary next-material-button" onClick={handleUploadClick}>Přidat</button>
                    : null}
                </div>               
            </div>           
        </div>
    )
}

export default AddMaterialPopup