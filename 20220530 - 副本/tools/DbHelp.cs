using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch.tools
{
    class DbHelp
    {
        static readonly string connstr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
        static SqlConnection conn = null;
        public static void sql_Insert(List<string> ls)
        {
            string time = DateTime.Now.ToString();
            conn = new SqlConnection(connstr);
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("insert into  stock (insert_time,read_mark,line,read_time,product_number,cd1,cd2,cd3,cd4,cd5,cd6,cd7,cd8,cd9,cd10,cd11,cd12,cd13,cd14,cd15," +
                    "cd16,cd17,cd18,cd19,cd20,cd21,cd22,cd23,cd24,cd25,cd26,cd27,cd28,cd29,cd30) " +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}')",
                    DateTime.Now.ToString(), ls[1], ls[2], time, ls[4], ls[5], ls[6], ls[7], ls[8], ls[9], ls[10], ls[11], ls[12], ls[13], ls[14], ls[15], ls[16], ls[17], ls[18], ls[19], ls[20], ls[21], ls[22], ls[23], ls[24], ls[25], ls[26], ls[27], ls[28], ls[29], ls[30], ls[31], ls[32], ls[33], ls[34]); 
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("DbHelp.sql_Insert：" + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
