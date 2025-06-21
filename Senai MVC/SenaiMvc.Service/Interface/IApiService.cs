namespace SenaiMvc.Service.Interface
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<bool> DeleteAsync(string endpoint);
        Task<List<T>> PegarEstados<T>();
    }
}
