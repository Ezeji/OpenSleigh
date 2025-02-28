﻿using System;

namespace OpenSleigh.Core.Messaging
{
    internal class DefaultMessageContext<TM> : IMessageContext<TM>
        where TM : IMessage
    {
        public DefaultMessageContext(TM message, ISystemInfo systemInfo)
        {
            SystemInfo = systemInfo ?? throw new ArgumentNullException(nameof(systemInfo));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public TM Message { get; }
        public ISystemInfo SystemInfo { get; }        
    }
}