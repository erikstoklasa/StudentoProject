import React from 'react'
import '../GradePage.css';

const ColumnHeader = ({ title, type, handleClick, displayInput, onInputChange, onClickHeader }) => {

    if (type === 'New Grade') {
        return (
            <div className="column-header">
            
                {(!displayInput ? <div className="column-header-container"><div className="btn btn-primary" onClick={handleClick}><p className="column-header-text">{title}</p></div></div>
                    : <div className="column-header-container"><input className="form-control column-header-input" onChange={onInputChange} /></div>)}
            
            </div>
        )
    }

    if (type === 'average') { 
        return (
            <div className="column-header" onClick={onClickHeader}><p className="column-header-text">{title}</p></div>
        ) 
    }

    else {
        return (
            <div className="column-header"><p className="column-header-text">{title}</p></div>
        )
    }
}

export default ColumnHeader