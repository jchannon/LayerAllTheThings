using System;
using Nancy;
using Nancy.ModelBinding;

namespace QueryHandler
{
    public static class NancyExtensions
    {
        public static Envelope<T> BindCommandEnvelope<T>(this NancyModule module, Guid commandId)
        {
            return new Envelope<T>(commandId, module.Bind<T>());
        }
    }
}
