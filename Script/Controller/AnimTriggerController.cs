using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTriggerController : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void RestTrigger (string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }


}
