import React from 'react';
import AverageGrade from './AverageGrade.js';
import ColumnHeader from './ColumnHeader.js';

const AverageColumn = ({ students, grades, onClickHeader }) => {   
    const gradeList = students.map((student, index) => {
        let total = 0;
        let gradeNum = 0;
                
        grades.forEach(grade => {
            if (grade.studentId === student.id) {
                total = total + grade.value
                gradeNum = gradeNum + 1                
            }
        })

        let formatedAvearage = (total / gradeNum).toFixed(2)
        
        if (!isNaN(formatedAvearage)) {
            return <AverageGrade key={index} grade={formatedAvearage} />
        }
        else {
            return <AverageGrade key={index} grade={''} />
        }
        
    })


    return (
        <div className="grade-table-column">
            <ColumnHeader title={'Průměr'} type={'average'} onClickHeader={onClickHeader} />
            <div className = "grade-shadow-bottom">
                {gradeList}
            
            </div>
        </div>
    );

};

export default AverageColumn