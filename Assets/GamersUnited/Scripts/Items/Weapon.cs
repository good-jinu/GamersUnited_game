using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//이 클래스는 이름이 바뀌거나 삭제될 수 있음
public abstract class Weapon : MonoBehaviour
{
    private ItemGrade grade;
    private EffectManager.EffectMethod hitEffect;
    private GameUnit unit;
    private System.DateTime cooldownEndTime = DateTime.MinValue;
    public EffectManager.EffectMethod HitEffect { get => hitEffect;}
    public GameUnit Unit { get => unit; set => unit = value; }
    protected DateTime CooldownEndTime { get => cooldownEndTime; set => cooldownEndTime = value; }
    public ItemGrade Grade { get => grade; }

    public virtual void Init(EffectManager.EffectMethod hitEffect, ItemGrade grade)
    {
        this.hitEffect = hitEffect;
        this.grade = grade;
    }

    //Attack 입력시, Attack 작업을 수행할 method.
    //공격 수행시 true 반환, 실패시(탄환이 없다던가, 공격 쿨타임이 아직 안끝났다던가) false
    public abstract bool Attack();
    protected virtual bool IsCanAttack()
    {
        return System.DateTime.Now >= CooldownEndTime && Unit != null;
    }
}
