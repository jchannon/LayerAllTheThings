using System;
using Nancy;
using Nancy.ModelBinding;

namespace QueryHandler
{
    public static class NancyExtensions
    {
        public static Envelop<T> BindCommandEnvolope<T>(this NancyModule module, Guid commandId)
        {
            return new Envelop<T>(commandId, module.Bind<T>());
        }
    }
}
