using System;
using System.Collections.Generic;
using TPM;

public class EditorCache
{
    public EditorCache(string path, string oldPath)
    {
        try
        {
            _path = path;

            // for migrate
            if (FileUtil.IsExists(path))
            {
                _data = JsonUtil.DeserializeObject<Dictionary<string, string>>(FileUtil.ReadText(path));
            }
            else
            {
                if (FileUtil.IsExists(oldPath))
                {
                    _data = JsonUtil.DeserializeObject<Dictionary<string, string>>(FileUtil.ReadText(oldPath));

                    FileUtil.RemoveFile(oldPath);
                    Save();
                }
            }
        }
        catch (Exception e)
        {
            //D.Error(e);
        }

        if (_data == null)
        {
            _data = new Dictionary<string, string>();
        }
    }
    
    private string _path;
    private Dictionary<string, string> _data = new Dictionary<string, string>();
    
    public string GetString(string key, string defaultValue = "")
    {
        string value;
        return _data.TryGetValue(key, out value) ? value : defaultValue;
    }
    
    public void SetString(string key, string value)
    {
        _data[key] = value;
        Save();
    }

    public void Save()
    {
        //D.LogPack("Save to editor cache", _path);
        FileUtil.WriteText(_path, _data.ToIndentedJsonString());
    }
}