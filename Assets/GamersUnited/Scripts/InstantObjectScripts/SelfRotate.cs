using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(90 * Time.deltaTime, 0, 0));
    }
}
