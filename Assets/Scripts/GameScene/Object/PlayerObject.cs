using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private int atk;
    public int money;

    private float roundSpeed = 155;
    private Animator animator;
    public Transform gunPoint;

    //打击特效
    public string hitEff;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
    }
    // Update is called once per frame
    void Update()
    {
        //移动变化，动作变化
        animator.SetFloat("Vspeed", Input.GetAxis("Vertical"));
        animator.SetFloat("Hspeed", Input.GetAxis("Horizontal"));

        this.transform.Rotate(Vector3.up,Input.GetAxis("Mouse X") * roundSpeed* Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1,1);//层级1，权重设为1
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift)){
            animator.SetLayerWeight(1,0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Roll");
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }
    }//update

    public void KnifeEvent()//刀
    {
        //进行伤害检测，刀用的是范围检测
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward +this.transform.up, 1 ,1 << LayerMask.NameToLayer("Monster"));
        //位置this.transform是角色脚底，向前，向上生成sphere范围 
        //半径是1
        //再弄层级，只打击怪物层

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Knife");

        for (int i = 0; i < colliders.Length; i++)
        {
            //得到碰撞上的怪物脚本
            MonsterObject monster  = colliders[i].gameObject.GetComponent<MonsterObject>();
            if(monster!=null)
            {
                Debug.Log("打到了");
                monster.Wound(this.atk);
                break;
            }

        }
       
    }
    public void ShootEvent()//枪
    {
        //进行射线检测
        //前提是要有开火点 开火点的forward是z轴
        RaycastHit[] hits = Physics.RaycastAll(gunPoint.position,gunPoint.forward,1<< LayerMask.NameToLayer("Monster"));
        //音效
        GameDataMgr.Instance.PlaySound("Music/Gun");
        //打击特效



        for (int i = 0; i < hits.Length; i++)
        {
            //得到怪物脚本，让其受伤
            MonsterObject monster =hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null)
            {
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRol.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);
                
                monster.Wound(this.atk);
                break;
            }
        }   

    }

    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    public void addMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
