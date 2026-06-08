using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    //动画相关
    public Animator animator;
    //位移相关寻路组件
    private NavMeshAgent agent;
    //一些不变的基础数据
    private MonsterInfo monsterInfo;

    private int hp;
    public bool isDead = false;

    private float frontTime;
    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        //状态机加载
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        hp = info.hp;
        //速度初始化
        //速度和加速度相同，这样就一下子就是匀速
        agent.speed = agent.acceleration =  info.moveSpeed ;
        agent.angularSpeed = info.roundSpeed;
    }
    //受伤
    public void Wound(int dmg)
    {
        hp -= dmg;
        animator.SetTrigger("Wound");
        if (hp < 0)
        {
            //死亡逻辑
            Dead();
        }
        else
        {
            GameDataMgr.Instance.PlaySound("Music/Wound");
            
        }
    }
    //死亡
    public void Dead()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetBool ("Dead",true);
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/dead");

        GameLeveMgr.Instance.player.addMoney(100);
    }
    public void AddMoney(int money)
    {
        
    }
    public void BornOver()
    {
        agent.SetDestination(MainTowerObject.Instance.transform.position);
    }

    public void DeadEvent()
    {
        //死亡动画播完后移除对象
        //之后有了关卡管理器再来处理
        GameLeveMgr.Instance.RemoveMonster(this);
        Destroy(this.gameObject);

        //怪物死亡时，检测游戏是否胜利
        if (GameLeveMgr.Instance.CheckOver())
        {
            //显示结束界面
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLeveMgr.Instance.player.money,true);
        }

    }


    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }
        //根据速度来决定动画播放什么
        animator.SetBool("Run", agent.velocity != Vector3.zero);
        //检测和目标点达到移动条件的时候就攻击
        if (Vector3.Distance(this.transform.position,MainTowerObject.Instance.transform.position)<5
            && Time.time - frontTime > monsterInfo.atkOffset)
        {
            //记录这次攻击的时间
            frontTime = Time.time;
            animator.SetTrigger("Atk");
        }
    }

    //伤害检测
    public void AtkEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position +this.transform.up +this.transform.forward, 1, 1<<LayerMask.NameToLayer("MainTower"));

        GameDataMgr.Instance.PlaySound("Music/Eat");

        for (int i = 0; i < colliders.Length; i++)
        {
            if(MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            {
                print("打中了塔，伤害"+ monsterInfo.atk);
                MainTowerObject.Instance.Wound(monsterInfo.atk);

            }
        }
    
    
    }
}
