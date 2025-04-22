namespace PharmaHub.Presentation.ActionRequest.Account
{
    public class VerifyCodeRequest
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }

    }
}
