using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MailScheduler.Models;

namespace MailScheduler.DAL
{
    class ICCIDMailDAL
    {
        private string constr = null;
        private string Query = null;
        public ICCIDMailDAL()
        {
            constr = ConfigurationManager.AppSettings["constr"];
            Query = ConfigurationManager.AppSettings["Query"];
        }

        public DataTable GetIccid()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(constr);
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                sda.Fill(dt);

                return dt;
            }
            catch(Exception ex)
            {
                return dt;
                
            }
            finally
            {
                con.Close();
            }
        }

        public int UpdateFlag(DataTable dt)
        {
            SqlConnection con = new SqlConnection(constr);
            string Ids = "";
            for(int i=0;dt.Rows.Count>i;i++)
            {
                Ids = Ids != "" ? Ids + "," + dt.Rows[i][1].ToString() : dt.Rows[i][1].ToString();
            }

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[CCUEOLS] SET IsICCIdSent ='Y', ICCIdSentDate='"+DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " "+DateTime.Now.Hour+":"+DateTime.Now.Minute+ "'  WHERE ccuid in  (" + Ids+")",con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                int res =  cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                return 0;

            }
            finally
            {
                con.Close();
            }
        }

    }
}
