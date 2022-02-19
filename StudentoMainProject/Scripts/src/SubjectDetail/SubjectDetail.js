import React, { useState, useEffect } from 'react';
import SubjectTitle from './SubComponents/SubjectTitle'
import StudentGrades from './SubComponents/StudentGrades'
import StudentMaterial from '../../Components/StudentMaterial/StudentMaterial'
import AddGradePopup from './SubComponents/AddGradePopup'
import LoadingScreen from '../../Components/Loading/LoadingScreen';
import { fetchGrades, postGrade, deleteGrades } from '../../Services/GradeService.js';
import { fetchSubjectInstance } from '../../Services/SubjectService.js';
import styled from 'styled-components';

const SubjectDetailContainer = styled.div` 
    display: flex;        
    justify-content: space-between;
    width: 100%;
    gap: 20px;
    @media(max-width: 1120px){   
        flex-wrap: wrap;       
    }
`

const SubjectDetail = () => {
    //initialize state
    const subjectId = window.location.href.split("Details?id=").pop()
    const [subjectInfo, updateSubjectInfo] = useState();    
    const [grades, updateGrades] = useState();
    const [showAddPopup, updateShowAddPopup] = useState(false)    

    // fetch grades, subject info and student material(in the future)
    const fetchData = () => {           
        const getData = async () => {
            const resGrades = await fetchGrades('Student', subjectId)
            const resSubject = await fetchSubjectInstance('Student', subjectId)            
            
            if (resSubject.success) {
                updateSubjectInfo({
                    loaded: true,
                    data: resSubject.data
                })
            } else {
                updateSubjectInfo({
                    loaded: false                    
                })
            }
            if (resGrades.success) {
                updateGrades({
                    loaded: true,
                    data: resGrades.data
                })
            } else {
                updateGrades({
                    loaded: false                   
                })
            }
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
        const res = await postGrade('Student', subjectId, name, value, weight, date)        
        if (res.success) {
            const newGrades = [res.data, ...grades.data]            
            updateGrades({
                loaded: grades.loaded,
                data: newGrades
            })
        }        
    }

    const deleteStudentGrade = async (id) => {
        const idArray = [id];
        const res = await deleteGrades('Student', grades.data, idArray)
        if (res.success) {
            let newArr = [...grades.data]
            idArray.forEach(id => {
                newArr = newArr.filter(grade => grade.id != id)            
            })
            updateGrades({
                loaded: grades.loaded,
                data: newArr
            })
        }        
    }   

    if (grades && subjectInfo) {
        return (
            <>
                <SubjectTitle info={subjectInfo} grades={grades} />
                <SubjectDetailContainer>
                    <StudentGrades grades={grades} showPopup={showPopup} deleteGrade={deleteStudentGrade} />
                    <StudentMaterial authors={{
                        students: [...subjectInfo.data.students],
                        teacher: subjectInfo.data.teacher
                    }} />
                    {showAddPopup ? <AddGradePopup addGrade={addStudentGrade} hidePopup={hidePopup} /> : null}
                </SubjectDetailContainer>
            </>
        )
    }else {
        return <LoadingScreen/>
    }    
}

export default SubjectDetail


        
            
                