using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var container = new Container();
            container.Options.RegisterParameterConvention(new UrlConvention());

            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
            container.Register(typeof(IProvider<>), typeof(Provider<>), Lifestyle.Scoped);
            container.Register<IWebString, WebString>();


            container.RegisterConditional<IElementProvider, WebElementProvider>(c => container.GetCurrentExecutionContextScope() == null);
            container.RegisterConditional<IElementProvider, ElementProvider>(Lifestyle.Scoped,
                c => container.GetCurrentExecutionContextScope() != null);
            //container.RegisterFunc<IElement, InboxMessage>();
            //container.Register<ILoginService,SALogin>(Lifestyle.Scoped);

            container.Verify();

            using (container.BeginExecutionContextScope())
            {
                var provider = container.GetInstance<IElementProvider>();
                provider.Value = new Element();
                var webString = container.GetInstance<IWebString>();
                var element = webString.Element;
            }
        }
    }
}
