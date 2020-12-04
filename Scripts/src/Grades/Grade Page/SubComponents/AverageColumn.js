import React from 'react';
import AverageGrade from './AverageGrade.js';
import ColumnHeader from './ColumnHeader.js';

const AverageColumn = ({ students, grades }) => {
    const gradeList = students.map((student, index) => {
        let total = 0;
        let gradeNum = 0;
        let studentGrades = []
        grades.forEach(grade => {
            if (grade.studentId === student.id) {
                total = total + grade.value
                gradeNum = gradeNum + 1
                studentGrades.push(grade.value)
            }
        })
        return <AverageGrade key={index} grade={ total/gradeNum }/>
    })


    return (
        <div className="grade-table-column">
            <ColumnHeader title={'Průměr'}/>
            {gradeList}
        </div>
    );

};

export default AverageColumn