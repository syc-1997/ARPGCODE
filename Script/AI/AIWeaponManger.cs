using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponManger : MonoBehaviour
{
    public AIActorManger aam;
    private Collider WeaponColL;
    private Collider WeaponColR;

    public GameObject whL;
    public GameObject whR;

    public AIWeaponController wcL;
    public AIWeaponController wcR;
    
    private void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindAIWeaponController(whL);
        wcR = BindAIWeaponController(whR);

        WeaponColL = whL.GetComponentInChildren<Collider>();
        WeaponColR = whR.GetComponentInChildren<Collider>();

        //WeaponCol = whR.GetComponentInChildren<Collider>();
        //print(transform.DeepFind("weaponHandleR"));

    }

    public AIWeaponController BindAIWeaponController(GameObject targetObj)
    {
        AIWeaponController tempWc;
        tempWc = targetObj.GetComponent<AIWeaponController>();
        if(tempWc == null)
        {
            tempWc = targetObj.AddComponent<AIWeaponController>();
        }
        tempWc.awm = this;

        return tempWc;
    }
    public void WeaponEnable()
    {
        WeaponColR.enabled = true;
    }

    public void WeaponDisable()
    {
        if(WeaponColL != null)
        {
            WeaponColL.enabled = false;
        }
        if (WeaponColR != null)
        {
            WeaponColR.enabled = false;
        }
        
        
    }

    
}
