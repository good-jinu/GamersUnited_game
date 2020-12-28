﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : GameUnit
{
    //Monster 들이 공통적으로 사용할 컴포넌트 선언..
    private NavMeshAgent nav;
    private bool aiActive = true;
    private Animator ani;

    protected NavMeshAgent Nav { get => nav; }
    protected Animator Ani { get => ani; }
    public bool AIActive { get => aiActive; set => aiActive = value; }
    protected override void Awake()
    {
        base.Awake();
        ani = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDamaged(in Vector3 dir,in float pushPower)
    {
        base.OnDamaged(dir, pushPower);
    }
    protected override void OnDead()
    {
        Ani.SetTrigger("doDead");
        base.OnDead();
    }

    protected delegate IEnumerator AttackMethodDelegate();
    protected class Pattern
    {
        public AttackMethodDelegate attackMethod;
        public float patternCooldown;
        public int useRate;
        private System.DateTime cooldownEndTime;
        public Pattern(AttackMethodDelegate attackMethod = null, float patternCooldown = 0f,int useRate = 0)
        {
            this.attackMethod = attackMethod;
            this.patternCooldown = patternCooldown;
            this.useRate = useRate;
            cooldownEndTime = System.DateTime.MinValue;
        }
        public bool IsCooldownEnd() 
        {
            return cooldownEndTime <= System.DateTime.Now;
        }
        public void SetCooldown()
        {
            cooldownEndTime = System.DateTime.Now.AddSeconds(patternCooldown);
        }
    }
    //매개변수로 넣은 Pattern 배열에서 사용가능(쿨타임 없는상태)한 패턴을 useRate 비율에 비례하여 랜덤하게 1개 선택, 반환
    //선택한 Pattern의 Cooldown 적용(SetCooldown())은 아직 없는 상태!
    protected Pattern SelectRandomPattern(params Pattern[] patterns)
    {
        int totalRate = 0;
        Pattern result = null;
        List<Pattern> validPatternList = new List<Pattern>();
        foreach(Pattern pattern in patterns)
        {
            if (pattern.IsCooldownEnd())
            {
                totalRate += pattern.useRate;
                validPatternList.Add(pattern);
            }
        }
        int random = Random.Range(0, totalRate);
        int rangeBegin = 0;
        foreach (Pattern pattern in validPatternList)
        {
            if (rangeBegin <= random && random < rangeBegin + pattern.useRate)
            {
                result = pattern;
                break;
            }
            else
            {
                rangeBegin += pattern.useRate;
            }
        }
        return result;
    }
}
