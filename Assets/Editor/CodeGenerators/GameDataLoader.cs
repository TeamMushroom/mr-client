using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TPM;
using TPMEditor;
using UnityEditor;
using UnityEngine;

public static class GameDataLoader
{
    private const string CacheKeyGameDataRepositoryPath = "GameDataRepositoryPath";
    
    public static (DataTable headerDataTable, DataTable dataTable) LoadExcelSheet(string excelName, string sheetName, int readRowCount = -1, bool useFilterSheet = true)
    {
        var (header, dataSet) = LoadExcel(excelName, readRowCount, useFilterSheet);
        return (header.Tables[sheetName], dataSet.Tables[sheetName]);
    }
    
    public static (DataSet header, DataSet dataSet) LoadExcel(string excelName, int readRowCount = -1, bool useFilterSheet = true)
    {
        var xlsxPath = $"{GetGameDataRepositoryExcelPath()}/{excelName}.xlsx";
        if (!File.Exists(xlsxPath))
        {
            Debug.LogError($"Not found file: {xlsxPath}");
            return (null, null);
        }

        var (header, dataSet) = ExcelUtil.GetGameDataSet(xlsxPath, readRowCount, useFilterSheet: useFilterSheet);

        foreach (DataTable dataTable in header.Tables)
        {
            dataTable.Namespace = excelName;
        }

        foreach (DataTable dataTable in dataSet.Tables)
        {
            dataTable.Namespace = excelName;
        }
        return (header, dataSet);
    }
    
    public static List<DataTable> LoadAllSheets(string excelName)
    {
        var list = new List<DataTable>();
        var (header, _) = LoadExcel(excelName, 0);

        if (header == null)
        {
            return list;
        }

        foreach (var obj in header.Tables)
        {
            list.Add(obj as DataTable);
        }

        return list;
    }
    
    public static GameDataColumnHeader ToGameDataHeaders(this DataTable @this)
    {
        var fileName = @this.Namespace;
        var sheetName = @this.TableName;
        var gameDataHeaders = new GameDataColumnHeader(fileName, sheetName);
        var types = @this.Rows[1];
        var options = @this.Rows[0];

        for (int i = 0; i < @this.Columns.Count; i++)
        {
            var key = @this.Columns[i].ColumnName;
            var name = key.TrimStart('#').Replace(" ", "");
            var type = types[key].ToString();
            var option = options[key].ToString();
            gameDataHeaders.AddHeaderData(name, type, option);
        }

        return gameDataHeaders;
    }
    
    public static string GetGameDataRepositoryPath(bool check = false)
    {
        var path = DefaultEditorCache.GetString(CacheKeyGameDataRepositoryPath);
        if (!check || !path.IsNullOrEmpty()) return path;
        
        const string error = "patch-data 경로를 설정해주세요.";
        EditorUtility.DisplayDialog("Error", error, "ok");
        EditorApplication.isPlaying = false;
        throw new Exception(error);
    }
    
    public static string GetGameDataRepositoryExcelPath()
    {
        return $"{GetGameDataRepositoryPath()}/protobuf";
    }
}
