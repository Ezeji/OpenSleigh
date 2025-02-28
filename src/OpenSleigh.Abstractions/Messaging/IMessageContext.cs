﻿namespace OpenSleigh.Core.Messaging
{
    public interface IMessageContext<out TM> where TM : IMessage
    {
        TM Message { get; }
        ISystemInfo SystemInfo { get; }
    }
}