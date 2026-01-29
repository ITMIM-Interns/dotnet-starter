namespace Identity.BLL.Exceptions.Commons
{
    public static class ExceptionMessage
    {
        public const string UserNotFoundMessage = "User not found";
        public const string UserEmailNotFoundMessage = "Email not found";
        public const string ExistUserEmailMessage = "The email is exist";
        public const string ExistUsernameMessage = "The username is exist";
        public const string InvalidVerificationCodeMessage = "The verification code is wrong";
        public const string VerificationNotFoundMessage = "Verification not found";
        public const string AccountNotActiveMessage = "Account not active ";
        public const string ValidVerificationCode = "Your previous verification code is still valid. Please check your email or wait 2 minute for send new verification code";
        public const string InvalidLoginMessage = "Email or Password is wrong";

    }
}
