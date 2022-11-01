using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace TPM
{
    public static partial class FileUtil
    {
        private static Encoding UTF8NoBOM => new UTF8Encoding(false);

        public static bool IsExists(string path)
        {
            try
            {
                return File.Exists(path) || Directory.Exists(path);
            }
            catch (Exception e)
            {
                //D.ErrorPack(path, e);
                return false;
            }
        }

        public static string[] GetFiles(string path, string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                if (!Directory.Exists(path)) return null;

                return Directory.GetFiles(path, searchPattern, searchOption);
            }
            catch (Exception e)
            {
                //D.ErrorPack(path, e);
                return null;
            }
        }

        public static void RemoveFile(string path)
        {
            File.Delete(path);
        }

        public static string ReadText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                //D.ErrorPack(path, e);
                return string.Empty;
            }
        }

        public static bool WriteText(string path, string text)
        {
            return WriteText(path, text, UTF8NoBOM);
        }

        public static bool WriteText(string path, string text, Encoding encoding)
        {
            try
            {
                CreateDirectoryFromFilePath(path);
                File.WriteAllText(path, text, encoding);
                return true;
            }
            catch (Exception e)
            {
                //D.ErrorPack(path, e);
                return false;
            }
        }

        public static void CreateDirectoryFromFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            var directoryName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(directoryName)) return;

            Directory.CreateDirectory(directoryName);
        }

        public static string GetEditorId()
        {
            var editorFolderPath = Path.GetDirectoryName(Application.dataPath);
            var editorFolderName = Path.GetFileName(editorFolderPath);
            var hash = GetHashFromMemory(editorFolderPath);
            return $"{editorFolderName}_{hash}";
        }
    }
}