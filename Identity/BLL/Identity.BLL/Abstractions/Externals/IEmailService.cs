namespace Identity.BLL.Abstractions.Externals
{
    public interface IEmailService
    {
        Task SendAsync(string to, string body,string subject);
    }
}
