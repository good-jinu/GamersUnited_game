using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform cameraPos;
    private Transform playerPos;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameData.PrefabPlayer.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPos.position + offset;
    }
}
