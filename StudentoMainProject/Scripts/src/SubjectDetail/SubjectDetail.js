import React, { useState, useEffect } from 'react';
import apiAddress from './variables.js'
import SubjectTitle from './SubComponents/SubjectTitle'
import StudentGrades from './SubComponents/StudentGrades'
import StudentMaterial from '../../Components/StudentMaterial/StudentMaterial'
import AddGradePopup from './SubComponents/AddGradePopup'
import { fetchGrades, postGrade, deleteGrades } from '../../Services/GradeService.js';
import { fetchSubjectInstance } from '../../Services/SubjectService.js';
import './SubjectDetail.css'


  
const SubjectDetail = () => {
    //initialize state
    const subjectId = window.location.href.split("Details?id=").pop()
    const [subjectInfo, updateSubjectInfo] = useState();    
    const [grades, updateGrades] = useState([]);
    const [showAddPopup, updateShowAddPopup] = useState(false)    

    // fetch grades, subject info and student material(in the future)
    const fetchData = () => {     
           
        const getData = async () => {
            const resGrades = await fetchGrades('Student', subjectId)
            const resSubject = await fetchSubjectInstance('Student', subjectId)
            
            updateSubjectInfo(resSubject)
            updateGrades(resGrades)            
        }
        getData()        
    }
    
    //initialize effect hook chain   
    useEffect(fetchData, [])    

    //update state to display add grade popup
    const showPopup = () => {
        updateShowAddPopup(true)
    }

    //update state to hide add grade popup
    const hidePopup = () => {
        updateShowAddPopup(false)
    }

    //send a request to post student grade
    const addStudentGrade = async (name, value, weight, date) => {        
        const res = await postGrade('Student', grades.data, subjectId, name, value, weight, date)        
        if (res.success) {
            updateGrades({
                success: true,
                status: grades.status,
                data: res.data
            })
        }        
    }

    const deleteStudentGrade = async (id) => {
        const idArray = [id];
        const res = await deleteGrades('Student', grades.data, idArray)
        if (res.success) {
            updateGrades({
                success: true,
                status: grades.status,
                data: res.data
            })
        }        
    }  
    
    return (
        <div className="subject-detail-container">
            {subjectInfo ? <SubjectTitle info={subjectInfo.data} grades={grades} /> : null}
        
            {grades && subjectInfo ?
                <div className="grades-material-container">
                    <StudentGrades grades={grades} showPopup={showPopup} deleteGrade={deleteStudentGrade} />
                    <StudentMaterial/>
                    {showAddPopup ? <AddGradePopup addGrade={addStudentGrade} hidePopup={hidePopup} /> : null}
                </div>
                : null}          
        </div>
    );
}

export default SubjectDetail