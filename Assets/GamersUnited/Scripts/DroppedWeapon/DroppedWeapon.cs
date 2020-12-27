using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DW
{
    public class DroppedWeapon : MonoBehaviour
    {
        private GameObject weaponForPlayer = null;
        private Color gradeColor;
        private Light wlight;
        private ParticleSystem particle;

        private void Start()
        {
            wlight = GetComponentInChildren<Light>();
            particle = GetComponentInChildren<ParticleSystem>();
        }

        internal void Init(ItemGrade grade, WeaponType type)
        {
            switch(type)
            {
                case WeaponType.Gun:
                    weaponForPlayer = Resources.Load<GameObject>("EquippedWeapon/GunEquipped");
                    break;
                case WeaponType.Shotgun:
                    weaponForPlayer = Resources.Load<GameObject>("EquippedWeapon/ShotGunEquipped");
                    break;
                case WeaponType.Sword:
                    weaponForPlayer = Resources.Load<GameObject>("EquippedWeapon/SwordEquipped");
                    break;
                case WeaponType.Longsword:
                    weaponForPlayer = Resources.Load<GameObject>("EquippedWeapon/LongSwordEquipped");
                    break;
            }//type에 따라 다른 오브젝트 참조

            weaponForPlayer.GetComponent<Weapon>().Init(grade);

            var Pmain = particle.main;
            switch (grade)
            {
                case ItemGrade.Common:
                    gradeColor = new Color(1f, 1f, 1f);
                    gradeColor = new Color(1f, 1f, 1f);
                    break;
                case ItemGrade.Rare:
                    gradeColor = new Color(0.25f, 0.5f, 1f);
                    gradeColor = new Color(0.25f, 0.5f, 1f);
                    break;
                case ItemGrade.Unique:
                    gradeColor = new Color(1f, 1f, 1f);
                    gradeColor = new Color(1f, 0.75f, 0.125f);
                    break;
            }
            wlight.color = gradeColor;
            Pmain.startColor = gradeColor;
            //light와 particle의 color 교체
        }

        public Weapon GetWeapon()
        {
            return weaponForPlayer.GetComponent<Weapon>();
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
