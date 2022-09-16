using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System;
using UnityEditor;


public class ExcelUtility
{
    //单元格信息
    public struct CellInfo 
    {
        public string Type;//类型
        public string Name;//名称
        public string Description;//描述
    }
    
    private const int CommentsLine = 0;//注释行
    private const int VariableNameLine = 1;//变量名行
    private const int VariableTypeLine = 2;//变量类型行
    private const int DataLine = 3;//数据类型行

    private string excelPath;
    /// <summary>
    /// 表格数据集合
    /// </summary>
    private DataSet mResultSet;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="excelFile">Excel file.</param>
    public ExcelUtility(string excelFile)
    {
        excelPath = excelFile;
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        mResultSet = mExcelReader.AsDataSet();
    }
    

    /// <summary>
    /// 转换为Json
    /// </summary>
    /// <param name="_jsonPath">导出Json文件路径</param>
    /// <param name="_cellPath">导出Json实体类路径</param>
    /// <param name="_nameSpace">实体类的命名空间</param>
    /// <param name="encoding">编码格式</param>
    public void ConvertToJson(string _jsonPath,string _cellPath,string _nameSpace, Encoding encoding)
    {
        var sheetCount = mResultSet.Tables.Count;
        //判断Excel文件中是否存在数据表
        if (sheetCount < 1)
            return;

        for (int i = 0; i < sheetCount; i++)
        {
            DataTable mSheet = mResultSet.Tables[i];
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                continue;
            
            string exportPath = Path.Combine(_jsonPath, $"{mSheet.TableName}.txt");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(txt,encoding))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\n");
                    sb.Append($"\"info\":[");
                    sw.WriteLine(sb.ToString());
                    ExportSheet(mSheet, sw);
                    StringBuilder sbs = new StringBuilder();
                    sbs.Append("\t]\n");
                    sbs.Append("}");
                    sw.WriteLine(sbs.ToString());
                }
            }
        }
        
        ExportAllCalss(_cellPath,_nameSpace,encoding);
        Debug.Log("excel转json成功！");
    }
    
    /// <summary>
    /// 导表json
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="sw"></param>
    private void ExportSheet(DataTable sheet, StreamWriter sw)
    {
        //读取数据表行数和列数
        int rowCount = sheet.Rows.Count;
        int colCount = sheet.Columns.Count;
        CellInfo[] cellInfos = new CellInfo[colCount];
        for (int i = 0; i < colCount; i++)
        {
            string fieldDesc = sheet.Rows[CommentsLine][i].ToString();
            string fieldName = sheet.Rows[VariableNameLine][i].ToString();
            string fieldType = sheet.Rows[VariableTypeLine][i].ToString();
            cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Description = fieldDesc };
        }

        //从第四行开始写入所有item值
        for (int i = DataLine; i < rowCount; ++i)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            for (int j = 0; j < colCount; ++j)
            {
                string desc = cellInfos[j].Description.ToLower();
                if (desc.StartsWith("#"))
                {
                    continue;
                }
                
                string fieldValue = sheet.Rows[i][j].ToString()??"";

                if (fieldValue == "")
                {
                    //  Log.Error($"sheet: {sheet.SheetName} 中有空白字段 {i},{j}");
                    //throw new Exception($"sheet: {sheet.SheetName} 中有空白字段 {i},{j}");
                }

                if (j > 0)
                {
                    sb.Append(",");
                }

                string fieldName = cellInfos[j].Name;
                
                string fieldType = cellInfos[j].Type;

                if (fieldType == "int" && fieldValue == "") fieldValue = "0";

                sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
            }
            sb.Append(i == rowCount-1 ? "\n}" : "\n},");
            sw.WriteLine(sb.ToString());
        }
    }

    private static string Convert(string type, string value)
    {
        switch (type)
        {
            case "int[]":
            case "int32[]":
            case "long[]":
                return $"[{value}]";
            case "string[]":
                return $"[{value}]";
            case "int":
            case "int32":
            case "int64":
            case "long":
            case "float":
            case "double":
                return value;
            case "string":
                return $"\"{value}\"";
            default:
                throw new Exception($"不支持此类型: {type}");
        }
    }
    
    /// <summary>
    /// 导出所有配置表为cs文件
    /// </summary>
    /// <param name="exportDir">导出路径</param>
    /// <param name="csHead">命名空间</param>
    public void ExportAllCalss(string exportDir, string csHead,Encoding encoding)
    {
        var sheetCount = mResultSet.Tables.Count;
        //判断Excel文件中是否存在数据表
        if (sheetCount < 1)
            return;

        for (int i = 0; i < sheetCount; i++)
        {
            DataTable sheet = mResultSet.Tables[i];
            //读取数据表行数和列数
            int rowCount = sheet.Rows.Count;
            int colCount = sheet.Columns.Count;
            //判断数据表内是否存在数据
            if (rowCount < 1)
                continue;
            string protoName = sheet.TableName;
            string exportPath = Path.Combine(exportDir, $"{protoName}.cs");
            
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(txt,encoding))
                {
                    StringBuilder sb = new StringBuilder();
                    //以下是要生产的格式
                    sb.Append("using System.Collections.Generic;\t\n");
                    if (!string.IsNullOrEmpty(csHead))
                    {
                        sb.Append($"namespace {csHead}\n");//类名
                        sb.Append("{\n");
                    }

                    sb.Append($"public class {protoName}s\n");//类名
                    sb.Append("{\n");
                    sb.Append($"\tpublic List<{protoName}> info;\n");
                    sb.Append("}\n\n");


                    sb.Append($"[System.Serializable]\n");
                    sb.Append($"public class {protoName}\n");//类名
                    sb.Append("{\n");

                    for (int j = 0; j < colCount; j++)
                    {
                        string fieldDes = sheet.Rows[CommentsLine][j].ToString();
                        if (fieldDes.StartsWith("#")) continue;
                        
                        string fieldName = sheet.Rows[VariableNameLine][j].ToString();

                        string fieldType = sheet.Rows[VariableTypeLine][j].ToString();

                        if (fieldType == "" || fieldName == "") continue;

                        sb.Append($"\t///{fieldDes} \n");
                        sb.Append($"\tpublic {fieldType} {fieldName};\n");
                    }
                    sb.Append("}\n");
                    if (!string.IsNullOrEmpty(csHead))
                    {
                        sb.Append("}\n");
                    }
                    sw.Write(sb.ToString());
                }
            }
        }
        AssetDatabase.Refresh();
    }
    

    /// <summary>
    /// 多sheet转换为Json
    /// </summary>
    /// <param name="JsonPath">Json文件路径</param>
    /// <param name="Header">表头行数</param>
    public void ConvertToJsons(string JsonPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;
        for (int k = 0; k < mResultSet.Tables.Count; k++)
        {
            DataTable mSheet = mResultSet.Tables[k];
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                return;

            //读取数据表行数和列数
            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            //准备一个列表存储整个表的数据
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            //读取数据
            for (int i = 1; i < rowCount; i++)
            {
                //准备一个字典存储每一行的数据
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int j = 0; j < colCount; j++)
                {
                    //读取第1行数据作为表头字段
                    string field = mSheet.Rows[0][j].ToString();
                    //Key-Value对应
                    row[field] = mSheet.Rows[i][j];
                }

                //添加到表数据中
                table.Add(row);
            }

            //生成Json字符串
            string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
            string[] temp = JsonPath.Split('.');
            //JsonPath = temp[0]+k+".json";
            //写入文件
            using (FileStream fileStream = new FileStream(temp[0] + "_sheet" + k + ".json", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                {
                    textWriter.Write(json);
                }
            }

        }
    }



    /// <summary>
    /// 转换为lua
    /// </summary>
    /// <param name="luaPath">lua文件路径</param>
    public void ConvertToLua(string luaPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("local datas = {");
        stringBuilder.Append("\r\n");

        //读取数据表
        foreach (DataTable mSheet in mResultSet.Tables)
        {
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                continue;

            //读取数据表行数和列数
            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            //准备一个列表存储整个表的数据
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            //读取数据
            for (int i = 1; i < rowCount; i++)
            {
                //准备一个字典存储每一行的数据
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int j = 0; j < colCount; j++)
                {
                    //读取第1行数据作为表头字段
                    string field = mSheet.Rows[0][j].ToString();
                    //Key-Value对应
                    row[field] = mSheet.Rows[i][j];
                }
                //添加到表数据中
                table.Add(row);
            }
            stringBuilder.Append(string.Format("\t\"{0}\" = ", mSheet.TableName));
            stringBuilder.Append("{\r\n");
            foreach (Dictionary<string, object> dic in table)
            {
                stringBuilder.Append("\t\t{\r\n");
                foreach (string key in dic.Keys)
                {
                    if (dic[key].GetType().Name == "String")
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = \"{1}\",\r\n", key, dic[key]));
                    else
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = {1},\r\n", key, dic[key]));
                }
                stringBuilder.Append("\t\t},\r\n");
            }
            stringBuilder.Append("\t}\r\n");
        }

        stringBuilder.Append("}\r\n");
        stringBuilder.Append("return datas");

        //写入文件
        using (FileStream fileStream = new FileStream(luaPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
    }


    /// <summary>
    /// 转换为CSV
    /// </summary>
    public void ConvertToCSV(string CSVPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //创建一个StringBuilder存储数据
        StringBuilder stringBuilder = new StringBuilder();

        //读取数据
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                //使用","分割每一个数值
                stringBuilder.Append(mSheet.Rows[i][j] + ",");
            }
            //使用换行符分割每一行
            stringBuilder.Append("\r\n");
        }

        //写入文件
        using (FileStream fileStream = new FileStream(CSVPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
    }

    /// <summary>
    /// 导出为Xml
    /// </summary>
    public void ConvertToXml(string XmlFile)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //创建一个StringBuilder存储数据
        StringBuilder stringBuilder = new StringBuilder();
        //创建Xml文件头
        stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.Append("\r\n");
        //创建根节点
        stringBuilder.Append("<Table>");
        stringBuilder.Append("\r\n");
        //读取数据
        for (int i = 1; i < rowCount; i++)
        {
            //创建子节点
            stringBuilder.Append("  <Row>");
            stringBuilder.Append("\r\n");
            for (int j = 0; j < colCount; j++)
            {
                stringBuilder.Append("   <" + mSheet.Rows[0][j].ToString() + ">");
                stringBuilder.Append(mSheet.Rows[i][j].ToString());
                stringBuilder.Append("</" + mSheet.Rows[0][j].ToString() + ">");
                stringBuilder.Append("\r\n");
            }
            //使用换行符分割每一行
            stringBuilder.Append("  </Row>");
            stringBuilder.Append("\r\n");
        }
        //闭合标签
        stringBuilder.Append("</Table>");
        //写入文件
        using (FileStream fileStream = new FileStream(XmlFile, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8")))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
    }
}

