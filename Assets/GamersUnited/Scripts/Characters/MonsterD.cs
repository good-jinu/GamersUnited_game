﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterD : Monster
{
    private const float TauntDamage = 40f;
    private const float MissileDamage = 8f;
    private const float ExplosionDamage = 30f;
    public Transform[] Ports = new Transform[2];

    private Pattern taunt, shotMissile, explosion;
    private bool doTaunt;
    private Vector2 tauntTarget;

    protected override void Update()
    {
        if (IsDead)
            return;
        //for Taunt Pattern
        if (doTaunt)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, new Vector3(tauntTarget.x, transform.position.y, tauntTarget.y), Movespeed * Time.deltaTime);
        }
        if (!IsAttack && GameManager.Instance.Player != null)
        {
            transform.LookAt(GameManager.Instance.Player.transform);
        }
    }
    private IEnumerator AI()
    {
        //Start delay
        yield return new WaitForSeconds(5f);
        while (true)
        {
            if (GameManager.Instance.Player != null && AIActive && !IsAttack && !IsDead)
            {
                //Select Pattern
                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                    new Vector2(GameManager.Instance.Player.transform.position.x, GameManager.Instance.Player.transform.position.z));
                Pattern pattern = null;
                if (distance < 25f)
                {
                    pattern = SelectRandomPattern(taunt, explosion);
                }
                else if (distance < 40f)
                {
                    pattern = SelectRandomPattern(taunt, explosion,shotMissile);
                }
                else
                {
                    SelectRandomPattern(taunt, shotMissile);
                }
                //Do Pattern
                if(pattern != null)
                {
                    StartCoroutine(pattern.attackMethod());
                    pattern.SetCooldown();
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    //override Part

    protected override void Awake()
    {
        base.Awake();
        taunt = new Pattern(Taunt, 8f, 1);
        shotMissile = new Pattern(ShotMissile, 3.5f, 3);
        explosion = new Pattern(Explosion, 6f, 2);
        Type = GameUnitList.MonsterD;
    }
    protected override void Start()
    {
        base.Start();
        Movespeed = 0;
        StartCoroutine(AI());
    }
    protected override void OnDead(Vector3 dir)
    {
        base.OnDead(dir);
        StartCoroutine(Separation());
    }
    protected override void OnDamaged(in Vector3 dir, in float pushPower)
    {
        
    }
    protected override void Targeting()
    {

    }

    //Pattern Method
    private IEnumerator Separation()
    {
        Monster[] monster = new Monster[2];
        for(int i = 0; i < 2; ++i)
        {
            monster[i] = GameManager.Instance.InstantiateUnit((GameUnitList)Random.Range(1, 4), transform.position) as Monster;
            monster[i].AIActive = false;
            monster[i].transform.Rotate(Vector3.up * Random.Range(0,360));
            monster[i].Rigid.AddForce((Vector3.up + monster[i].transform.forward) * 30, ForceMode.Impulse);
            monster[i].SetInvincible(1f);
            monster[i].gameObject.layer = 12;
        }
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 2; ++i)
        {
            monster[i].AIActive = true;
            monster[i].Rigid.velocity = Vector3.zero;
            monster[i].gameObject.layer = 8;
        }
    }
    private IEnumerator Taunt()
    {
        float range = 40f;
        IsAttack = true;
        var target = GameManager.Instance.Player.transform.position;
        target.y = 0;
        var warningArea = GameManager.Instance.Effect.WarningAreaEffect(target, range, 2f);
        warningArea.SetAttackWhenDestory(range, TauntDamage * Atk, 15, "Player", this, null);
        warningArea.SetAttackWhenDestory(15, 0, 10, "Monster", this, null);
        warningArea.SetSignalWhenDestory(TauntMoveEnd);
        yield return new WaitForSeconds(0.4f);
        gameObject.layer = 9;
        Ani.SetTrigger("doTaunt");
        yield return new WaitForSeconds(0.1f);
        Movespeed = Mathf.Sqrt(Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2)) / 1.1f;
        tauntTarget = new Vector2(target.x, target.z);
        doTaunt = true;
        SetInvincible(1.5f);
        yield return new WaitForSeconds(1.75f);
        gameObject.layer = 8;
        IsAttack = false;
    }

    private void TauntMoveEnd(Transform objTransform)
    {
        Movespeed = 0;
        transform.position = new Vector3(objTransform.position.x, transform.position.y, objTransform.position.z);
        doTaunt = false;
        //TODO : 충격파 이펙트 있을시 넣기
    }

    private IEnumerator ShotMissile()
    {
        IsAttack = true;
        Ani.SetTrigger("doShot");
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < 2; ++i) {
            var missile = Instantiate(GameData.PrefabMissileBoss, Ports[i].position, transform.rotation);
            var scripts = missile.GetComponentsInChildren<InstantObject>();
            foreach(InstantObject script in scripts)
            {
                if(script as AttackObject)
                {
                    ((AttackObject)script).Init(Atk * MissileDamage, "Player", 0, missile.transform.position, this, 1, null);
                    script.ChaseBulletFire(7.5f, 90, 8f, GameManager.Instance.Player.transform);
                }
                else
                {
                    script.SelfRotate(90f, true, false, false);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.7f);
        IsAttack = false;
    }

    private IEnumerator Explosion()
    {
        float skillMinRange = 2.5f;
        float skillMaxRange = 30f;
        float explosionRange = 11f;
        int stage = 4;
        int explosionPerStage = 5;
        IsAttack = true;
        Ani.SetTrigger("doBigShot");
        yield return new WaitForSeconds(0.75f);
        float minRange = skillMinRange;
        float rangeInterval = (skillMaxRange - skillMinRange) / stage;
        var hashSet = new HashSet<GameObject>();
        for (int i = 0; i < stage; ++i)
        {
            float maxRange = minRange += rangeInterval;
            float angleInterval = 360f / ((i + 1) * explosionPerStage);
            for (int j = 0; j < (i + 1) * explosionPerStage; ++j)
            {
                var angle = j * angleInterval + Random.Range(-angleInterval / 2, angleInterval / 2) * Mathf.Deg2Rad;
                var distance = Random.Range(minRange, maxRange);
                //Covert angle to dirVec
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);
                Vector3 effectivePos = new Vector3(cos, 0, sin);
                effectivePos *= distance;
                effectivePos.x += transform.position.x;
                effectivePos.z += transform.position.z;
                var instant = GameManager.Instance.Effect.WarningAreaEffect(effectivePos, explosionRange, 1.5f);
                instant.SetAttackWhenDestory(explosionRange, ExplosionDamage * Atk, 5f, "Player", this, hashSet, null);
            }
            yield return new WaitForSeconds(0.1f);
            minRange = maxRange;
        }
        yield return new WaitForSeconds(1.25f - 0.1f * stage);
        IsAttack = false;
    }
}
