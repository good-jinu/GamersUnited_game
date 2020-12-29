using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Transform objectToFollow = null;
    private Vector3 offset = new Vector3(0, 30, -10);

    private void Start()
    {
        GameManager.Instance.MainCamera = this;
    }

    private void Update()
    {
        if(objectToFollow!=null)
        {
            transform.position = objectToFollow.position + offset;
        }
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
