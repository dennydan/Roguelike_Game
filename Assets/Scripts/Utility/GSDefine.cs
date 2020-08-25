using UnityEngine;

public class GSDefine : MonoBehaviour
{

    public enum PlayerState
    {
        IDLE = 1,
        ATTACK,
        CASTSPELL,
        DODGE,
        JUMP,
        DIE 
    }
    /** <summary>
    1. 新增攻擊特效時也要新增枚舉名稱                            
    2. 前面的特效為玩家角色使用
     </summary>
    */
    public enum AttackEffect
    {
        Hit_POOF = 0,
        HIT_VIRUS
    }
}
