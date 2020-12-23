using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : GameUnit
{
    protected delegate void AttackMethodDelegate();
    protected class Pattern
    {
        public AttackMethodDelegate attackMethod;
        public float patternCooldown;
        public int useRate;
        //최근 사용 시간을 저장할 변수 선언할것
        public Pattern(AttackMethodDelegate attackMethod = null, float patternCooldown = 0f,int useRate = 0)
        {
            this.attackMethod = attackMethod;
            this.patternCooldown = patternCooldown;
            this.useRate = useRate;
        }
    }
    //매개변수로 넣은 Pattern 배열에서 사용가능(쿨타임 없는상태)한 패턴을 useRate 비율에 비례하여 랜덤하게 1개 선택, 반환
    protected Pattern SelectRandomPattern(params Pattern[] patterns)
    {
        int totalRate = 0;
        Pattern result = null;
        List<Pattern> validPatternList = new List<Pattern>();
        foreach(Pattern pattern in patterns)
        {
            //쿨타임을 확인하여 넣을 것
            //현재 생각한 방안은 Time 비교 사용
            if (true)
            {
                totalRate += pattern.useRate;
                validPatternList.Add(pattern);
            }
        }
        if (totalRate <= 0) throw new System.Exception();
        int random = Random.Range(0, totalRate);
        int rangeBegin = 0;
        foreach (Pattern pattern in validPatternList)
        {
            if(rangeBegin<=random&& random < rangeBegin + pattern.useRate)
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
