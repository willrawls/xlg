using System;
using System.IO;

namespace MetX.Standard.Library.Encryption;

public class NoiseStream : Stream
{
    private readonly long _length = long.MaxValue;
    private long _lengthLeft = long.MaxValue;

    public DateTime Until = DateTime.MinValue;

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Position { get; set; } = 0;
    public override long Length => _length;

    public NoiseStream(DateTime until)
    {
        Until = until;
    }

    public NoiseStream(long length)
    {
        _length = length;
        _lengthLeft = _length;
    }

    public override void Flush()
    {
        //SuperRandom.ResetProvider();
    }
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || count < 0 || offset + count > buffer.Length)
            throw new ArgumentOutOfRangeException();

        if (Until != DateTime.MinValue && DateTime.Now >= Until)
        {
            _lengthLeft = 0;
            return 0;
        }

        int bytesToRead = (int)Math.Min(count, _lengthLeft);
        if (bytesToRead <= 0)
        {
            _lengthLeft = 0;
            return 0;
        }

        var randomBytes = SuperRandom.NextBytes(bytesToRead);
        Buffer.BlockCopy(randomBytes, 0, buffer, offset, bytesToRead);

        Position += bytesToRead;
        _lengthLeft -= bytesToRead;

        return (int) _lengthLeft;
    }

    public static NoiseStream StreamForFixedLength(int length)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
        return new NoiseStream(length);
    }

    public static NoiseStream StreamForMinutes(int minutes)
    {
        if (minutes <= 0) throw new ArgumentOutOfRangeException(nameof(minutes));
        return new NoiseStream(DateTime.Now.AddMinutes(minutes));
    }

    public static NoiseStream StreamForSeconds(int seconds, int milliseconds)
    {
        if (milliseconds < 0) throw new ArgumentOutOfRangeException(nameof(milliseconds));
        return new NoiseStream(DateTime.Now.AddSeconds(seconds + (double) milliseconds / 1000));
    }

    public static NoiseStream StreamForTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds <= 0) throw new ArgumentOutOfRangeException(nameof(timeSpan));
        return new NoiseStream(DateTime.Now.Add(timeSpan));
    }
    
    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException("Seek is not supported in NoiseStream.");
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException("SetLength is not supported in NoiseStream.");
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException("Write is not supported in NoiseStream.");
    }
}