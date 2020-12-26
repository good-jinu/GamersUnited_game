using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterD : Monster
{
    private const float TauntDamage = 30f;
    private void Update()
    {
        Vector3 target = GameManager.Instance.Player.transform.position;
        if(Movespeed != 0)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), Movespeed * Time.deltaTime);
        }

        //for test
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Taunt());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 1, 0);
        }
        //test code end
    }

    //override Part

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        Movespeed = 0;
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
        //TODO : 공격 시작, 다른 행동 하지 않도록 처리하기
        var target = GameManager.Instance.Player.transform.position;
        target.y = 0;
        var warningArea = GameManager.Instance.Effect.WarningAreaEffect(target, range, 1.6f);
        warningArea.SetAttackWhenDestory(range, TauntDamage * Atk, 0, "Player", this, null);
        warningArea.SetAttackWhenDestory(range, 0, 0, "Monster", this, null);
        warningArea.SetSignalWhenDestory(TauntMoveEnd);
        yield return new WaitForSeconds(0.45f);
        gameObject.layer = 9;
        Ani.SetTrigger("doTaunt");
        yield return new WaitForSeconds(0.05f);
        Movespeed = Mathf.Sqrt(Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2)) / 1.1f;
        yield return new WaitForSeconds(1.35f);
        gameObject.layer = 8;

    }

    private void TauntMoveEnd(Transform objTransform)
    {
        Movespeed = 0;
        transform.position = new Vector3(objTransform.position.x, transform.position.y, objTransform.position.z);
        //TODO : 충격파 이펙트 있을시 넣기
    }
}
