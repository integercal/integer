using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Acesso.i18n;
using Integer.Infrastructure.I18n;

namespace Integer.Domain.Acesso
{
    public class AcessoResourceWrapper
    {
        public static string InvalidLogin 
        { 
            get 
            {
                return AcessoResource.InvalidLogin;
            } 
        }

        public static string ExistingUserWithEmail
        {
            get
            {
                return AcessoResource.ExistingUserWithEmail;
            }
        }

        public static string UserNotFound
        {
            get
            {
                return AcessoResource.UserNotFound;
            }
        }
    }
}
