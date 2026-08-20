using System;
using System.Security.Cryptography;
using System.Text;


namespace ReporteGasolina.Utilities
{
    public static class DpapiHelper
    {
        public static string Protect(string plain)
        {
            if (plain == null) return null;
            var bytes = Encoding.UTF8.GetBytes(plain);
            var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        public static string Unprotect(string protectedBase64)
        {
            if (string.IsNullOrWhiteSpace(protectedBase64)) return null;
            var protectedBytes = Convert.FromBase64String(protectedBase64);
            var bytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
