import { gradesApiAddress } from "./Variables"
import { convertInternalToDisplay, convertDisplayToInternal } from "./GradeValueConverter"
import moment from "moment" 

export const fetchGrades = async (userType, subjectId, valueFormat = null) => {    
    const res = await fetch(`${gradesApiAddress}/${userType}?subjectInstanceId=${subjectId}${valueFormat ? `&gradeValueFormat=${valueFormat}` : ''}`, {
        method: 'GET',
        headers: {
            'Cache-Control': 'no-cache'
        }
    })   

    if (res.ok) {        
        const data = await res.json()
        if (data.length > 0) {
            data.forEach(grade => {
                Object.assign(grade, {
                    displayValue: convertInternalToDisplay(parseInt(grade.value)),
                    addedRelative: moment.utc(grade.added).locale('cs').fromNow(),
                    addedDisplay: moment.utc(grade.added).local().locale('cs').format("D. MMMM Y")
                })
            })
        }

        const reqRes = {
            success: res.ok,
            status: res.status,
            data : data
        }

        return reqRes 
    } else {
        const reqRes = {
            success: res.ok,
            status: res.status            
        }
        return reqRes
    }         
}

export const postGrade = async (userType, gradeData, subjectId, name, value, weight, date) => {
    if (userType === 'Student') {
        const gradeDate = moment(date, 'YYYY-MM-DD').toISOString()
        const reqBody = {
            value: convertDisplayToInternal(value),
            name: name,            
            weight: parseInt(weight),
            subjectInstanceId: subjectId,
            added: gradeDate
        }
    
        const res = await fetch(`${gradesApiAddress}/${userType}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(reqBody)
        })
        if (res.ok) {
            const data = await res.json()
            Object.assign(data, {
                displayValue: value,
                addedRelative: moment.utc(gradeDate).locale('cs').fromNow(),
                addedDisplay: moment.utc(gradeDate).local().locale('cs').format("D. MMMM Y")
            })

            const newArray = [data, ...gradeData]

            const reqResponse = {
                success: res.ok,
                status: res.status,
                data: newArray
            }

            return reqResponse
        } else {
            const reqResponse = {
                success: res.ok,
                status: res.status
            }
            return reqResponse
        }
    }

}

export const deleteGrades = async (userType, gradeData, idArray) => {
    const res = await fetch(`${gradesApiAddress}/${userType}/Batch`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: JSON.stringify(idArray)
    })
    if (res.ok) {
        let newArr = [...gradeData]
        idArray.forEach(id => {
            newArr = newArr.filter(grade => grade.id != id)            
        })      

        const reqResponse = {
            success: true,
            status: res.status,
            data: newArr
        }

        return reqResponse
    } else {
        const reqResponse = {
            success: false,
            status: res.status,           
        }
        return reqResponse
    }
}