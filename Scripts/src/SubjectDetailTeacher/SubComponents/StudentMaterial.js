import React from 'react'

const StudentMaterial = ({ material, info }) => {

    //display student material(not ready)
    return (
       
        <div class="student-material-container">
            <div className="student-heading-container">
                <p class="table-heading">Studijní materiály</p>
                <a class="btn btn-primary" href={`/api?SubjectInstanceId=1&amp;page=%2FTeacher%2FSubjects%2FMaterials%2FCreate`}><img src="/images/add.svg" alt="Přidat" height="18px" class="btn-icon" />Přidat studijní materiál</a>
            </div>
            <p class="alert alert-dark my-1 w-100">Zatím jste nepřidal/a žádné studijní materiály 🙁</p>
        </div>
        
    )

}

export default StudentMaterial