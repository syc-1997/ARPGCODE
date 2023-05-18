using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{
    public AIActorManger aam;
    
    public float HP = 15.0f;
    public float HPMax = 15.0f;

    [Header("1st Order state flags")]
    public bool isDie;



    private void Start()
    {
        
        HP = HPMax;
    }
    private void Update()
    {
        isDie = aam.aac.CheckState("Dead");
    }
    public void AddHp (float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);
        if(HP > 0)
        {
            aam.Hit();
        }
        else
        {
            aam.Die();
        }
    }
    public void Test()
    {
        print(HP);
    }
}
