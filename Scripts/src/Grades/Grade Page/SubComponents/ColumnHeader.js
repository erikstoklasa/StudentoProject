import React, {useState} from 'react'
import '../GradePage.css';

const ColumnHeader = ({ title, type, handleClick, displayInput, onInputChange, onClickHeader }) => {
    const [displayArrow, updateDisplayArrow] = useState(false)

    const handleAverageHeaderClick = () => {
        updateDisplayArrow(!displayArrow)
    }


    if (type === 'New Grade') {
        return (
            <div className="column-header">
            
                {(!displayInput ? <div className=" column-header-container"><div className=" btn btn-primary btn-display" onClick={handleClick}><img src="/images/add.svg" alt="Přidat" height="18px" className="plus"></img><p className="column-header-text">{title}</p></div></div>
                    : <div className="column-header-container"><input className="form-control column-header-input" onChange={onInputChange} /></div>)}
            
            </div>
        )
    }




    if (type === 'average' && !displayArrow) { 
        return (
            <div className="column-header" onClick={() => {
                onClickHeader()
                handleAverageHeaderClick()
            }}><img src="/images/uparrow.svg" height="18px" className="plus"></img><p className="column-header-text">{title}</p></div>
        ) 
    }

    if (type === 'average' && displayArrow) { 
        return (
            <div className="column-header" onClick={() => {
                onClickHeader()
                handleAverageHeaderClick()
            }}><img src="/images/downarrow.svg" height="18px" className="plus"></img><p className="column-header-text">{title}</p></div>
        ) 
    }


    else {
        return (
            <div className="column-header"><p className="column-header-text">{title}</p></div>
        )
    }
}

export default ColumnHeader

