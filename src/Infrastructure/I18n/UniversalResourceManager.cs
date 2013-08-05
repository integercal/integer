using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Integer.Infrastructure.I18n
{
    public class UniversalResourceManager
    {
        private readonly ResourceManager resourceManager;

        public UniversalResourceManager(string resource, Assembly assembly)
        {
            resourceManager = new System.Resources.ResourceManager(resource, assembly);
        }

        public string GetString(string key) 
        {
            return resourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
        }
    }
}
