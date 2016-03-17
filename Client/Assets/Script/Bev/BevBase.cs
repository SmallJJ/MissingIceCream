using System;
using DG.Tweening;

public abstract class BevBase:ComponentBase,IBev
{
    /// <summary>
    /// 状态机
    /// </summary>
    public StateMachine StateMachine { get; private set; }

    /// <summary>
    /// 是否死亡
    /// </summary>
    public bool IsDead { get; private set; }

    #region IBevInterface methods
    public void Idle()
    {
        throw new NotImplementedException();
    }

    public void Move()
    {
        throw new NotImplementedException();
    }

    public bool IsArrived()
    {
        throw new NotImplementedException();
    }

    public bool Dead()
    {
        throw new NotImplementedException();
    }

    public void Hurt(int damage)
    {
        throw new NotImplementedException();
    }

    public bool HasFood()
    {
        throw new NotImplementedException();
    }
    public bool HasPosTarget()
    {
        throw new NotImplementedException();
    }

    public void Eat()
    {
        throw new NotImplementedException();
    }
    #endregion

}
