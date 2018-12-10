using System.IO;

namespace Books.Utils
{
    public static class StreamExtensions
    {
        public static byte[] ReadBytes(this Stream stream)
        {
            if (stream is MemoryStream)
                return (stream as MemoryStream).ToArray();

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
