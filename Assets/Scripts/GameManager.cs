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
    public static GameManager Instance { get => instance; }
    public Dictionary<string, GameUnit> Units { get => units; set => units = value; }
    public UIManager UI { get => ui; set => ui = value; }
    public EffectManager Effect { get => effect; set => effect = value; }

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
        //      Monster 중 D Type(죽을시 2마리로 분리) 사망 시 랜덤한 Monster 2기 생성하기
        //      Player 사망시 다음 중 하나 : 바로 게임 재시작 / UI창 뛰운 후 재시작 또는 메뉴로 되돌아가기 선택
    }
    //선택할 사항 1: 각종 Effect/사망한 Unit/획득한 Item을 Scene에서 그 순간에 바로 Destory할지, 아니면 비활성화 후 GameManager가 특정 타이밍에 다 Destory할지
    //              전자 선택시 Effect 처리/Unit또는 Item 습득 처리 직후 바로 Destory 하도록 코딩해야함
    //              후자 선택시 비활성화만 한 후 GameManager의 리스트 자료형에 gameObject 추가, gameObject가 특정 간격으로 자동으로 Destory하도록 코딩해야함

    //TODO : Scene 시작시 마다 Monster 랜덤으로 3개 생성/스탯 설정
    //      Scene 이동시 마다 Player의 현재 스탯(아이템 정보 포함)을 불러오기 할 것
    //      

}
