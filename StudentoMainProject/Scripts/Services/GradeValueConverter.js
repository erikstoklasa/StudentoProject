export const convertInternalToDisplay = (value) => {
    switch (value) {
        case 110:
            return '1+'
        case 100:
            return '1'
        case 90:
            return '1-'
        case 85:
            return '2+'
        case 75:
            return '2'
        case 65:
            return '2-'
        case 60:
            return '3+'
        case 50:
            return '3'
        case 40:
            return '3-'
        case 35:
            return '4+'
        case 25:
            return '4'
        case 15:
            return '4-'
        case 10:
            return '5+'
        case 0:
            return '5'
        case -10:
            return '5-'
        default:
            return 'Incorrect value'
    }  
}


export const convertDisplayToInternal = (value) => {        
    switch (value) {
        case '1*':
            return 110
        case '1+':
            return 110
        case '1':
            return 100
        case '1-':
            return 90
        case '2+':
            return 85
        case '2':
            return 75
        case '2-':
            return 65
        case '3+':
            return 60
        case '3':
            return 50
        case '3-':
            return 40
        case '4+':
            return 35
        case '4':
            return 25
        case '4-':
            return 15
        case '5+':
            return 10
        case '5':
            return 0
        case '5-':
            return -10
        default:
            return 'Incorrect value'
    }  
}