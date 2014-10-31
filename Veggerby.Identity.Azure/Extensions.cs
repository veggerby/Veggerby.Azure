using System;
using System.Collections.Generic;
using System.Text;

namespace Veggerby.Identity.Azure
{
    public static class Extensions
    {

        public static string ToBase64UserId(this string obj)
        {
            var userInfo = obj.Split('@');
            return string.Join("@", userInfo.Base64Encode());
        }

        public static string ToBase64(this string obj)
        {
            return string.Join("", Base64Encode(new[] { obj }));
        }

        public static string FromBase64(this string obj)
        {
            return Base64Decode(obj);
        }

        private static string Base64Decode(this string obj)
        {
            try
            {
                var decodedBytes = Convert.FromBase64String(obj.Replace('_', '/').Replace('.', '='));
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        private static IEnumerable<string> Base64Encode(this IEnumerable<string> objectArray)
        {
            foreach (var obj in objectArray)
            {
                var userIdBytes = Encoding.UTF8.GetBytes(obj);
                var userToken = Convert.ToBase64String(userIdBytes);
                yield return userToken
                    .Replace('/', '_')
                    .Replace('=', '.');
            }
        }
    }
}
