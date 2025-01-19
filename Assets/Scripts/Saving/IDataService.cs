public interface IDataService
{
    bool SaveData<T>(string path, T data, bool encrypted);

    bool LoadData<T>(string path, bool encrypted, out T data);
}
