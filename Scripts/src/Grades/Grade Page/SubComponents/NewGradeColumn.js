import React, {useState} from 'react'
import ColumnHeader from './ColumnHeader';
import NewGrade from './NewGrade';
import '../GradePage.css'

const NewGradeColumn = ({ students, subjectInstanceId }) => {
    const [newGradeName, updateNewGradeName] = useState('Přidat známku')
    const [showInput, updateShowInput] = useState(false)
    const [newGrades, updateNewGrades] = useState([])

    const handleHeaderClick = () => {
        updateNewGrades([])
        updateShowInput(!showInput);
    }

    const trackNewGradeValues = (grade, id) => {
        const gradesArray = newGrades;
        const newGrade = {
            value: grade,
            subjectInstanceId: subjectInstanceId,
            studentId: id,

        }
        if (!newGrades.some(e => e.studentId === newGrade.studentId )) {
            gradesArray.push(newGrade)
        }
        else {
            newGrades.forEach(grade => {
                if (grade.studentId === newGrade.studentId) {
                    grade.value = newGrade.value;
                }
            })
        }
        updateNewGrades(gradesArray)
    }

    const removeNewGrade = (studentId) => {
        const gradesArray = newGrades.filter((grade)=> {
            return grade.studentId !== studentId;
        });
        updateNewGrades(gradesArray)
    }

    const handleSubmitNewGrades = () => {console.log(newGrades)}
    
    const inputList = students.map((student, index) => {
        return <NewGrade key={index} studentId={student.id} showInput={showInput} onGradeChange={trackNewGradeValues} onGradeRemove={removeNewGrade}/>
    })

    return (
        <div className='grade-table-column'>
            <ColumnHeader title={'Přidat známku'} type={'New Grade'} handleClick={handleHeaderClick} />
            {inputList}
            <button onClick={handleSubmitNewGrades}>Submit</button>
        </div>
    )

}

export default NewGradeColumn