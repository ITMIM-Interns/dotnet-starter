namespace Identity.DTO.Accounts
{
    public sealed class JwtSetting
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpireAt { get; set; }
    }
}
