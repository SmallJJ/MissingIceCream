public interface IBev
{
    /// <summary>
    /// 休闲
    /// </summary>
    void Idle();

    /// <summary>
    /// 移动
    /// </summary>
    void Move();

    /// <summary>
    /// 是否到达
    /// </summary>
    bool IsArrived();

    /// </summary>
    /// 死亡
    /// </summary>
    bool Dead();

    /// <summary>
    /// 受伤
    /// </summary>
    void Hurt(int damage);
}
