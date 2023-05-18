using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;

public class DataBase 
{
    private string weaponDatabaseFileName = "weaponData";
    public readonly JSONObject weaponDataBaes;
    public DataBase()
    {
        TextAsset weaponContent = Resources.Load(weaponDatabaseFileName) as TextAsset;
        weaponDataBaes = new JSONObject(weaponContent.text);
    }

}
