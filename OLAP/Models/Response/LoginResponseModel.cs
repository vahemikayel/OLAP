using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace OLAP.API.Models.Response
{
    public class LoginResponseModel
    {
        public string Message { get; set; }

        [JsonIgnore]
        public SignInResult SignInResult { get; private set; }

        [JsonIgnore]
        public AccessTokenResponseModel TokenResponse { get; private set; }

        public bool TeamDoesNotSpecified { get; internal set; }

        public bool IncorrectTeam { get; internal set; }

        public bool Succeeded { get; set; }

        public LoginResponseModel()
        { }

        public LoginResponseModel(SignInResult signInResult)
        {
            SignInResult = signInResult;
            Succeeded = signInResult.Succeeded;
        }

        public LoginResponseModel(SignInResult signInResult, AccessTokenResponseModel tokenResponse)
        {
            SignInResult = signInResult;
            TokenResponse = tokenResponse;
            Succeeded = signInResult.Succeeded;
        }

        public LoginResponseModel(IdentityResult changeRes)
        {
            Succeeded = changeRes.Succeeded;
            Message = string.Join(Environment.NewLine, changeRes.Errors.Select(x => $"{x.Code} {x.Description}"));
        }
    }
}
