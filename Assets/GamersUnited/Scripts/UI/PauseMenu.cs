using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIMan
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject settingMenu;
        private void OnEnable()
        {
            settingMenu.SetActive(false);
        }
        private void OnDisable()
        {
        }

        public void EnableSettingMenu()
        {
            settingMenu.SetActive(true);
        }
    }
}
