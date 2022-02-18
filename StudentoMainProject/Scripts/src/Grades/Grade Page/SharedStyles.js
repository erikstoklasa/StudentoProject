import styled from 'styled-components'
import { Input } from '../../../Styles/GlobalStyles'

export const Cell = styled.div` 
    height: 40px;    
    padding-top: 10px;
    padding-bottom: 10px;
    width: 100%;
    border-top: 1px solid lightgray;
    display: flex;
    justify-content: center;
    align-items: center;
`
export const Grade = styled.p`
    width: 40px;
    height: 24px;
    text-align: center;
    margin: 0;
    padding: 0;
`
export const GradeInput = styled(Input)` 
    max-width: 40px;
    max-height: 25px;
    padding: 0.375rem 0;
    -webkit-appearance: none;
    margin: 0;
    text-align: center;
`