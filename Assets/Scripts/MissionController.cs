using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public static MissionController Instance { get; private set; }

    public string[] CoordsStr { get; private set; }
    public List<GeoCoord> Coords { get; private set; }

    [SerializeField, Min(1)] private DataFile _file = DataFile.Part;
    [SerializeField, Min(1)] private int _count = 100;

    private const string JSON_PART = "Cities";
    private const string JSON_ALL = "CitiesAll";
    private const string JSON_MY = "MyCities";

    private void Awake()
    {
        Instance = this;

        string fileName = _file switch
		{
            DataFile.All => JSON_ALL,
            DataFile.MyFile => JSON_MY,
            _ => JSON_PART
        };
        string json = Resources.Load<TextAsset>(fileName).text;
        CoordsStr = json.Split("\n");
        Coords = new List<GeoCoord>
        (
            Enumerable.Range(0, _count)
            .Select(x => Random.Range(0, CoordsStr.Length))
            .ToHashSet()
            .Select(x => JsonUtility.FromJson<GeoCoord>(CoordsStr[x]))
            .Where(x => x != null)
        );
    }

    public GeoCoord GetRandomCity()
    {
        int index = Random.Range(0, CoordsStr.Length);
        return JsonUtility.FromJson<GeoCoord>(CoordsStr[index]);
    }

    public GeoCoord GetRandomFromConverted()
    {
        return Coords[Random.Range(0, Coords.Count)];
    }

    public enum DataFile
    {
        MyFile,
        All,
        Part
    }
}
