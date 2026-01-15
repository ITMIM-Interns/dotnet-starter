namespace MiniApp.BLL.Exceptions.Commons
{
    public static class ExceptionMessage
    {
        public const string UserNotFoundMessage = "User not found";
        public const string UserEmailNotFoundMessage = "Email not found";
        public const string ExistUserEmailMessage = "The email is exist";
        public const string ExistUsernameMessage = "The username is exist";
        public const string InvalidVerificationCodeMessage = "The verification code is wrong";
        public const string InvalidExpiresTimeMessage = "The verification code has expired. Please send a new code.";
        public const string VerificationNotFoundMessage = "Verification not found";
        public const string VerificationConfirmedMessage = "Verification already confirmed";


    }
}
