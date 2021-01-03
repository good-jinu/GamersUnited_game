using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalObj : MonoBehaviour
{
    private string sceneName;

    public string SceneName { get => sceneName; set => sceneName = value; }

    public void Start()
    {
        sceneName = "Scene2";
    }
    private void OnTriggerStay(Collider other)
    {
        NextStage();
    }

    private void NextStage()
    {
        GameManager.Instance.SavePlayerInfo();
        SceneManager.LoadSceneAsync(sceneName);
    }
}
