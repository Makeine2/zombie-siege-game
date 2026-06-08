using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr 
{
   //单例
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //音乐数据
    public MusicData musicData;
    //所有角色数据
    public List<RoleInfo> roleInfoList;
    //玩家数据
    public PlayerData playerData;
    //记录选择的英雄角色
    public RoleInfo nowSelRol;
    //所有场景数据
    public List<SceneInfo> sceneInfoList;
    //所有的怪物数据
    public List<MonsterInfo> monsterInfoList;
    //所有塔的数据
    public List<TowerInfo> towerInfoList;

    private GameDataMgr()//拿取 音乐数据
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //玩家数据
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");

        //读取角色数据
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");//调试的话要附加到unity进程中，在visual studio里找

        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");

        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        //读取塔的数据
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");

    }

    /// <summary>
    /// 存储音效数据
    /// </summary>
    public void SaveMusicData()//存放 音乐数据
    {
        JsonMgr.Instance.SaveData(musicData,"MusicData");
    }
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
    /// <summary>
    /// 播放音效方法
    /// </summary>
    /// <param name="resName"></param>
    public void PlaySound(string resName)
    {
        GameObject musicObect = new GameObject();
        AudioSource a = musicObect.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.soundValue;
        a.mute = !musicData.soundOpen;
        a.Play();
        GameObject.Destroy(musicObect,0.5f);

    }


}
