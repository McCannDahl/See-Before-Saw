using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sbsUtils
{
    public static Vector3 cloneVector(Vector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
    public static Quaternion cloneQuaternion(Quaternion v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }
    public static MaterialListRowData BeamToMaterialListRowData(GameObject beam)
    {
        OnClickShowMaterialInfo script = beam.GetComponent<OnClickShowMaterialInfo>();
        MaterialListRowData newData = new MaterialListRowData(1, script.getDimentionString(), script.materialName, script.getActualDimentions(), script.getMiniDimentions());
        return newData;
    }
    public static List<MaterialListRowData> BuildingToRowData(GameObject buildingFrame)
    {
        List<MaterialListRowData> rs = new List<MaterialListRowData>();
        foreach (Transform beam in buildingFrame.transform)
        {
            if (beam != null && beam.gameObject != null)
            {
                var newData = BeamToMaterialListRowData(beam.gameObject);
                foreach (MaterialListRowData rowData in rs)
                {
                    if (rowData != null && rowData.name != null && rowData.dim != null && newData != null && newData.dim != null && newData.name != null)
                    {
                        if (rowData.name == newData.name && rowData.dim == newData.dim)
                        {
                            rowData.qty += 1;
                            newData = null;
                        }
                    }
                }
                if (newData != null && newData.qty > 0)
                {
                    rs.Add(newData);
                }
            }
        }
        return rs;
    }
}
