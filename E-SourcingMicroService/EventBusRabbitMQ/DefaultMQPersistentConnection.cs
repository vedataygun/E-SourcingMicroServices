using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class DefaultMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<DefaultMQPersistentConnection> _logger;
        private bool _disposed;

        public DefaultMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount, ILogger<DefaultMQPersistentConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }

        public bool IsConnected {
            get
            {
                return _connection != null && _connection.IsOpen &&  !_disposed;

            }      
        }


        public bool TryConnect()
        {
            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                });


            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShowDown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;


                return true;
            }
            else
            {
                return false;
            }
        }


        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        { 
            if (_disposed) return;

            TryConnect();
        }

        private void OnConnectionShowDown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }

        public IModel CreateModel()
        {
            if(!IsConnected)
            {
                throw new Exception("1asd");
            }

            return _connection.CreateModel();
        }


        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch(IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
    }
}
