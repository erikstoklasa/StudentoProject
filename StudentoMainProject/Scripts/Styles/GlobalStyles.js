import styled from 'styled-components'

export const Button = styled.div`
    display: inline-block;
    font-weight: 400;
    color: #212529;
    text-align: center;
    vertical-align: middle;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    background-color: transparent;
    border: 1px solid transparent;
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    line-height: 1.5;
    border-radius: 0.25rem;
    transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    padding: 0.4rem 0.7rem;
    border-radius: 10px;
    width: 100%;
    cursor: pointer;
    @media(min-width: 576px){
        width: auto;
    }
`
export const PrimaryButton = styled(Button)` 
    background-color: var(--primaryYellowStrong);
    border: 2px solid var(--primaryYellowStrong);
    white-space: nowrap;
    color: #1a0504;
    :hover {
        background-color: #fff;
        border-color: var(--primaryYellowStrong);
    }
`
export const DangerButton = styled(Button)` 
    color: #fff;
    background-color: #dc3545;
    border-color: #dc3545;
`
export const SecondaryButton = styled(Button)` 
    color: #000;
    background-color: #fff;
    border: 2px solid var(--primaryYellowStrong) !important;
    :hover {
        color: #000;
        background-color: var(--primaryYellowStrong);
        border-color: var(--primaryYellowStrong);
    }
`
export const Table = styled.div`
    width: 100%;
    margin-bottom: 1rem;
    color: #212529;
    display: block;    
    overflow-x: auto;
    -webkit-overflow-scrolling: touch;
`

export const WhiteTable = styled(Table)` 
    box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16);
    border-radius: 10px;
    background: #fff;
    padding: 0.5rem;
`

export const Input = styled.input`
    display: block;
    width: 100%;
    height: calc(1.5em + 0.75rem + 2px);
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #495057;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid #ced4da;
    border-radius: 0.25rem;
    transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    outline: none !important;    
`
export const StyledWarningText = styled.p` 
    color: var(--pastelRed);
    font-size: 1rem;
    font-weight: 600;  
    margin: 10px 0px 10px 0px;
`
export const StyledAlert = styled.div` 
    position: relative;
    padding: 0.75rem 1.25rem;
    margin-bottom: 1rem;
    border: 1px solid transparent;
    border-radius: 0.25rem;
`

export const StyledInfoAlert = styled(StyledAlert)` 
    color: #0c5460;
    background-color: #d1ecf1;
    border-color: #bee5eb;
`
export const StyledErrorAlert = styled(Alert)` 
    color: #721c24;
    background-color: #f8d7da;
    border-color: #f5c6cb;
`

export const StyledCloseIcon = styled.img.attrs(props => (
    {
    src: '/images/icons/delete.svg'
    }
))` 
    cursor: pointer;
    height: 30px;
`

 