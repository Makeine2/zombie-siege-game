# zombie-siege-demo

一个基于 Unity 制作的第三人称丧尸塔防战斗 Demo，用来练习客户端常见模块的拆分与实现，包括 UI 管理、JSON 配置、角色与怪物逻辑、基础 AI、动画复用和场景流程控制。

## 项目亮点

- `UI 管理`
  - 使用 `UIManager + BasePanel` 管理界面生命周期
  - 面板支持统一显示、隐藏和淡入淡出
- `数据配置与持久化`
  - 使用 JSON 管理角色、怪物、场景、塔等配置
  - 玩家数据和音量设置支持本地持久化
- `角色与怪物逻辑`
  - 玩家支持移动、翻滚、近战/远程攻击
  - 怪物使用 `NavMeshAgent` 完成寻路、追击和攻击主塔
- `动画系统`
  - 使用 Animator 状态机管理角色与怪物动作
  - 不同怪物通过 `AnimatorOverrideController` 复用基础状态机
- `关卡流程`
  - 出怪点按波次刷怪
  - 关卡管理器统一管理胜负判定、怪物列表和波次信息

## 目录说明

- `Assets/Scripts/UI`
  - 通用 UI 管理器和面板基类
- `Assets/Scripts/Data`
  - 角色、怪物、场景、玩家等数据结构与数据管理
- `Assets/Scripts/Json`
  - JSON 序列化与反序列化封装
- `Assets/Scripts/BeginScene`
  - 开始界面、选角和选关逻辑
- `Assets/Scripts/GameScene`
  - 玩家、怪物、塔、主塔和关卡流程逻辑

## 核心实现思路

### 1. UI 管理

项目中使用单例 `UIManager` 统一管理面板实例。通过 `Dictionary<string, BasePanel>` 缓存当前显示的面板，避免各个界面直接互相耦合。`BasePanel` 抽离了通用的初始化和淡入淡出逻辑，子类只需要关注自己的按钮监听和界面刷新。

### 2. 数据驱动

角色、怪物、场景和塔等配置数据放在 `StreamingAssets` 中，运行时通过 `JsonMgr` 读取。玩家的存档数据则写入 `persistentDataPath`，形成“默认配置 + 本地持久化”的结构，便于调试和扩展。

### 3. 怪物 AI

怪物系统采用“数据驱动 + 统一逻辑脚本”的方式。`MonsterInfo` 决定怪物的预制体、动画控制器、攻击力、速度、转向速度和攻击间隔；`MonsterObject` 负责运行时行为，包括寻路、攻击判断、受伤与死亡；`GameLeveMgr` 则负责记录场上怪物、出怪点和胜负判定。

### 4. 动画复用

怪物没有为每种外形重做一整套状态机，而是以基础怪物控制器为骨架，使用 `AnimatorOverrideController` 替换不同怪物的动作片段。这样能够在保持逻辑结构一致的前提下，用更低成本支持不同形态的怪物表现。


## 运行环境

- Unity 2022 系列
- Windows

## 说明

该项目主要用于个人学习与面试展示，代码结构和实现方式以练习客户端基础能力为目标。
