namespace Services
{
    public interface ISaveLoadService
    {
        void Save<T>(string key, T data);
        T Load<T>(string key);
        bool HasKey(string key);
        void DeleteKey(string key);
    }
}