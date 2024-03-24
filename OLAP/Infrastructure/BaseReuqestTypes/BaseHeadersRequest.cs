namespace OLAP.API.Infrastructure.BaseReuqestTypes
{
    public class BaseHeadersRequest
    {
        public Guid _TeamId { get; private set; }

        public Guid _UserId { get; private set; }

        public Guid _RequestId { get; private set; }

        public void SetRequestId(Guid requestId) => _RequestId = requestId;

        public void SetTeam(Guid team) => _TeamId = team;

        internal void SetUserId(Guid userId)
        {
            _UserId = userId;
        }
    }
}
