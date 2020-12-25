﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private Dictionary<string,GameUnit> units;
    private UIManager ui;
    private EffectManager effect;
    private Player player;

    // Prefab 참조 GameData로 이전

    public static GameManager Instance { get => instance; }
    public Dictionary<string, GameUnit> Units { get => units; set => units = value; }
    public UIManager UI { get => ui; set => ui = value; }
    public EffectManager Effect { get => effect; set => effect = value; }
    public Player Player { get => player; set => player = value; }  

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            units = new Dictionary<string, GameUnit>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Scene 이 Load될때 마다 호출되는 함수
        //Scene 재시작/이동시 마다 각종 초기화 작업을 이 함수에서 수행할 것.

        //플레이어를 생성
        InstantiateUnit(GameUnitList.Player, GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position);

        //for test
        InstantiateUnit(GameUnitList.MonsterD, new Vector3(0, 0, 0));
        //test code end
    }
    public void OnUnitDead(string name, Vector3 point)
    {
        //option : 매개변수를 사망한 Unit의 이름과 사망 위치(Vector 3)으로 받을지, GameUnit 객체로 받을 지 선택

        //Scene 내 Unit(Player/Monster)가 Hp가 0이되어 사망할 시 호출된다.
        //TODO: 사망한 Unit Dictionary에서 제거
        //      Monster 사망시 확률적으로 랜덤한 아이템 생성
        //      Monster 전부 사망 시 다음 중 하나 : 바로(또는 일정시간 후) 다음 레벨로 이동 / 다음 레벨로 이동하게 할 워프 Object 생성시키기
        //                                         후자의 경우 워프 오브젝트의 스크립트도 GameManager 담당이 코딩
        //      Player 사망시 다음 중 하나 : 바로 게임 재시작 / UI창 뛰운 후 재시작 또는 메뉴로 되돌아가기 선택
    }

    //TODO : Scene 시작시 마다 Monster 랜덤으로 3개 생성/스탯 설정
    //      Scene 이동시 마다 Player의 현재 스탯(아이템 정보 포함)을 불러오기 할 것
    //      
    //    


    //InstantiateUnit(GameUnitList unit, vector3 pos,int unitLevel)
    //: pos 위치에 unit으로 지정한 GameUnit Prefab을 생성하고, GameUnit Class를 반환한다.
    //unitLevel은 GameData.multiple 배열의 인덱스로 사용되어 기본 Stat의 Hp/Atk에 곱해진다.
    //unitLevel은 반드시 0~9 사이의 정수여야 한다.
    public GameUnit InstantiateUnit(GameUnitList unit, Vector3 pos, int unitLevel = 0)
    {
        if (unitLevel < 0 || unitLevel > 9) throw new System.ArgumentOutOfRangeException();
        GameObject target = null;
        (int, float, float, int) stat = (0,0,0,0);
        switch (unit)
        {
            case GameUnitList.Player:
                target = GameData.PrefabPlayer;
                stat = GameData.GetPlayerStat();
                break;
            case GameUnitList.MonsterA:
                target = GameData.PrefabMonsterA;
                stat = GameData.GetMonsterStat(MonsterType.A);
                break;
            case GameUnitList.MonsterB:
                target = GameData.PrefabMonsterB;
                stat = GameData.GetMonsterStat(MonsterType.B);
                break;
            case GameUnitList.MonsterC:
                target = GameData.PrefabMonsterC;
                stat = GameData.GetMonsterStat(MonsterType.C);
                break;
            case GameUnitList.MonsterD:
                target = GameData.PrefabMonsterD;
                stat = GameData.GetMonsterStat(MonsterType.D);
                break;
        }
        var instant = Instantiate(target, pos, new Quaternion());
        var script = instant.GetComponent<GameUnit>();
        float multiple = GameData.multiple[unitLevel];
        script.InitStat((int)(stat.Item1*multiple), stat.Item2*multiple, stat.Item3, stat.Item4);
        return script;
    }
}

public enum GameUnitList { Player, MonsterA, MonsterB, MonsterC, MonsterD}