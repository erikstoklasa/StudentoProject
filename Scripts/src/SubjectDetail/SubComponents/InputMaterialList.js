import React from 'react'
import SingleMaterialInput from './SingleMaterialnput'

const InputMaterialList = ({ data, trackInputData, type }) => {
    const inputList = data.map((inputObject, index) => {
        return <SingleMaterialInput key={index} referenceId={index} data={inputObject} type={type} trackData={trackInputData} />
    })

    return (
        <div>
            {inputList}
        </div>
    )
}

export default InputMaterialList