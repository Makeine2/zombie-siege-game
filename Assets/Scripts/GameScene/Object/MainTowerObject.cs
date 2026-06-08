using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private int hp =100;
    private int maxHp =100;

    private bool isDead = false;

    //弄成单例模式
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;
    private void Awake()
    {
        instance = this;
    }

    //自己收到伤害更新血量

    public void UpdateHp(int hp,int maxHP)//更新血量
    {
        this.hp = hp;
        this.maxHp = maxHP;

        //更新界面上的ui血量
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp,maxHp);
    }

    public void Wound(int dmg)
    {
        if (isDead)//保护区域已经被打死，就没必要更新
        {
            return;
        }
        hp -= dmg;
        if (hp < 0)
        {
            hp = 0;
            isDead = true;
            //游戏结束
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo((int)(GameLeveMgr.Instance.player.money *0.5f),false);

        }
        UpdateHp(hp, maxHp);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        instance = null;
    }
}
