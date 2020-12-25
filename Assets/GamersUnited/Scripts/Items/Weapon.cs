using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    private ItemGrade grade;
    private EffectManager.EffectMethod hitEffect;
    private GameUnit unit;
    private System.DateTime cooldownEndTime = DateTime.MinValue;
    public GameUnit Unit { get => unit; set => unit = value; }
    protected DateTime CooldownEndTime { get => cooldownEndTime; set => cooldownEndTime = value; }
    public ItemGrade Grade { get => grade; }
    protected EffectManager.EffectMethod HitEffect { get => hitEffect; set => hitEffect = value; }

    public virtual void Init(ItemGrade grade)
    {
        this.grade = grade;
    }

    //Attack 입력시, Attack 작업을 수행할 method.
    //공격 수행시 true 반환, 실패시(탄환이 없다던가, 공격 쿨타임이 아직 안끝났다던가) false
    public abstract bool Attack();
    public virtual bool IsCanAttack()
    {
        return System.DateTime.Now >= CooldownEndTime && Unit != null;
    }
}
