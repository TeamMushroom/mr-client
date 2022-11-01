using System.IO;
using UnityEngine;
using FileUtil = TPM.FileUtil;

public class DefaultEditorCache 
{
    public static string DefaultEditorCacheFolderPath = $"{Application.persistentDataPath}/{FileUtil.GetEditorId()}";
    public static string DefaultEditorCachePath = $"{DefaultEditorCacheFolderPath}/default_editor_cache.editorcache";
    public static string OldDefaultEditorCachePath = $"{Application.persistentDataPath}/default_editor_cache.editorcache";

    private static EditorCache _editorCacheInternal;

    private static EditorCache _editorCache
    {
        get
        {
            if (_editorCacheInternal == null)
            {
                if (Directory.Exists(DefaultEditorCacheFolderPath))
                {
                    Directory.CreateDirectory(DefaultEditorCacheFolderPath);
                }

                _editorCacheInternal = new EditorCache(DefaultEditorCachePath, OldDefaultEditorCachePath);
            }

            return _editorCacheInternal;
        }
    }
    
    public static string GetString(string key, string defaultValue = "")
    {
        return _editorCache.GetString(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
        _editorCache.SetString(key, value);
    }
}