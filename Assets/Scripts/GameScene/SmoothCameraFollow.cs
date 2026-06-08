using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("目标设置")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0);
    public float distance = 5.0f;

    [Header("视角控制")]
    [Tooltip("上下观看的固定角度")]
    public float fixedPitch = 15f;
    [Tooltip("旋转跟随的平滑速度（值越小越快）")]
    public float rotationSmoothTime = 0.15f;
    [Tooltip("移动跟随的平滑速度")]
    public float followSmoothTime = 0.1f;

    // 内部平滑变量
    private float currentYaw;
    private float yawVelocity;

    private Vector3 currentTargetPos;
    private Vector3 posVelocity;

    void Start()
    {
        if (target != null)
        {
            currentYaw = target.eulerAngles.y;
            currentTargetPos = target.position + targetOffset;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. 获取目标当前的朝向角度
        float targetYaw = target.eulerAngles.y;

        // 2. 平滑处理旋转角度 (使用 SmoothDampAngle 专门处理 0-360 度循环)
        currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, rotationSmoothTime);

        // 3. 计算旋转并应用 (保持固定的俯仰角 fixedPitch)
        Quaternion rotation = Quaternion.Euler(fixedPitch, currentYaw, 0);
        transform.rotation = rotation;

        // 4. 平滑处理目标位置
        Vector3 desiredTargetPos = target.position + targetOffset;//这里是为了看人物的身子，target.position是人物的脚，这里加了偏移就是往上移
        currentTargetPos = Vector3.SmoothDamp(currentTargetPos, desiredTargetPos, ref posVelocity, followSmoothTime);

        // 5. 计算并应用最终位置：目标位置 - (当前旋转的正前方 * 距离)
        transform.position = currentTargetPos- (rotation * Vector3.forward * distance);
    }
    public void SetTarget(Transform player)
    {
        target = player;
    }

}