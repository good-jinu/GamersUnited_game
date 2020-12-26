using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterD : Monster
{
    private const float TauntDamage = 40f;
    private const float MissileDamage = 8f;
    private const float ExplosionDamage = 30f;
    public Transform[] Ports = new Transform[2];

    private Pattern taunt, shotMissile, explosion;
    private bool isAttack;

    private void Update()
    {
        //for Taunt Pattern
        Vector3 target = GameManager.Instance.Player.transform.position;
        if (Movespeed != 0)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), Movespeed * Time.deltaTime);
        }
        if (!isAttack)
        {
            transform.LookAt(target);
        }
    }
    private IEnumerator AI()
    {
        //Start delay
        yield return new WaitForSeconds(5f);
        while (true)
        {
            if (GameManager.Instance.Player != null && AIActive && !isAttack)
            {
                //Select Pattern
                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                    new Vector2(GameManager.Instance.Player.transform.position.x, GameManager.Instance.Player.transform.position.z));
                Pattern pattern;
                if (distance < 20f)
                {
                    pattern = SelectRandomPattern(taunt, explosion);
                }
                else
                {
                    pattern = SelectRandomPattern(taunt, shotMissile);
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
    }
    protected override void Start()
    {
        base.Start();
        Movespeed = 0;
        StartCoroutine(AI());
    }
    protected override void OnDead(Vector3 dir)
    {
        StartCoroutine(Separation());
        base.OnDead(dir);
    }

    //Pattern Method
    private IEnumerator Separation()
    {
        Monster[] monster = new Monster[2];
        //TODO : Layer를 "Dead" 로 임시로 변경하여 벽/바닥에만 충돌이 되도록 할것
        for(int i = 0; i < 2; ++i)
        {
            monster[i] = GameManager.Instance.InstantiateUnit((GameUnitList)Random.Range(1, 4), transform.position) as Monster;
            monster[i].AIActive = false;
            monster[i].transform.Rotate(Vector3.up * Random.Range(0,360));
            monster[i].Rigid.AddForce((Vector3.up + monster[i].transform.forward) * 30, ForceMode.Impulse);
            monster[i].SetInvincible(1f);
        }
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 2; ++i)
        {
            monster[i].AIActive = true;
        }
    }
    private IEnumerator Taunt()
    {
        float range = 40f;
        isAttack = true;
        var target = GameManager.Instance.Player.transform.position;
        target.y = 0;
        var warningArea = GameManager.Instance.Effect.WarningAreaEffect(target, range, 1.9f);
        warningArea.SetAttackWhenDestory(range-1, TauntDamage * Atk, 30, "Player", this, null);
        warningArea.SetAttackWhenDestory(15, 0, 20, "Monster", this, null);
        warningArea.SetSignalWhenDestory(TauntMoveEnd);
        yield return new WaitForSeconds(0.4f);
        gameObject.layer = 9;
        Ani.SetTrigger("doTaunt");
        yield return new WaitForSeconds(0.1f);
        Movespeed = Mathf.Sqrt(Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2)) / 1.1f;
        SetInvincible(1.4f);
        yield return new WaitForSeconds(1.75f);
        gameObject.layer = 8;
        isAttack = false;
    }

    private void TauntMoveEnd(Transform objTransform)
    {
        Movespeed = 0;
        transform.position = new Vector3(objTransform.position.x, transform.position.y, objTransform.position.z);
        //TODO : 충격파 이펙트 있을시 넣기
    }

    private IEnumerator ShotMissile()
    {
        isAttack = true;
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
                    script.ChaseBulletFire(15, 360, 4f, GameManager.Instance.Player.transform);
                }
                else
                {
                    script.SelfRotate(90f, true, false, false);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.7f);
        isAttack = false;
    }

    private IEnumerator Explosion()
    {
        float skillMinRange = 2.5f;
        float skillMaxRange = 30f;
        float explosionRange = 11f;
        int stage = 4;
        int explosionPerStage = 5;
        isAttack = true;
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
                instant.SetAttackWhenDestory(explosionRange-1, ExplosionDamage * Atk, 10f, "Player", this, hashSet, null);
            }
            yield return new WaitForSeconds(0.1f);
            minRange = maxRange;
        }
        yield return new WaitForSeconds(1.25f - 0.1f * stage);
        isAttack = false;
    }
}
