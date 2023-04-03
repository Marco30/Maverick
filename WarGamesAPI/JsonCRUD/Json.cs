using Newtonsoft.Json;

namespace WarGamesAPIAPI.JsonCRUD;

public class Json
{
    static readonly string startupPath = Environment.CurrentDirectory;

    private static bool CheckJsonFileEmpty(string jsonName)
    {
        while (true)
        {
            try
            {
                var contentRootPath = startupPath;
                var file = $@"{contentRootPath}/JsonDB/" + jsonName + ".json";
                var FileData = System.IO.File.ReadAllText(file);
                return FileData.Length == 0 || FileData == "{}";
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    public static List<T> GetJsonData<T>(string jsonName)
    {
        while (true)
        {
            try
            {
                var contentRootPath = startupPath;
                var file = $@"{contentRootPath}/JsonDB/" + jsonName + ".json";
                var FileData = System.IO.File.ReadAllText(file);

                List<T> jsonObject = JsonConvert.DeserializeObject<List<T>>(FileData);

                return jsonObject;
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
        
    public static  void AddJsonData<T>(string jsonName, List<T> jsonList)
    {
        while (true)
        {
            try
            {
                var contentRootPath = startupPath;
                var jsonData = JsonConvert.SerializeObject(jsonList);
                //var contentRootPath = _hostingEnvironment.ContentRootPath;
                var file = $@"{contentRootPath}/JsonDB/" + jsonName + ".json";
                System.IO.File.WriteAllText(file, jsonData);
                break;
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    public static void CheckAndAddDataToJson<T>(string jsonName, T newData)
    {
        List<T> JsonData = new List<T>();

        if (!CheckJsonFileEmpty(jsonName))
        {
            JsonData = GetJsonData<T>(jsonName);

            JsonData.Add(newData);

        }
        else
        {
            JsonData.Add(newData);
        }

        AddJsonData<T>(jsonName, JsonData);

    }

    public static void EditAndAddDataToJson<T>(string jsonName, T newData) where T : IGenericIdInterface<T>
    {
        List<T> JsonData = new List<T>();

        if (!CheckJsonFileEmpty(jsonName))
        {
            JsonData = GetJsonData<T>(jsonName);

            var index = JsonData.FindIndex(c => c.Id == newData.Id);

            JsonData[index] = newData;


        }
        else
        {
            JsonData.Add(newData);
        }

        AddJsonData<T>(jsonName, JsonData);

    }

    public static  void RemoveAndAddDataToJson<T>
        (string jsonName, T newData) where T : IGenericIdInterface<T>
    {
        if (!CheckJsonFileEmpty(jsonName))
        {
            var JsonData = GetJsonData<T>(jsonName);

            var index = JsonData.FindIndex(c => c.Id == newData.Id);

            JsonData.RemoveAt(index);

            AddJsonData(jsonName, JsonData);
        }

    }


    public static void RemoveDataFromJson<T>
        (string jsonName, T removeData) where T : IGenericIdInterface<T>
    {

        List<T> JsonData = new List<T>();

        if (!CheckJsonFileEmpty(jsonName))
        {
            JsonData = GetJsonData<T>(jsonName);

            var index = JsonData.FindIndex(x => x.Id == removeData.Id);

            JsonData.RemoveAt(index);

            AddJsonData<T>(jsonName, JsonData);
        }

    }

    public interface IGenericIdInterface<out T>
    {
        public int Id { get; }
    }
        

        
}