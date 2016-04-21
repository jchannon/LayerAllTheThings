using System;
using MediatR;

namespace MultiDbSupportWithConventions
{
    public abstract class AbstractNpgsqlPagingRequestHandler<T,U> : IRequestHandler<T, U> where T : IRequest<U>
    {
        public abstract U Handle(T message);

        public string SQL { get; set; }

        public string PagingSQL
        { 
            get
            { 
                return string.Concat("select count(items.id) over() as totalitemcount, items.* from (", this.SQL, ") as items limit @Limit offset @Offset");
            }
        }
    }
}