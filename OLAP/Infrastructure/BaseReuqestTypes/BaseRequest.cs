using MediatR;
using Newtonsoft.Json;

namespace OLAP.API.Infrastructure.BaseReuqestTypes
{
    public abstract class BaseRequest<TResponse> : BaseHeadersRequest, IRequest<TResponse>
    {
        private bool _usesTransaction;
        //[BsonIgnore]
        [JsonIgnore]
        public bool UsesTransaction
        {
            get { return _usesTransaction; }
            private set
            {
                _usesTransaction = value;
                if (_usesTransaction)
                    IsDataEdit = true;
            }
        }

        [JsonIgnore]
        public bool UsesIdempotent { get; private set; }

        [JsonIgnore]
        public bool UsesConnection { get; private set; }

        [JsonIgnore]
        public bool IsDataEdit { get; protected set; }

        internal virtual void SetUsesTransaction(bool usesTransaction) => UsesTransaction = usesTransaction;

        internal void SetUsesConnection(bool usesConnection) => UsesConnection = usesConnection;


        public BaseRequest()
        {
            UsesIdempotent = true;
            UsesTransaction = true;
            UsesConnection = false;
            IsDataEdit = true;
        }

        public BaseRequest(bool usesIdempotent, bool usesTransaction, bool usesConnection, bool isDataEdit) : this()
        {
            UsesIdempotent = usesIdempotent;
            UsesTransaction = usesTransaction;
            UsesConnection = usesConnection;
            IsDataEdit = isDataEdit;
        }
    }
}
