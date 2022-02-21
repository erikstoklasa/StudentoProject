import React from 'react'
import styled, { css } from 'styled-components'

const Cell = styled.div` 
    border: 2px solid #00000026;
    background-color: white;
    border-radius: 10px;
    margin: 3px;
    flex: 1 1 0px;
    min-height: 85px;
    min-width: 100px;
    padding: 10px;
    display: flex;
    flex-direction: column;
    justify-content: start;
    ${props => props.vertical ? 
        css` 
            min-height: 85px;
            min-width: 0px;
        `
    : null}

`
const CellChanged = styled(Cell)` 
    border: 2px solid var(--primaryYellowStrong);
    background-color: var(--primaryYellowStrong);
`
const CellEmpty = styled(Cell)` 
    border: 2px solid #00000026;
    background-color: #00000024;
`
const SubjectHeading = styled.div` 
    font-weight: bold;
    min-width: 0;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
`
const Additional = styled.div` 
    overflow: hidden;
    text-overflow: ellipsis;  
`

const TableCell = ({ frame, type, isVertical }) => {
    if (frame.timetableEntryChange && frame.timetableEntryChange.cancelled) {
        //return canceled
        return (
            <CellChanged vertical={isVertical}></CellChanged>
        )
    } else if (frame.timetableEntryChange && !frame.timetableEntryChange.cancelled) {
        //return change
        if (parseInt(frame.timetableEntryChange.subjectInstanceId > 0)) {
            if (type === "Teacher") {
                const group = frame.timetableEntry.group
                return (
                    <CellChanged vertical={isVertical}>
                        <div>{frame.timetableEntry.room}</div>
                        <SubjectHeading>{heading}</SubjectHeading>
                        <Additional>{group}</Additional>
                    </CellChanged>
                )
            }
            else {
                const teacher = frame.timetableEntryChange.teacherFirstName.substring(0, 1).concat(`. ${frame.timetableEntry.teacherLastName}`)
                return (
                    <CellChanged vertical={isVertical}>
                        <div>{frame.timetableEntryChange.room}</div>
                        <SubjectHeading>{heading}</SubjectHeading>
                        <Additional>{`${teacher}`}</Additional>
                    </CellChanged>
                )
            }
        } else {
            return (            
                <CellEmpty vertical={isVertical}>                
                </CellEmpty>            
        )
        }        
    }else if (frame.timetableEntry) {
        //return normal entry
        const heading = frame.timetableEntry.subjectInstanceName
        if (type === "Teacher") {
            const group = frame.timetableEntry.group
            return (
                <Cell vertical={isVertical}>
                    <div>{frame.timetableEntry.room}</div>
                    <SubjectHeading>{heading}</SubjectHeading>
                    <Additional>{group}</Additional>
                </Cell>
            )
        }
        else {
            const teacher = frame.timetableEntry.teacherFirstName.substring(0, 1).concat(`. ${frame.timetableEntry.teacherLastName}`)
            return (
                <Cell vertical={isVertical}>
                    <div>{frame.timetableEntry.room}</div>
                    <SubjectHeading>{heading}</SubjectHeading>
                    <div className="teacher-name">{`${teacher}`}</div>
                </Cell>
            )
        }
    } else { 
        return (            
                <CellEmpty vertical={isVertical}></CellEmpty>            
        )
    }   
  
}

export default TableCell