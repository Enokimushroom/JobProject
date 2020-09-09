using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;

public class ExcelUtility
{
    private DataSet mResultSet;

    public ExcelUtility(string excelFile)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        mResultSet = mExcelReader.AsDataSet();
    }

    public void ConvertToJson(string JsonPath,Encoding encoding)
    {
        if (mResultSet.Tables.Count < 1) return;
        //读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];
        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1) return;
        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //列表存储整个表的数据
        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
        //读取数据
        for(int i = 1; i < rowCount; ++i)
        {
            //一个字典存储一行数据
            Dictionary<string, object> row = new Dictionary<string, object>();
            for(int j = 0; j < colCount; ++j)
            {
                //第一行是表头字段
                string field = mSheet.Rows[0][j].ToString();
                row[field] = mSheet.Rows[i][j];
            }
            table.Add(row);
        }

        //生成Json字符串
        string json = JsonConvert.SerializeObject(table, Formatting.Indented);
        //写入文件
        using (FileStream fileStream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                textWriter.Write(json);
        }
    }
}
