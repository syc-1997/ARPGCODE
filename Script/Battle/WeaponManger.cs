using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManger : IActorMangerInterface
{
    public Collider WeaponColL; // ×ó‚È¤ÎÎäÆ÷¤Î¥³¥é¥¤¥À©`
    public Collider WeaponColR; // ÓÒ‚È¤ÎÎäÆ÷¤Î¥³¥é¥¤¥À©`

    public GameObject whL; // ×ó‚È¤ÎÎäÆ÷¥Ï¥ó¥É¥é
    public GameObject whR; // ÓÒ‚È¤ÎÎäÆ÷¥Ï¥ó¥É¥é

    public WeaponController wcL; // ×ó‚È¤ÎÎäÆ÷¥³¥ó¥È¥í©`¥é
    public WeaponController wcR; // ÓÒ‚È¤ÎÎäÆ÷¥³¥ó¥È¥í©`¥é

    private void Awake() // Awake¥á¥½¥Ã¥É
    {
        // ÎäÆ÷¥Ï¥ó¥É¥é¤òÒŠ¤Ä¤±¤Æ¥Ð¥¤¥ó¥É
        whL = transform.DeepFind("weaponHandleL").gameObject;
        wcL = BindWeaponController(whL);

        whR = transform.DeepFind("weaponHandleR").gameObject;
        wcR = BindWeaponController(whR);
    }

    private void Start() // Start¥á¥½¥Ã¥É
    {
        // ÎäÆ÷¤Î¥³¥é¥¤¥À©`¤òÈ¡µÃ
        WeaponColL = whL.GetComponentInChildren<Collider>();
        WeaponColR = whR.GetComponentInChildren<Collider>();
    }

    // ÎäÆ÷¤Î¥³¥é¥¤¥À©`¤ò¸üÐÂ
    public void UpdateWeaponCollider(string side, Collider col)
    {
        if (side == "L") // ×ó‚È¤ÎˆöºÏ
        {
            WeaponColL = col;
        }
        else if (side == "R") // ÓÒ‚È¤ÎˆöºÏ
        {
            WeaponColR = col;
        }
    }

    // WeaponController¤ò¥Ð¥¤¥ó¥É
    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>(); 
        if (tempWc == null) // ´æÔÚ¤·¤Ê¤¤ˆöºÏ
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this; 

        return tempWc; // WeaponController¤ò·µ¤¹
    }

    // ÎäÆ÷¤òÓÐ„¿¤Ë¤¹¤ë
    public void WeaponEnable()
    {
        if (am.ac.CheckStateTag("AttackL")) // ×ó‚È¹¥“Ä¤ÎˆöºÏ
        {
            WeaponColL.enabled = true;
        }
        if (am.ac.CheckStateTag("AttackR")) // ÓÒ‚È¹¥“Ä¤ÎˆöºÏ
        {
            WeaponColR.enabled = true;
        }
    }

    // ÎäÆ÷¤òŸo„¿¤Ë¤¹¤ë
    public void WeaponDisable()
    {
        if (WeaponColL != null) // ×ó‚È¤Î¥³¥é¥¤¥À©`¤¬´æÔÚ¤¹¤ëˆöºÏ
        {
            WeaponColL.enabled = false;
        }
        if (WeaponColR != null) // ÓÒ‚È¤Î¥³¥é¥¤¥À©`¤¬´æÔÚ¤¹¤ëˆöºÏ
        {
            WeaponColR.enabled = false;
        }
    }
}
