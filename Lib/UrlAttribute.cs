using System;

namespace Lib
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, Inherited = true)]
    public class UrlAttribute : Attribute
    {
        public string Url;

        public UrlAttribute(string url)
        {
            Url = url;
        }
    }
}