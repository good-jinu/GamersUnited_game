using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIMan
{
    public class QuitTheGame : MonoBehaviour
    {
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}