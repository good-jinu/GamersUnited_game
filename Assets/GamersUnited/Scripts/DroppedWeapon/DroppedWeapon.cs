using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DW;


namespace DW
{
    public class DroppedWeapon : MonoBehaviour
    {
        private GameObject weaponForPlayer = null;
        private Color gradeColor;
        private Light wlight = null;
        private ParticleSystem particle = null;
        private Rigidbody rigid;
        private SphereCollider sphereC;

        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
            sphereC = GetComponent<SphereCollider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Floor"))
            {
                rigid.isKinematic = true;
                sphereC.enabled = false;
            }
        }

        private void InitType(WeaponType type)
        {
            GameObject meshObj = null;
            GameObject meshObjE = null;
            //meshObjE는 플레이어에게 장착된 무기의 메쉬오브젝트
            weaponForPlayer = new GameObject("WeaponEquipped");
            switch (type)
            {
                case WeaponType.Gun:
                    weaponForPlayer.AddComponent<Gun>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/GunMesh");
                    meshObjE = Resources.Load<GameObject>("DroppedWeapon/GunMesh");
                    break;
                case WeaponType.Shotgun:
                    weaponForPlayer.AddComponent<ShotGun>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/ShotGunMesh");
                    meshObjE = Resources.Load<GameObject>("DroppedWeapon/ShotGunMesh");
                    break;
                case WeaponType.Sword:
                    weaponForPlayer.AddComponent<Sword>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/SwordMesh");
                    meshObjE = Resources.Load<GameObject>("DroppedWeapon/SwordMesh");
                    break;
                case WeaponType.Longsword:
                    weaponForPlayer.AddComponent<LongSword>();
                    meshObj = Resources.Load<GameObject>("DroppedWeapon/LongSwordMesh");
                    meshObjE = Resources.Load<GameObject>("DroppedWeapon/LongSwordMesh");
                    break;
            }//type에 따라 다른 오브젝트 참조

            meshObj = Instantiate<GameObject>(meshObj);
            meshObj.transform.position = new Vector3(-0.5f, 1f, 0f);
            meshObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -30));
            meshObj.transform.SetParent(transform, false);
            //메쉬오브젝트 자식오브젝트로 추가

            weaponForPlayer.gameObject.SetActive(false);
            Instantiate(meshObjE, weaponForPlayer.transform, false);
            //weaponForPlayer에 메쉬오브젝트 자식오브젝트로 추가
        }

        private void InitGrade(ItemGrade grade)
        {
            weaponForPlayer.GetComponent<Weapon>().Init(grade);
            wlight = GetComponentInChildren<Light>();
            particle = GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule Pmain = particle.main;
            switch (grade)
            {
                case ItemGrade.Common:
                    wlight.color = new Color(1f, 1f, 1f);
                    Pmain.startColor = new Color(1f, 1f, 1f);
                    break;
                case ItemGrade.Rare:
                    wlight.color = new Color(0.25f, 0.5f, 1f);
                    Pmain.startColor = new Color(0.25f, 0.5f, 1f);
                    break;
                case ItemGrade.Unique:
                    wlight.color = new Color(1f, 1f, 1f);
                    Pmain.startColor = new Color(1f, 0.75f, 0.125f);
                    break;
            }
            //light와 particle의 color 교체
        }

        internal void Init(ItemGrade grade, WeaponType type)
        {
            InitType(type);
            //무기 타입 초기화 과정
            InitGrade(grade);
            //무기 등급 초기화 과정
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
