using System;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;

namespace TPM
{
    public static class ExcelUtil
    {
        private static string[] _tempOptions;
        private static string[] _tempTypes;
        
        private static void PrintIfLongTimeLoaded(string tableName, long time)
        {
            if (time > 1000)
            {
                Debug.LogError($"{tableName}, {time}");
            }
        }
        
        public static (DataSet header, DataSet data) GetGameDataSet(string filePath, int readRowCount = -1, int readFieldCount = 100, bool useFilterSheet = true)
        {
            var header = new DataSet();

            try
            {
                var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var excelDataReader = ExcelReaderFactory.CreateReader(stream);
                var configuration = GetGameDataSetConfiguration();
                var dataSet = excelDataReader.AsDataSet(configuration, PrintIfLongTimeLoaded);
                stream.Close();
                return (header, dataSet);

                ExcelDataSetConfiguration GetGameDataSetConfiguration()
                {
                    return new ExcelDataSetConfiguration
                    {
                        FilterSheet = FilterSheet,
                        ConfigureDataTable = tableReader => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true,
                            ReadHeaderRow = OnReadHeaderRow,
                            IgnoreFirstNRow = 2,
                            ReadRowCount = readRowCount,
                            ReadFieldCount = readFieldCount,
                        }
                    };
                }

                bool FilterSheet(IExcelDataReader dataReader, int index)
                {
                    if (useFilterSheet)
                    {
                        return !dataReader.Name.StartsWith("_");
                    }

                    return true;
                }

                void OnReadHeaderRow(IExcelDataReader dataReader)
                {
                    // 필드가 너무 많은 경우 줄인다.
                    var fieldCount = dataReader.FieldCount;
                    if (fieldCount > readFieldCount)
                    {
                        Debug.LogError($"필드가 너무 많습니다., {dataReader.Name}, {fieldCount}");
                        fieldCount = readFieldCount;
                    }

                    switch (dataReader.Depth)
                    {
                        case 0: // Option
                        {
                            _tempOptions = new string[fieldCount];
                            for (var i = 0; i < _tempOptions.Length; i++)
                            {
                                _tempOptions[i] = Convert.ToString(dataReader.GetValue(i));
                            }

                            break;
                        }
                        case 1: // Type
                        {
                            _tempTypes = new string[fieldCount];
                            for (var i = 0; i < _tempTypes.Length; i++)
                            {
                                _tempTypes[i] = Convert.ToString(dataReader.GetValue(i));
                            }

                            break;
                        }
                        case 2: // Name
                        {
                            var dataTable = new DataTable()
                            {
                                TableName = dataReader.Name,
                            };

                            // add column
                            for (var i = 0; i < fieldCount; i++)
                            {
                                var name = Convert.ToString(dataReader.GetValue(i));
                                var columnName = ExcelDataReaderExtensions.GetUniqueColumnName(dataTable, name);
                                var column = new DataColumn(columnName, typeof(object)) { Caption = name };
                                dataTable.Columns.Add(column);
                            }

                            // add option row
                            var row = dataTable.NewRow();
                            for (var i = 0; i < _tempOptions.Length; i++)
                            {
                                row[i] = _tempOptions[i];
                            }

                            dataTable.Rows.Add(row);

                            // add type row
                            row = dataTable.NewRow();
                            for (var i = 0; i < _tempTypes.Length; i++)
                            {
                                row[i] = _tempTypes[i];
                            }

                            dataTable.Rows.Add(row);

                            // add data table
                            header.Tables.Add(dataTable);

                            _tempOptions = null;
                            _tempTypes = null;

                            break;
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Debug.LogError($"{filePath}, {exception}");
                return (null, null);
            }
        }
    }
}