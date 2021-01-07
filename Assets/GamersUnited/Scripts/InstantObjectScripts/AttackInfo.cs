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

    public Vector3 AttackPosition => _attackTransform == null? _attackPosition : _attackTransform.position;
    public AttackFailedDelegate AttackFailed => _attackFailed;

    public AttackSuccessDelegate AttackSuccess => _attackSuccess;

    public int EnableHitCount => _enableHitCount;

    public string TargetTag => _targetTag;

    public float PushPower => _pushPower;

    public float Damage => _damage;

    public GameUnit Caster => _caster;

    public AttackInfo(GameUnit caster,
                      float damage,
                      float pushPower,
                      string targetTag,
                      Vector3 attackPosition,
                      int enableHitCount,
                      AttackSuccessDelegate attackSuccess,
                      AttackFailedDelegate attackFailed)
    {
        _ = caster != null ? caster : throw new System.ArgumentNullException(nameof(caster));
        if (pushPower < 0f)
            throw new System.ArgumentOutOfRangeException(nameof(pushPower), "Must be greater than or equal to 0.");
        _ = targetTag ?? throw new System.ArgumentNullException(nameof(targetTag));
        if(enableHitCount < 1)
            throw new System.ArgumentOutOfRangeException(nameof(enableHitCount), "Must be greater than or equal to 0.");

        //allocate
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

    public void SetSyncPosition(Transform target) { _attackTransform = target; }

}

