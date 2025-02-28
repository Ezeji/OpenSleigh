﻿using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using OpenSleigh.Core;
using OpenSleigh.Core.Messaging;
using OpenSleigh.Core.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSleigh.Transport.AzureServiceBus
{
    internal class ServiceBusPublisher : IPublisher
    {
        private readonly IServiceBusSenderFactory _senderFactory;
        private readonly ITransportSerializer _serializer;
        private readonly ILogger<ServiceBusPublisher> _logger;
        private readonly ISystemInfo _systemInfo;

        public ServiceBusPublisher(IServiceBusSenderFactory senderFactory,
            ITransportSerializer serializer,
            ILogger<ServiceBusPublisher> logger, 
            ISystemInfo systemInfo)
        {
            _senderFactory = senderFactory ?? throw new ArgumentNullException(nameof(senderFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _systemInfo = systemInfo ?? throw new ArgumentNullException(nameof(systemInfo));
        }

        public Task PublishAsync(IMessage message, CancellationToken cancellationToken = default)
        {
            if (message == null) 
                throw new ArgumentNullException(nameof(message));

            return PublishAsyncCore(message, cancellationToken);
        }

        private async Task PublishAsyncCore(IMessage message, CancellationToken cancellationToken)
        {
            ServiceBusSender sender = _senderFactory.Create((dynamic) message);
            _logger.LogInformation($"client '{_systemInfo.ClientId}' publishing message '{message.Id}' to {sender.FullyQualifiedNamespace}/{sender.EntityPath}");

            var serializedMessage = _serializer.Serialize(message);
            var busMessage = new ServiceBusMessage(serializedMessage)
            {
                CorrelationId = message.CorrelationId.ToString(),
                MessageId = message.Id.ToString(),
                ApplicationProperties =
                {
                    {HeaderNames.MessageType, message.GetType().FullName}
                }
            };

            await sender.SendMessageAsync(busMessage, cancellationToken).ConfigureAwait(false);
        }
    }
}
