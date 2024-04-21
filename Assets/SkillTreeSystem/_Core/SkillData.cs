using UnityEngine;

/*
 * custom skill data;
 * 해당 클래스 상속해 커스텀 스킬 SO 클래스 생성
 */
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public int sp;

    /*
     * 필요한 공통 스킬 정보를 여기에 추가
     */
}
