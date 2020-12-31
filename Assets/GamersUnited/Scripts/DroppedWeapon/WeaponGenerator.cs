using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DW
{
    public class WeaponGenerator
    {
        public static Weapon GetWeapon(WeaponType type, ItemGrade grade, Transform par)
        {
            //type, grade의 무기를 par의 자식오브젝트로 생성해주는 스태틱 함수

            //무기 오브젝트 생성
            GameObject weaponObj = new GameObject("WeaponEuipped");
            GameObject meshObj = null;
            Weapon weapon = null;

            weaponObj.transform.SetParent(par, false);
            weaponObj.SetActive(false);
            //무기 오브젝트 비활성화

            //무기 타입에 따른 처리
            switch(type)
            {
                case WeaponType.Gun:
                    weapon = weaponObj.AddComponent<Gun>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/GunMesh");
                    break;
                case WeaponType.Shotgun:
                    weapon = weaponObj.AddComponent<ShotGun>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/ShotGunMesh");
                    break;
                case WeaponType.Sword:
                    weapon = weaponObj.AddComponent<Sword>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/SwordMesh");
                    break;
                case WeaponType.Longsword:
                    weapon = weaponObj.AddComponent<LongSword>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/LongSwordMesh");
                    break;
            }
            Object.Instantiate(meshObj, weaponObj.transform, false);

            //무기 등급에 따른 처리
            weapon.Init(grade);

            //오브젝트 활성화 후 반환
            weaponObj.SetActive(true);
            return weapon;
        }
    }
}
