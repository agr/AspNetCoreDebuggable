// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Internal
{
    /// <summary>
    /// A Stream that wraps another stream starting at a certain offset and reading for the given length.
    /// </summary>
    internal class ReferenceReadStream : Stream
    {
        private readonly Stream _inner;
        private readonly long _innerOffset;
        private readonly long _length;
        private long _position;

        private bool _disposed;

        public ReferenceReadStream(Stream inner, long offset, long length)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            _inner = inner;
            _innerOffset = offset;
            _length = length;
            _inner.Position = offset;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return _inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return _length; }
        }

        public override long Position
        {
            get { return _position; }
            set
            {
                ThrowIfDisposed();
                if (value < 0 || value > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "The Position must be within the length of the Stream: " + Length.ToString());
                }
                VerifyPosition();
                _position = value;
                _inner.Position = _innerOffset + _position;
            }
        }

        // Throws if the position in the underlying stream has changed without our knowledge, indicating someone else is trying
        // to use the stream at the same time which could lead to data corruption.
        private void VerifyPosition()
        {
            if (_inner.Position != _innerOffset + _position)
            {
                throw new InvalidOperationException("The inner stream position has changed unexpectedly.");
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                Position = offset;
            }
            else if (origin == SeekOrigin.End)
            {
                Position = Length + offset;
            }
            else // if (origin == SeekOrigin.Current)
            {
                Position = Position + offset;
            }
            return Position;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            VerifyPosition();
            var toRead = Math.Min(count, _length - _position);
            var read = _inner.Read(buffer, offset, (int)toRead);
            _position += read;
            return read;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            VerifyPosition();
            var toRead = Math.Min(count, _length - _position);
            var read = await _inner.ReadAsync(buffer, offset, (int)toRead, cancellationToken);
            _position += read;
            return read;
        }
#if NET451
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed();
            VerifyPosition();
            var tcs = new TaskCompletionSource<int>(state);
            BeginRead(buffer, offset, count, callback, tcs);
            return tcs.Task;
        }

        private async void BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, TaskCompletionSource<int> tcs)
        {
            try
            {
                var read = await ReadAsync(buffer, offset, count);
                tcs.TrySetResult(read);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            if (callback != null)
            {
                // Offload callbacks to avoid stack dives on sync completions.
                var ignored = Task.Run(() =>
                {
                    try
                    {
                        callback(tcs.Task);
                    }
                    catch (Exception)
                    {
                        // Suppress exceptions on background threads.
                    }
                });
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException(nameof(asyncResult));
            }

            var task = (Task<int>)asyncResult;
            return task.GetAwaiter().GetResult();
        }
#endif
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
#if NET451
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }
#endif
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ReferenceReadStream));
            }
        }
    }
}
