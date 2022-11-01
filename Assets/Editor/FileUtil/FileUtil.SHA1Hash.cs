using System.Text;

namespace TPM
{
    public static partial class FileUtil
    {
        private static string GetHashFromMemory(string text)
        {
            return GetHashFromMemory(Encoding.UTF8.GetBytes(text));
        }

        private static string GetHashFromMemory(byte[] data)
        {
            var sha1 = new System.Security.Cryptography.SHA1Managed();
            var hash = sha1.ComputeHash(data);
            return ToHexadecimalHash(hash);
        }

        private static string ToHexadecimalHash(byte[] hash, string hexadecimalFormat = "X2")
        {
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                sb.Append(b.ToString(hexadecimalFormat));
            }

            return sb.ToString();
        }
    }
}