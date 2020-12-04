import React from 'react'
import '../GradePage.css';

const ColumnHeader = ({ title, type, handleClick }) => {
    if (type === 'New Grade') {
        return (
            <div className="column-header" onClick={handleClick}>{title}</div>
        )
     }
    else {
        return (
            <div className="column-header">{title}</div>
        )
    }
}

export default ColumnHeader