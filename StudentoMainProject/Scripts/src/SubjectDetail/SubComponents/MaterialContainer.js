import React, {useState, useEffect} from 'react'
import MaterialView from './MaterialView'
import apiAddress from '../variables.js'

const MaterialContainer = ({ materials, deleteMaterial, info }) => {
    const [studentInfo, updateStudentInfo] = useState()

    const fetchInfo = () => {
        fetch(`${apiAddress}/Auth/GetUserInfo`)
            .then(res => res.json())
            .then(data => updateStudentInfo(data))

    }

    useEffect(fetchInfo, [])

    const materialsList = materials.map(material => {
        return <MaterialView material={material} deleteMaterial={deleteMaterial} info={info} student={ studentInfo}/>
    })

    return (
        <div className="table table-responsive table-white">
            {studentInfo ? materialsList : null}
        </div>
    )
}

export default MaterialContainer