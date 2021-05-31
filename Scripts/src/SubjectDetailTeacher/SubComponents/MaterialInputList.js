import React from 'react'
import MaterialInput from './MaterialInput'

const MaterialInputList = ({ materials, removeFile, changeName }) => {
    
    const materialList = materials.map((material, i) => {
        return (
            <MaterialInput material={material} removeFile={removeFile} changeName={changeName} index={i}/>
        )
    })

    return (
        <div className="mt10">
            {materialList}
        </div>
    )

}

export default MaterialInputList