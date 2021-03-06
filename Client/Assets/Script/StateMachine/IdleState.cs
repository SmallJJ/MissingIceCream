﻿public class IdleState:StateBase
{
    public IdleState(BevBase bev) : base(bev) 
    {
    }
    #region override methods
    public override void Execute()
    {
        if (base.frameCounter >= base.intervalFrame)
        {
            base.frameCounter = 0;
            if (base.bev.HasPosTarget())
            {
                this.bev.StateMachine.ChangeState(BevStateType.Move);
            }
            else
            {
                this.bev.Idle();
            }
        }
        else
        {
            base.frameCounter++;
        }
    }
    #endregion
}
