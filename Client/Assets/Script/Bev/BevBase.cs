using System;
using DG.Tweening;

public abstract class BevBase:ComponentBase,IBev
{
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
    #endregion
}
