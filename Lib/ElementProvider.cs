using System.Threading.Tasks;

namespace Lib
{
    public interface IElement { }
    public class Element : IElement { }

    public interface IElementProvider : IProvider<IElement>
    {
        Task<IElement> GetElement(string url = null, bool alwaysReload = false);
    }

    public class ElementProvider : IElementProvider
    {
        public IElement Value
        {
            get { return _provider.Value; }
            set { _provider.Value = value; }
        }
        private readonly IProvider<IElement> _provider;

        public async Task<IElement> GetElement(string url = null, bool alwaysReload = false)
        {
            await Task.Yield();
            return Value;
        }

        public ElementProvider(IProvider<IElement> provider)
        {
            _provider = provider;
        }
    }

    public class WebElementProvider : IElementProvider
    {
        public IElement Value { get; set; }

        public async Task<IElement> GetElement(string url = null, bool alwaysReload = false)
        {
            return null;
        }

        public WebElementProvider()
        {
        }
    }
}
