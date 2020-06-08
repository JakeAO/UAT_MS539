﻿namespace UAT_MS539.Core.Code.StateMachine
{
    public interface IState
    {
        string LocationLocId { get; }
        string TimeLocId { get; }

        void PerformSetup(Context context, IState previousState);
        void PerformContent(Context context);
        void PerformTeardown(Context context, IState nextState);
    }
}