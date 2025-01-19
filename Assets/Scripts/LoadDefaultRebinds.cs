using UnityEngine;

public class LoadDefaultRebinds : MonoBehaviour
{
    private JsondataService dataService = new JsondataService();
    
    public UserRebinds LoadRebindOfType(RebindType Type)
    {
        string path;
        switch (Type)
        {
            case RebindType.Apex:
                path = "DefaultRebinds/ApexDefaultRebinds"; break;
            default:
                path = "DefaultRebinds/ApexDefaultRebinds"; break;
        }

        TextAsset stringData = Resources.Load(path) as TextAsset;

        dataService.ReadDataFromText<UserRebinds>(stringData.text, out UserRebinds rebinds);

        return rebinds;
    }
}
