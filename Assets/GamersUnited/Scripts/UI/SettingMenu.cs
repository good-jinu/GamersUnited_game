using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIMan
{
    public class SettingMenu : MonoBehaviour
    {
        public Slider camView;
        private void OnDisable()
        {
            GameManager.Instance.MainCamera.SetFiedOfView(camView.value);
        }

        public void ExitSettingMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
