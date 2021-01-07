using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo
{
    private readonly GameUnit _hitUnit;
    private readonly float _damage;
    private readonly Vector3 _hitPosition;
    public HitInfo(GameUnit hitUnit, float damage, Vector3 hitPosition)
    {
        _ = hitUnit != null ? hitUnit : throw new System.ArgumentNullException(nameof(hitUnit));
        //allocate
        _hitUnit = hitUnit;
        _damage = damage;
        _hitPosition = hitPosition;
    }

    public Vector3 HitPosition => _hitPosition;

    public float Damage => _damage;

    public GameUnit HitUnit => _hitUnit;
}