import React from 'react'
import MaterialContainer from './MaterialContainer'

const StudentMaterial = ({ material, info, showPopup}) => {
    return (
       
        <div class="student-material-container">
            <div className="student-heading-container">
                <p class="table-heading">Studijní materiály</p>
                <a class="btn btn-primary" onClick={showPopup}><img src="/images/add.svg" alt="Přidat" height="18px" class="btn-icon" />Přidat studijní materiál</a>
            </div>
            {material ? <MaterialContainer materials={material} info={info} /> : <p class="alert alert-dark my-1 w-100">Zatím jsi nepřidal/a žádné studijní materiály 🙁</p>}
        </div>
        
    )

}

export default StudentMaterial