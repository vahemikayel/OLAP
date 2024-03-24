namespace OLAP.API.Models
{
    public enum EResultMessageType : int
    {
        Unknown = 0,
        Success = 1,
        CatchException,
        IsNotPasswordAvailable,
        IsNotAuthorized,
        InvalidContent,
        SqlConnectionFailed


    }
}
