using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameUnit
{
    public Transform weaponPoint;
    //인덱스 0 : 투구, 1 : 갑옷, 2 : 신발
    private Armor[] equip;
    private Weapon weapon;
    private Animator ani;

    //Input variables
    private float horizontal;
    private float vertical;
    private bool attackDown;
    private bool dodgeDown;
    private bool equipDown;

    //private method variables
    private Vector3 moveVec;
    private Vector3 dodgeVec;
    private bool isDodge;
    private bool isAttack;
    private bool isBorder;

    public Weapon Weapon { get => weapon; }
    public Armor[] Equip { get => equip; }

    protected override void Awake()
    {
        base.Awake();
        equip = new Armor[3];
        ani = GetComponentInChildren<Animator>();
        Type = GameUnitList.Player;
    }
    override protected void Start()
    {
        base.Start();
        GameManager.Instance.Player = this;
        //메인카메라가 본인 쫓아오게조정
        GameManager.Instance.MainCamera.SetObjectToFollow(transform);
        //weaponPoint 오른손으로 초기화
        weaponPoint = transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(1);
        //플레이어 정보 불러오기
        if (PlayerPrefs.HasKey("MaxHP"))
        {
            InitStat(PlayerPrefs.GetInt("MaxHP"), PlayerPrefs.GetFloat("HP"), Atk, Movespeed, Armor);
            if (PlayerPrefs.HasKey("Weapon"))
            {
                EquipWeapon((WeaponType)PlayerPrefs.GetInt("Weapon"), (ItemGrade)PlayerPrefs.GetInt("WeaponGrade"));
            }
        }
    }

    void Update()
    {
        if (IsDead)
            return;
        if(weapon == null)
        {
            EquipWeapon(WeaponType.Sword, ItemGrade.Common);
        }
        GetInput();
        Move();
        LookToMoveDirection();
        Dodge();
        Attack();

    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;
        FreezeRotation();
        StopToWall();
    }
    private void OnTriggerStay(Collider other)
    {
        if (equipDown && other.GetComponent<DW.DroppedWeapon>())
        {
            EquipWeapon(other.GetComponent<DW.DroppedWeapon>().GetWeapon());
            other.GetComponent<DW.DroppedWeapon>().DestroyObject();
        }
    }
    protected override void DamagedPhysic(in Vector3 dir, in float pushPower)
    {
        base.DamagedPhysic(dir, pushPower);
        ani.SetBool("isDamaged", true);
        ani.SetTrigger("doDamaged");
    }
    protected override void DamagedPhysicEnd()
    {
        base.DamagedPhysicEnd();
        ani.SetBool("isDamaged", false);
    }
    protected override void OnDead(Vector3 dir)
    {
        base.OnDead(dir);
        ani.SetTrigger("doDead");
    }


    public void EquipArmor(Armor armor)
    {
        //방어구 습득시 호출, 방어구에 정해진 스탯만큼 Unit 스탯을 증가시킴
        if (equip[(int)armor.Type] != null)
        {
            UnequipArmor(armor.Type);
        }
        var armorStat = GameData.GetArmorStat(armor.Type, armor.Grade);
        MaxHp += armorStat.Item1;
        Health += armorStat.Item1;
        Armor += armorStat.Item2;
        Movespeed += armorStat.Item3;
    }
    public void UnequipArmor(ArmorType armorType)
    {
        //방어구 해제시 호출, ArmorType으로 지정한 현재 착용중인 방어구를 해제하고, 해당 방어구로 올랏던 스탯을 감소시킴.
        var unEquipArmor = equip[(int)armorType];
        if (unEquipArmor == null)
            throw new System.Exception("장착하지 않은 방어구를 Unequip 시도함");
        equip[(int)armorType] = null;

        var armorStat = GameData.GetArmorStat(unEquipArmor.Type, unEquipArmor.Grade);
        MaxHp -= armorStat.Item1;
        Armor -= armorStat.Item2;
        Movespeed -= armorStat.Item3;
        if (MaxHp < Health)
            Health = MaxHp;
    }
    public void EquipWeapon(WeaponType equipWeapon, ItemGrade grade)
    {
        if (weapon != null)
            UnequipWeapon();

        //무기생성 함수 호출
        weapon = DW.WeaponGenerator.GetWeapon(equipWeapon, grade, weaponPoint);
        weapon.Unit = this;
        weapon.transform.SetParent(weaponPoint, false);
    }
    public void EquipWeapon(Weapon equipWeapon)
    {
        if (weapon != null)
            UnequipWeapon();
        //Weapon을 매개변수로 받는 오버로드
        equipWeapon.Unit = this;
        weapon = equipWeapon;
        //무기 Right hand 쪽에 장착
        weapon.transform.SetParent(weaponPoint, false);
        //weapon 오브젝트 활성화
        weapon.gameObject.SetActive(true);
    }
    public void UnequipWeapon()
    {
        Destroy(weapon.gameObject);
        weapon = null;
    }

    //private Part
    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        attackDown = Input.GetButton("Fire1");
        dodgeDown = Input.GetButton("Dodge");
        equipDown = Input.GetButtonDown("Equip");
    }

    private void LookToMoveDirection()
    {
        if (IsDamaged || isAttack)
            return;
        transform.LookAt(transform.position + moveVec);
    }
    private void LookToMouse()
    {
        Ray ray = GameManager.Instance.MainCamera.GetCamera().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }
    }
    private void Move()
    {
        moveVec = isDodge ? dodgeVec : new Vector3(horizontal, 0, vertical).normalized;
        if (isAttack || IsDamaged || moveVec == Vector3.zero)
        {
            ani.SetBool("isRun", false);
        }
        else
        {
            if(!isBorder)
            transform.position += moveVec * Movespeed * Time.deltaTime * (isDodge ? 2f : 1f);
            ani.SetBool("isRun", true);
        }
    }
    private void Dodge()
    {
        if (!dodgeDown || isAttack || IsDamaged || isDodge || moveVec == Vector3.zero)
            return;
        dodgeVec = moveVec;
        isDodge = true;
        ani.SetTrigger("doDodge");
        Invoke("DodgeEnd", 0.5f);
    }
    private void DodgeEnd()
    {
        isDodge = false;
    }
    private void Attack()
    {
        if (!attackDown || weapon == null || IsDamaged || isAttack || isDodge)
            return;
        LookToMouse();
        if (Weapon.Attack())
        {
            string animationName = null;
            float animationTime = 0f;
            isAttack = true;
            switch (Weapon.Type)
            {
                case WeaponType.Gun:
                case WeaponType.Shotgun:
                    animationName = "doShot";
                    animationTime = 0.3f;
                    CheckAmmo();
                    break;
                case WeaponType.Sword:
                case WeaponType.Longsword:
                    animationName = "doSwing";
                    animationTime = 0.4f;
                    break;
            }
            ani.SetBool("isRun", false);
            ani.SetTrigger(animationName);
            Invoke("AttackEnd", animationTime);
        }
    }
    private void CheckAmmo()
    {
        if(weapon is Gun)
        {
            if (((Gun)weapon).Ammo <= 0)
                UnequipWeapon();
        }
        else if (weapon is ShotGun)
        {
            if (((ShotGun)weapon).Ammo <= 0)
                UnequipWeapon();
        }
    }
    private void AttackEnd()
    {
        isAttack = false;
    }

    private void FreezeRotation()
    {
        Rigid.angularVelocity = Vector3.zero;
    }
    private void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
}
