namespace OLAP.API.Models.Response.Error
{
    public class ErrorResponseModel<TData> : ErrorResponseModel
    {
        public TData Value { get; set; }

        public ErrorResponseModel() : base() { }
        public ErrorResponseModel(TData value, EResultMessageType type)
        {
            MessageType = type;
            if (value != null &&
                type == EResultMessageType.Success)
            {
                IsOk = true;
                Value = value;
            }

            if (value.ToString() == "" &&
               type != EResultMessageType.Success)
                Value = value;
        }
    }
    public class ErrorResponseModel
    {
        public bool IsOk { get; set; }
        public EResultMessageType MessageType { get; set; }
        public ErrorResponseModel() { }
    }
}
