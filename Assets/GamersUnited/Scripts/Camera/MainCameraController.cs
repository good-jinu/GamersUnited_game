using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Transform objectToFollow = null;
    private Vector3 offset = new Vector3(0, 30, -10);
    private Camera camComp;

    private void Start()
    {
        GameManager.Instance.MainCamera = this;
        camComp = GetComponent<Camera>();
        camComp.fieldOfView = 60f;
    }

    private void Update()
    {
        if(objectToFollow!=null)
        {
            transform.position = objectToFollow.position + offset;
        }
    }

    public Camera GetCamera()
    {
        return camComp;
    }

    public void SetFiedOfView(float FOV)
    {
        camComp.fieldOfView = FOV;
    }
    public void SetObjectToFollow(Transform target)
    {
        objectToFollow = target;
    }

    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }
}
