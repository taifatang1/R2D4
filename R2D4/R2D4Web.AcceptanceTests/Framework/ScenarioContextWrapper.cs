using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace R2D4Web.AcceptanceTests.Framework
{
    public class ScenarioContextWrapper
    {
        public static T Get<T>(string name) where T : class
        {
            return ScenarioContext.Current[name] as T;
        }

        public static void Set<T>(string name, T @object)
        {
            ScenarioContext.Current.Set<T>(@object, name);
        }
    }
}
