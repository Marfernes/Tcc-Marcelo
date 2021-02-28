using System;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SCRH.Utils
{
    public class Account
    {
        public static Guid UsuarioId => Guid.Parse(GetByType(ClaimTypes.NameIdentifier));
        public static string NomeCompleto => GetByType(ClaimTypes.Name);
        public static string Roles => GetByType(ClaimTypes.Role);

        public static string GetByType(string type)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var byType = identity.Claims.FirstOrDefault(c => c.Type == type);
            if (byType != null)
                return identity == null ? string.Empty : byType.Value;
            return string.Empty;
        }
    }
}