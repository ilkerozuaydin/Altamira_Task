using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        Task<T> Get<T>(string key);

        Task<object> Get(string key);

        Task Add(string key, object data, int duration);

        Task<bool> IsAdded(string key);

        Task Remove(string key);

        Task RemoveByPattern(string pattern);
    }
}