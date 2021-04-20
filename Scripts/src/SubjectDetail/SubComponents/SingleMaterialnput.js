import React from 'react'

const SingleMaterialInput = ({ referenceId, type, trackData }) => {

    if (type === 'group') {
        return (
            <div className="group-material-input">
                <div className="border-container"></div>
                <input className="form-control mb10" placeholder="Jméno materiálu" onChange={(event) => { trackData(referenceId, event.target.value, 'name') }}></input>
                <input className="form-control mb10" placeholder="Popis" onChange={(event) => { trackData(referenceId, event.target.value, 'description') }}></input>
                <input className="mb10" type="file" id="FileUpload" name="FileUpload" onChange={(event) => { trackData(referenceId, event.target.files[0], 'file') }}></input>                
            </div>
        )
    }
    else {
        return (
            <div className="single-material-input">
                <input className="form-control mb10" placeholder="Jméno materiálu" onChange={(event) => { trackData(referenceId, event.target.value, 'name') }}></input>
                <input className="form-control mb10" placeholder="Popis"  onChange={(event) => { trackData(referenceId, event.target.value, 'description') }}></input>
                <input type="file" id="FileUpload" name="FileUpload" onChange={(event) => { trackData(referenceId, event.target.files[0], 'file') }}></input>
            </div>
        )
    }
}

export default SingleMaterialInput