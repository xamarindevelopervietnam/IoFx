﻿using System.Net.Sockets;
using System.Threading;

namespace System.IoFx.Sockets
{
    class SocketSender : IObserver<ArraySegment<byte>>, IDisposable
    {
        private int _disposed;
        private readonly Socket _socket;

        public SocketSender(Socket socket)
        {
            _socket = socket;
        }
        public void OnCompleted()
        {
            Dispose(true);
        }

        public void OnError(Exception error)
        {
            Dispose(false);
        }

        public void OnNext(ArraySegment<byte> value)
        {
            try
            {
                _socket.Send(value.Array, value.Offset, value.Count, SocketFlags.None);
            }
            catch (Exception)
            {
                Dispose(false);
                throw;
            }
        }

        void Dispose(bool shutdown)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                if (shutdown && _socket.Connected)
                {
                    _socket.Shutdown(SocketShutdown.Send);
                }

                _socket.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}