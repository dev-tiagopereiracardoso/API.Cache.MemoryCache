namespace API.Cache.MemoryCache.Domain.Implementation.Interfaces
{
    public interface IMemoryService
    {
        string Get(string key);

        bool Set(string key, string value);

        void Delete();
    }
}