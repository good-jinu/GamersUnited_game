using System.Collections;
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


    //
    // Prefab 정보를 Public 변수로 저장한 후 Unity상의 GameManager에서 직접 연결해줘야 하는 것으로 보임.
    // 오브젝트를 인스턴스화 시킬때 Prefab 정보가 필요할 시 GameManager.cs에 public 변수(Property 아님) 추가 후 사용할 것
    public GameObject prefabShotGunBullet;
    public GameObject prefabGunBullet;
    public GameObject prefabSwordAttackRange;
    public GameObject prefabLongSwordAttackRange;
    //

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
    //      D Type Monster의 사망시 분리(2기 생성)를 위해 특정 위치에 몬스터 생성하는 public method 추가해줄 것(몬스터 종류 선택/메소드 내에서 랜덤은 자유)

}
