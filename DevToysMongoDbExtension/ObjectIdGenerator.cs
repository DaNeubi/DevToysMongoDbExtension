using System.Security.Cryptography;
using System.Text;

public static class ObjectIdGenerator
{
    private static readonly object IncrementLock = new ();
    private static int _increment = new Random().Next();
    private static readonly byte[] ProcessUnique = GenerateProcessUnique();

    public static string Generate(int? timestamp = null)
    {
        var time = timestamp ?? (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        var inc = GetIncrement();
        var buffer = new byte[12];

        // 4-byte timestamp
        buffer[0] = (byte)(time >> 24);
        buffer[1] = (byte)(time >> 16);
        buffer[2] = (byte)(time >> 8);
        buffer[3] = (byte)time;

        // 5-byte process unique
        Buffer.BlockCopy(ProcessUnique, 0, buffer, 4, 5);

        // 3-byte counter
        buffer[9] = (byte)(inc >> 16);
        buffer[10] = (byte)(inc >> 8);
        buffer[11] = (byte)inc;

        return ByteArrayToHexString(buffer);
    }

    private static int GetIncrement()
    {
        lock (IncrementLock)
        {
            return _increment++;
        }
    }

    private static byte[] GenerateProcessUnique()
    {
        using var rng = RandomNumberGenerator.Create();
        var processUnique = new byte[5];
        rng.GetBytes(processUnique);
        return processUnique;
    }
    
    private static string ByteArrayToHexString(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}