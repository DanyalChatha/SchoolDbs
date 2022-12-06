using System;

namespace SchoolDb.Controllers
{
    internal class EnableCorsAttribute : Attribute
    {
        public EnableCorsAttribute(string origins, string methods, string headers)
        {
            Origins = origins;
            Methods = methods;
            Headers = headers;
        }

        public string Origins { get; }
        public string Methods { get; }
        public string Headers { get; }
    }
}