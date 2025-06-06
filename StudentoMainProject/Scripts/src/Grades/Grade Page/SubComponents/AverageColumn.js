import React from 'react';
import AverageGrade from './AverageGrade.js';
import ColumnHeader from './ColumnHeader.js';

const AverageColumn = ({ students, onClickHeader }) => {   
    const gradeList = students.map((student, index) => {          
        if (student.average) {
            return <AverageGrade key={index} grade={student.average} />
        }
        else {
            return <AverageGrade key={index} grade={''} />
        }        
    })


    return (
        <div>
            <ColumnHeader title={'Průměr'} type={'average'} onClickHeader={onClickHeader} />
            <div>
                {gradeList}            
            </div>
        </div>
    );

};

export default AverageColumn