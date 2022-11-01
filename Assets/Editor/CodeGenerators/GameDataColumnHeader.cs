using System.Collections;
using System.Collections.Generic;
using TPMEditor;

public class GameDataColumnHeader : IEnumerable<GameDataColumnHeader.HeaderData>
{
    #region HeaderData

    private readonly List<HeaderData> _headerDatas = new List<HeaderData>();
    
    public class HeaderData
    {
        public string Name { get; }
        public string Type { get; }
        public string[] Options { get; }
        
        public HeaderData(string name, string type, string options)
        {
            Name = name.SnakeCaseToPascalCase();
            Type = type;
            Options = options.Split(';');

            for (var i = 0; i < Options.Length; i++)
            {
                Options[i] = Options[i].Trim().ToLower();
            }
        }
    }

    public IEnumerator<HeaderData> GetEnumerator()
    {
        return _headerDatas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _headerDatas.GetEnumerator();
    }
    
    public void AddHeaderData(string name, string type, string options)
    {
        _headerDatas.Add(new HeaderData(name, type, options));
    }
    
    #endregion
    
    public string FileName { get; private set; }
    public string SheetName { get; private set; }
    
    public GameDataColumnHeader(string fileName, string sheetName)
    {
        FileName = fileName;
        SheetName = sheetName;
    }
}
