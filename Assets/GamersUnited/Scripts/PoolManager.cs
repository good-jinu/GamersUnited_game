using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
    //initial pooling count variables
    [SerializeField]
    private int bulletCount;
    [SerializeField]
    private int missileCount;
    [SerializeField]
    private int missileBossCount;
    [SerializeField]
    private int warningAreaEffectCount;
    [SerializeField]
    private int explosionEffectCount;
    [SerializeField]
    private int capsuleAttackCount;
    [SerializeField]
    private int monsterMeleeAttackCount;
    [SerializeField]
    private int hitEffectACount;

    //Container
    private Queue<InstantObject> _bullet;
    private Queue<InstantObject> _missile;
    private Queue<InstantObject> _missileBoss;
    private Queue<InstantObject> _warningAreaEffect;
    private Queue<InstantObject> _explosionEffect;
    private Queue<InstantObject> _capsuleAttack;
    private Queue<InstantObject> _monsterMeleeAttack;
    private Queue<InstantObject> _hitEffectA;

    public enum AttackObjectList
    {
        Bullet, Missile, MissileBoss, CapsuleAttack, MonsterMeleeAttack
    }
    public enum ParticleList
    {
        Explosion, HitEffectA
    }

    //Initialize (Unity method)
    private void Awake()
    {
        _bullet = new Queue<InstantObject>();
        for(int i = 0; i < bulletCount; ++i)
        {
            AddToContainer(GameData.PrefabGunBullet, _bullet);
        }
        _missile = new Queue<InstantObject>();
        for (int i = 0; i < bulletCount; ++i)
        {
            AddToContainer(GameData.PrefabMissile, _missile);
        }
        _missileBoss = new Queue<InstantObject>();
        for (int i = 0; i < bulletCount; ++i)
        {
            AddToContainer(GameData.PrefabMissileBoss, _missileBoss);
        }
        _warningAreaEffect = new Queue<InstantObject>();
        for (int i = 0; i < warningAreaEffectCount; ++i)
        {
            AddToContainer(GameData.PrefabWarningArea, _warningAreaEffect);
        }
        _explosionEffect = new Queue<InstantObject>();
        for (int i = 0; i < explosionEffectCount; ++i)
        {
            AddToContainer(GameData.PrefabExplosion, _explosionEffect);
        }
        _capsuleAttack = new Queue<InstantObject>();
        for (int i = 0; i < capsuleAttackCount; ++i)
        {
            AddToContainer(GameData.PrefabCapsuleAttackArea, _capsuleAttack);
        }
        _monsterMeleeAttack = new Queue<InstantObject>();
        for (int i = 0; i < monsterMeleeAttackCount; ++i)
        {
            AddToContainer(GameData.PrefabMonsterMeleeAttackArea, _monsterMeleeAttack);
        }
        _hitEffectA = new Queue<InstantObject>();
        for (int i = 0; i < monsterMeleeAttackCount; ++i)
        {
            AddToContainer(GameData.PrefabHitEffectA, _hitEffectA);
        }
    }
    private void Start()
    {
        GameManager.Instance.Pooling = this;
    }

    //public method
    public AttackObject GetAttackObject(AttackObjectList type)
    {
        Queue<InstantObject> container = null;
        GameObject prefab = null;

        switch (type)
        {
            case AttackObjectList.Bullet:
                container = _bullet;
                prefab = GameData.PrefabGunBullet;
                break;
            case AttackObjectList.Missile:
                container = _missile;
                prefab = GameData.PrefabMissile;
                break;
            case AttackObjectList.MissileBoss:
                container = _missileBoss;
                prefab = GameData.PrefabMissileBoss;
                break;
            case AttackObjectList.CapsuleAttack:
                container = _capsuleAttack;
                prefab = GameData.PrefabCapsuleAttackArea;
                break;
            case AttackObjectList.MonsterMeleeAttack:
                container = _monsterMeleeAttack;
                prefab = GameData.PrefabMonsterMeleeAttackArea;
                break;
        }

        if(container.Count == 0)
        {
            AddToContainer(prefab,container);
        }
        AttackObject rv = container.Dequeue() as AttackObject;
        rv.gameObject.SetActive(true);
        return rv;
    }
    public Utility.Pair<InstantObject,InstantObject> GetWarningAreaEffect()
    {
        Utility.Pair<InstantObject, InstantObject> pair = new Utility.Pair<InstantObject, InstantObject>();
        if(_warningAreaEffect.Count == 0)
        {
            AddToContainer(GameData.PrefabWarningArea, _warningAreaEffect);
        }
        pair.first = _warningAreaEffect.Dequeue();
        pair.first.gameObject.SetActive(true);
        foreach (var script in pair.first.GetComponentsInChildren<InstantObject>())
        {
            if (script != pair.first)
            {
                pair.second = script;
            }
        }
        return pair;
    }
    public InstantObject GetParticleEffect(ParticleList type)
    {
        Queue<InstantObject> container = null;
        GameObject prefab = null;

        switch (type)
        {
            case ParticleList.Explosion:
                container = _explosionEffect;
                prefab = GameData.PrefabExplosion;
                break;
            case ParticleList.HitEffectA:
                container = _hitEffectA;
                prefab = GameData.PrefabHitEffectA;
                break;
        }

        if (container.Count == 0)
        {
            AddToContainer(prefab, container);
        }
        InstantObject rv = container.Dequeue();
        rv.gameObject.SetActive(true);
        return rv;
    }

    //private method

    private void AddToContainer(GameObject prefab, Queue<InstantObject> container)
    {
        var instant = Instantiate(prefab, transform);
        var script = instant.GetComponent<InstantObject>();
        script.SetPoolingContainer(container);
        script.gameObject.SetActive(false);
        container.Enqueue(script);
    }
    
}
