using System.Collections.Generic;

public class SceneDataContainer
{
    public static SceneDataContainer Instance { get; private set; }

    private Dictionary<string, object> _data;

    static SceneDataContainer()
    {
        Instance = new SceneDataContainer();
    }

    private SceneDataContainer()
    {
        _data = new Dictionary<string, object>();
    }

    public void SetData(string key, object data)
    {
        _data[key] = data;
    }

    public bool TryAddData(string key, object data)
    {
        if (_data.ContainsKey(key))
            return false;

        _data.Add(key, data);
        return true;
    }

    public bool TryGetData<T>(string key, out T data)
    {
        data = default;
        if (_data.TryGetValue(key, out object value) && value is T v)
        {
            data = v;
            return true;
        }
        return false;
    }
}
