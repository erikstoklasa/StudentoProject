import { authApiAdress } from "./Variables";

export const fetchUserInfo = async () => {
    const res = await fetch(`${authApiAdress}/GetUserInfo`)
    if (res.ok) {
        const data = await res.json()
        const reqResponse = {
            success: true,
            data: data
        }
        return reqResponse
    } else {
        const reqResponse = {
            success: false            
        }
        return reqResponse
    }
}