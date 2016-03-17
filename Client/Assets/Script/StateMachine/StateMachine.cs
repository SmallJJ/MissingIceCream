using System.Linq;
using System.Collections.Generic;
public class StateMachine
{
    private Dictionary<BevStateType, IState> m_StateDic;
    private IState m_CurState;
    private BevBase m_CurBev;

    public StateMachine(BevBase bev)
    {
        this.m_StateDic = new Dictionary<BevStateType, IState>();
        this.m_CurBev = bev;
        this.SetCurState(BevStateType.Idle);
    }

    #region public methods

    /// <summary>
    /// 设置当前状态
    /// </summary>
    /// <param name="type"></param>
    public void SetCurState(BevStateType type)
    {
        this.m_CurState = this.GetStateByType(type);
        this.m_CurState.Enter();
    }

    /// <summary>
    /// 通过类型获取状态
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public IState GetStateByType(BevStateType type)
    {
        if(this.m_StateDic.ContainsKey(type))
        {
            return this.m_StateDic[type];
        }
        IState state=null;
        switch (type)
        {
            case BevStateType.Idle:
                state=new IdleState(this.m_CurBev);
                break;
            case BevStateType.Move:
                state = new MoveState(this.m_CurBev);
                break;
            case BevStateType.Eat:
                state = new EatState(this.m_CurBev);
                break;
            case BevStateType.Dead:
                state = new EatState(this.m_CurBev);
                break;
        }
        if(state!=null)
        {
            this.m_StateDic.Add(type,state);
        }
        return state;
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="type"></param>
    public void ChangeState(BevStateType type)
    {
        if (this.m_CurState != null)
        {
            this.m_CurState.Exit();
        }
        this.SetCurState(type);
    }

    /// <summary>
    /// 获取当前状态类型
    /// </summary>
    /// <returns></returns>
    public BevStateType GetCurStateType()
    {
        return this.m_StateDic.FirstOrDefault(a => a.Value == this.m_CurState).Key;
    }

    #endregion
}
