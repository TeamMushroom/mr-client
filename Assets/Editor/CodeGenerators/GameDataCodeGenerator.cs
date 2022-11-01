using System;
using System.Collections.Generic;
using System.IO;
using TPM;
using UnityEngine;

public class GameDataCodeGenerator 
{
    public const string GeneratedGameDataNameSpace = "Generated";
    public static readonly string ScriptFilePath = $"{Application.dataPath}/1_Script/GameData";

    private static readonly string[] BlackListGenerateDataTableSheetNames =
    {
        //제외할 column이름 넣기
    };
    
    public static (Dictionary<string, GameDataColumnHeader>, List<string>) GetData()
    {
        var data = new Dictionary<string, GameDataColumnHeader>();
        var keys = new List<string>();

        try
        {
            foreach (var excelName in GetGameDataExcelNames())
            {
                if (string.IsNullOrEmpty(excelName)) continue;

                foreach (var sheet in GameDataLoader.LoadAllSheets(excelName))
                {
                    if (BlackListGenerateDataTableSheetNames.Contains(sheet.TableName)) continue;

                    data.Add(sheet.TableName, sheet.ToGameDataHeaders());
                    keys.Add(sheet.TableName);
                }
            }
            keys.Sort();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return (data, keys);
    }
    
    private static string[] GetGameDataExcelNames()
    {
        var gameDataRepositoryExcelPath = GameDataLoader.GetGameDataRepositoryExcelPath();
        var fileNames = new List<string>();
        var files = FileUtil.GetFiles(gameDataRepositoryExcelPath, "*Data*");
        for (var i = 0; i < files.Length; i++)
        {
            var fileName = Path.GetFileNameWithoutExtension(files[i]);
            if (fileName.StartsWith("~")) continue;
            fileNames.Add(fileName);
        }
        return fileNames.ToArray();
    }
}
