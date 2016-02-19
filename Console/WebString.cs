namespace Lib
{
    public interface IWebString
    {
        IElement Element { get; }
    }

    public class WebString : IWebString
    {
        public IElement Element { get; }
        private string _url;
        public WebString(IElementProvider elementProvider, string url)
        {
            Element = elementProvider.Value;
            _url = url;
        }
    }
}
