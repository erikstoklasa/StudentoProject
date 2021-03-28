import React, {useState} from 'react'
import InputMaterialList from './InputMaterialList'


const AddMaterialPopup = ({ upload, hidePopup }) => {
    const [currentDisplay, updateCurrentDisplay] = useState('selection')
    const [headingText, updateHeadingText] = useState('Přidat materiály')
    const [inputGroupName, updateInputName] = useState('')
    const [inputData, updateInputData] = useState(
    [
        {
            materialName: null,
            materialDescription: null,
            materialFile: null
        }
    ])
    
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
        const newInputList = [...inputData,
            {
            materialName: null,
            materialDescription: null,
            materialFile: null
            }
        ]
        updateInputData(newInputList)
    }

    const handleUploadClick = () => {       
        upload(inputGroupName, inputData)
        hidePopup()
    }

    return (
        <div className="add-material-container">              
            <div className="popup-inner-container">
                <div className="popup-inner-padding">
                <div className="popup-title-container">
                    <h4 className="popup-title">{headingText}</h4>
                        <img src="/images/close.svg" alt="zavřít" height="25px" onClick={() => { hidePopup()}}></img>
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
                </div>
                {currentDisplay !== 'selection' ?
                    <div className="add-grade-div" onClick={handleUploadClick}>
                        <p className="add-grade-text" >Přidat</p>
                    </div>
                : null}
            </div>           
        </div>
    )
}

export default AddMaterialPopup