import React, { useState, useEffect } from 'react'
import { Input, StyledWarningText, PrimaryButton } from '../../Styles/GlobalStyles'
import MaterialInputList from './MaterialInputList'
import styled from 'styled-components'

const StyledContainer = styled.div` 
    min-width: 250px;
    position: fixed;
    width: 100%;
    height: 100%;
    right: 0;
    top: 0;
    left: 0;
    bottom: 0;
    margin: auto;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 5;
`
const StyledPopup = styled.div` 
    max-height: 95vh;
    width: 100%;
    max-width: 400px;
    margin: 20px;
    padding: 15px;   
    background-color: white;
    border: none;
    border-radius: 10px;
    overflow-y: auto !important; 
`
const StyledHeadingContainer = styled.div` 
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
`
const StyledTitle = styled.h4` 
      margin: 0;
`
const StyledCloseIcon = styled.img` 
    cursor: pointer;
    height: 30px;
`
const StyledFileInput = styled.input` 
    width: 0.1px;
	height: 0.1px;
	opacity: 0;
	overflow: hidden;
	position: absolute;
	z-index: -1;
`
const StyledFileInputLabel = styled.label` 
    background: #FFFFFF;
    box-shadow: 1px 1px 8px rgba(0, 0, 0, 0.1);
    border: transparent;
    outline: none;
    border-radius: 10px;
    cursor: pointer;
    padding: 10px 10px 40px 10px;
    min-height: 158.4px;
    margin: 15px 0px;
    width: 100%;
`
const StyledLabelText = styled.p` 
    text-align: center;
    margin-top: 50px;
    margin-bottom: 20px;
    font-size: 0.8rem;
` 
const StyledFileNameInput = styled(Input)` 
    margin: 10px 0px;
`
const StyledButtonContainer = styled.div` 
    display: flex;
    justify-content: flex-end;
`

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
        <StyledContainer onClick={hidePopup}>              
            <StyledPopup onClick={e=> {e.stopPropagation()}}>                
                <StyledHeadingContainer>
                    <StyledTitle>Přidat soubory</StyledTitle>
                    <StyledCloseIcon src="/images/icons/delete.svg" alt="zavřít" onClick={hidePopup}/>
                </StyledHeadingContainer>                           
                <div>                        
                    <StyledFileInput type="file" name="materialy" id="materialy" multiple onChange={(event) => { handleFileInput(event.target.files)}}/>                    
                    <StyledFileInputLabel htmlFor="materialy">
                    {showHelpText ? <StyledLabelText>Přetáhni sem soubory, které chceš sdílet se svými spolužáky</StyledLabelText> : null}
                    {showGroupNameInput ? <StyledFileNameInput placeholder="Jméno skupiny materiálu, např. příprava na test" onChange={(event)=>{updateInputName(event.target.value)}}/> : null}
                    {inputData.length > 0 ? <MaterialInputList materials={inputData} removeFile={removeFile} changeName={changeMaterialName}/> : null}
                    </StyledFileInputLabel>
                </div>                       
                {showWarning? 
                        <StyledWarningText>{warningMessage}</StyledWarningText>
                    : null}
                <StyledButtonContainer>
                    <PrimaryButton onClick={handleUploadClick}>Přidat</PrimaryButton>                    
                </StyledButtonContainer>            
            </StyledPopup>           
        </StyledContainer>
    )
}

export default AddMaterialPopup