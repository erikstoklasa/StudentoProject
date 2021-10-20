import { subjectApiAddress } from "./Variables"

export const fetchSubjectInstance = async (userType, subjectId) => {
    if (subjectId) {        
        const res = await fetch(`${subjectApiAddress}/${userType}/${subjectId}`, {
            method: 'GET',
            headers: {
                'Cache-Control': 'no-cache'
            }
        })
        
        if (res.ok) {
            const data = await res.json()            
            const reqResponse = {
                success: res.ok,
                status: res.status,
                data: data
            }

            return reqResponse
        } else {
            const reqResponse = {
                success: res.ok,
                status: res.status,                
            }
            return reqResponse
        }
    }    
}