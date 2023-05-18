using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonCamera : MonoBehaviour
{
    public IUserInput pi;
    public float playerHandleSpeed;
    public float vertiaclSpeed;
    public Image lockDot;
    public bool lockState;
    public bool isAI = false;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject thirdPersonCamera;
    private LockTarget lockTarget;
    private bool TimeON;
    private float UnLockTime;



    private Vector3 cameraDampVelocity;

    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;
        if (!isAI)
        {
            thirdPersonCamera = Camera.main.gameObject;
            lockDot.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        lockState = false;
    }
    private void FixedUpdate()
    {
        if (lockTarget == null)
        {
            Vector3 tempModeEuler = model.transform.eulerAngles;
            playerHandle.transform.Rotate(Vector3.up, pi.Jright * playerHandleSpeed * Time.deltaTime);
            tempEulerX -= pi.Jup * vertiaclSpeed * Time.deltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -10, 50);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

            model.transform.eulerAngles = tempModeEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt
                (lockTarget.obj.transform.position);
        }
        if (!isAI)
        {
            thirdPersonCamera.transform.position = Vector3.SmoothDamp
            (thirdPersonCamera.transform.position, transform.position, ref cameraDampVelocity, 0.1f);

            thirdPersonCamera.transform.LookAt(cameraHandle.transform);
        }


    }
    private void LateUpdate()
    {
        if (lockTarget != null)
        {
            //print(lockTarget.halfHeight);
            if (!isAI)
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint
                (lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            }


            float TargetDistance = Vector3.Distance(model.transform.position, lockTarget.obj.transform.position);
            if (TargetDistance > 15f)
            {
                UnLock();
            }

            Ray rayGround = new Ray(model.transform.position + new Vector3(0, model.transform.lossyScale.y, 0),
                model.transform.forward);
            RaycastHit hitGround;
            if (Physics.Raycast(rayGround, out hitGround, TargetDistance, LayerMask.GetMask("Ground")))
            {
                TimeON = true;
            }
            else
            {
                TimeON = false;
            }
            if (TimeON)
            {
                UnLockTime += Time.deltaTime;
            }
            else
            {
                UnLockTime = 0;
            }
            if (UnLockTime > 1.5f)
            {
                UnLock();
            }
            if (lockTarget.am != null && lockTarget.am.asm.isDie)
            {
                UnLock();
            }
        }
    }

    public void LockUnlock()
    {
        //try lock
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f),
            model.transform.rotation, LayerMask.GetMask(isAI ? "Player" : "Enemy"));
        if (cols.Length == 0)
        {
            UnLock();
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget != null && lockTarget.obj == col.gameObject)
                {
                    UnLock();
                    break;
                }
                lockTarget = new LockTarget(col.gameObject, col.bounds.extents.y);
                //print(col.name);
                lockDot.enabled = true;
                lockState = true;
                break;
            }
        }
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public AIActorManger am;
        public LockTarget(GameObject _obj, float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
            am = _obj.GetComponent<AIActorManger>();
        }
    }

    public void UnLock()
    {
        lockTarget = null;
        if (!isAI)
        {
            lockDot.enabled = false;
        }
        lockState = false;
    }


}

