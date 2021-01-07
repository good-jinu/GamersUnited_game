using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo
{
    private readonly GameUnit _caster;
    private readonly float _damage;
    private readonly float _pushPower;
    private readonly string _targetTag;
    private readonly Vector3 _attackPosition;
    private readonly int _enableHitCount;
    private readonly AttackSuccessDelegate _attackSuccess;
    private readonly AttackFailedDelegate _attackFailed;
    private Transform _attackTransform;

    public delegate void AttackSuccessDelegate(HitInfo info);
    public delegate void AttackFailedDelegate(Vector3 position);

    public GameUnit Caster { get => _caster;}
    public float Damage { get => _damage;}
    public float PushPower { get => _pushPower;}
    public string TargetTag { get => _targetTag;}
    public Vector3 AttackPosition { get => _attackTransform == null? _attackPosition : _attackTransform.position;}
    public int EnableHitCount { get => _enableHitCount;}
    public AttackSuccessDelegate AttackSuccess { get => _attackSuccess;}
    public AttackFailedDelegate AttackFailed { get => _attackFailed;}
    public Transform AttackTransform {set => _attackTransform = value; }

    public AttackInfo(GameUnit caster,
                      float damage,
                      float pushPower,
                      string targetTag,
                      Vector3 attackPosition,
                      int enableHitCount,
                      AttackSuccessDelegate attackSuccess,
                      AttackFailedDelegate attackFailed)
    {
        _caster = caster;
        _damage = damage;
        _pushPower = pushPower;
        _targetTag = targetTag;
        _attackPosition = attackPosition;
        _enableHitCount = enableHitCount;
        _attackSuccess = attackSuccess;
        _attackFailed = attackFailed;
    }
    public AttackInfo(GameUnit caster,
                      float damage,
                      float pushPower,
                      string targetTag,
                      Vector3 attackPosition) : this(caster, damage, pushPower, targetTag, attackPosition, int.MaxValue, null, null) { }
    public AttackInfo(GameUnit caster,
                      float damage,
                      float pushPower,
                      string targetTag,
                      Vector3 attackPosition,
                      int enableHitCount) : this(caster, damage, pushPower, targetTag, attackPosition, enableHitCount, null, null) { }
    public AttackInfo(GameUnit caster,
                      float damage,
                      float pushPower,
                      string targetTag,
                      Vector3 attackPosition,
                      int enableHitCount,
                      AttackSuccessDelegate attackSuccess) : this(caster, damage, pushPower, targetTag, attackPosition, enableHitCount, attackSuccess, null) { }
    public AttackInfo(GameUnit caster,
                      float damage,
                      float pushPower,
                      string targetTag,
                      Vector3 attackPosition,
                      int enableHitCount,
                      AttackFailedDelegate attackFailed) : this(caster, damage, pushPower, targetTag, attackPosition, enableHitCount, null, attackFailed) { }
}

