using System.IO;
using UnityEngine;

namespace Helpers
{
    public class LocalCacheManager : MonoBehaviour
    {
        private const string JsonFileExtension = "json";

        // single file

        public T Load<T>()
        {
            string filePath = GetFilePath<T>(JsonFileExtension);

            if (File.Exists(filePath) == true)
            {
                try
                {
                    T objectData = JsonUtility.FromJson<T>(File.ReadAllText(filePath));
                    return objectData;
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public bool Save<T>(T data, bool allowOverwrite)
        {
            string filePath = GetFilePath<T>(JsonFileExtension);

            return SaveAtPath<T>(data, filePath, allowOverwrite);
        }

        public bool FileExists<T>()
        {
            string filePath = GetFilePath<T>(JsonFileExtension);
            return File.Exists(filePath);
        }

        // multi file

        public T LoadFromTypedFolder<T>(string filename)
        {
            string filePath = GetFileInFolderPath<T>(filename, JsonFileExtension);

            if (File.Exists(filePath) == true)
            {
                try
                {
                    T objectData = JsonUtility.FromJson<T>(File.ReadAllText(filePath));
                    return objectData;
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public string[] ListTypedFolder<T>()
        {
            string folderPath = GetTypedFolderPath<T>();
            string[] result;
            if (Directory.Exists(folderPath) == false)
            {
                result = new string[] { };
            }
            else
            {
                result = Directory.GetFiles(folderPath);
            }
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Path.GetFileNameWithoutExtension(result[i]);
            }
            return result;
        }

        public bool FileExistsInTypedFolder<T>(string fileName)
        {
            foreach (string fileInFolder in ListTypedFolder<T>())
            {
                if (fileInFolder == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool SaveInTypedFolder<T>(T data, string filename, bool allowOverwrite)
        {
            string filePath = GetFileInFolderPath<T>(filename, JsonFileExtension);

            return SaveAtPath<T>(data, filePath, allowOverwrite);
        }

        // private helpers

        private bool SaveAtPath<T>(T data, string filePath, bool allowOverwrite)
        {
            if (File.Exists(filePath) == true && allowOverwrite == true)
            {
                File.Delete(filePath);
            }

            if (File.Exists(filePath) == false)
            {
                string textData = JsonUtility.ToJson(data, true);
                File.WriteAllText(filePath, textData);
            }
            else
            {
                return false;
            }

            return true;
        }

        private string GetFilePath<T>(string fileType)
        {
            return Path.Combine(Application.persistentDataPath, typeof(T).FullName + "." + fileType);
        }

        private string GetFileInFolderPath<T>(string filename, string fileType)
        {
            string folderPath = GetTypedFolderPath<T>();

            Directory.CreateDirectory(folderPath);

            return Path.Combine(folderPath, filename + "." + fileType);
        }

        private static string GetTypedFolderPath<T>()
        {
            return Path.Combine(Application.persistentDataPath, typeof(T).FullName);
        }
    }
}
