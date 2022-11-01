using System;

namespace ExcelDataReader
{
    /// <summary>
    /// Processing configuration options and callbacks for AsDataTable().
    /// </summary>
    public class ExcelDataTableConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating the prefix of generated column names.
        /// </summary>
        public string EmptyColumnNamePrefix { get; set; } = "Column";

        /// <summary>
        /// Gets or sets a value indicating whether to use a row from the data as column names.
        /// </summary>
        public bool UseHeaderRow { get; set; } = false;

        /// <summary>
        /// Gets or sets a callback to determine which row is the header row. Only called when UseHeaderRow = true.
        /// </summary>
        public Action<IExcelDataReader> ReadHeaderRow { get; set; }

        /// <summary>
        /// Gets or sets a callback to determine whether to include the current row in the DataTable.
        /// </summary>
        public Func<IExcelDataReader, bool> FilterRow { get; set; }

        /// <summary>
        /// Gets or sets a callback to determine whether to include the specific column in the DataTable. Called once per column after reading the headers.
        /// </summary>
        public Func<IExcelDataReader, int, bool> FilterColumn { get; set; }

        /// <summary>
        /// CK : 처음 N개의 row를 무시합니다. 헤더가 여러줄인 엑셀 게임데이터 대응용입니다.
        /// </summary>
        public int IgnoreFirstNRow { get; set; } = 0;

        /// <summary>
        /// CK : Header를 제외하고 몇개의 행을 읽을지 결정합니다. 0보다 작을 경우 전부 읽습니다.
        /// </summary>
        public int ReadRowCount { get; set; } = -1;

        /// <summary>
        /// CK : Header를 제외하고 몇개의 열을 결정합니다. 0보다 작을 경우 전부 읽습니다.
        /// </summary>
        public int ReadFieldCount { get; set; } = -1;
    }
}