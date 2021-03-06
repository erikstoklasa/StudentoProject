import React, {useState, useEffect} from 'react';
  
function SubjectDetail() {
    const [subjectId, updateSubjectId] = useState()

    const determineSubjectID = () => { 
        const location = window.location.href
        const subjectId = location.split("Details?id=").pop()
        updateSubjectId(subjectId)
    }

    const fetchGrades = () => {
        if (subjectId) {
            fetch(`https://localhost:5001/api/Grades/Student?subjectInstanceId=${subjectId}`)
                .then(res => res.json())
                .then(data => console.log(data))
        }
    }

    useEffect(determineSubjectID, [])
    useEffect(fetchGrades, subjectId)


    return (
        <div>
        <h1>hello</h1>
        </div>
    );
}

export default SubjectDetail;
