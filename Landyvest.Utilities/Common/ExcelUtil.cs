using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class ExcelUtil
    {
        public static DataSet ExcelToDataSet(string SourceFilename)
        {
            DataSet ds = new DataSet();
            //try
            //{
            //    string connStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;\"", SourceFilename);

            //    OleDbConnection conn = new OleDbConnection(connStr);
            //    conn.Open();
            //    DataTable schemaDT = conn.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
            //    conn.Close();


            //    string tableName = schemaDT.Rows[0]["TABLE_NAME"].ToString();

            //    OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tableName), conn);

            //    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            //    adapter.Fill(ds);

            //}
            //catch (Exception ex)
            //{

            //}

            return ds;
        }

    }
}
