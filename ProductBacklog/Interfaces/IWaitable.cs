﻿namespace ProductBacklog.Interfaces
{
    public interface IWaitable
    {
        bool IsBusy { get; }

        string WaitMessage { get; }
    }
}
