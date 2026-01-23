namespace Identity.BLL.Abstractions.Externals
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan time);
        Task RemoveAsync(string key);
    }
}
