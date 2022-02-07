using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ClosedXML.Excel;
using MailScheduler.DAL;
using MailScheduler.Models;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace MailScheduler.BAL
{
    class ICCIDMailBAL
    {
        ICCIDMailDAL _iccid = null;
        
        public string Application = "";
        public string ExcelPath = "";
        public string LogPath = "";
        
        public string fileName = "ICCIDData" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + ""+DateTime.Now.Year;
        
        public ICCIDMailBAL()
        {
            _iccid = new ICCIDMailDAL();
           
            Application = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Application = Application.Replace("file:\\", "");
            
            LogPath = ConfigurationManager.AppSettings["Logs"].ToString();
            //fileName = fileName.Replace("-", "");
            ExcelPath = Application + "\\ExcelFiles\\"+fileName+".xlsx";
        }

        public void getIccid()
        {
            try
            {
                //throw new Exception("Manully created exception");
                List<IccidModel> lst = new List<IccidModel>();
               

                Console.WriteLine("===== Getting Iccid data  =====");
                DataTable dt = _iccid.GetIccid();

                Console.WriteLine("===== Get Iccid count :" +dt.Rows.Count);
                var mailTo = ConfigurationManager.AppSettings["SendMailTo"].ToString();
                if (dt.Rows.Count > 0)
                {
                    ExportDataSetToExcel(dt, ExcelPath);
                    Console.WriteLine("Sending mail....");

                    SendEmail(mailTo, "ICCID List for Bootstrap Plan Activation", ExcelPath);
                    Console.WriteLine("Sent mail....");
                    _iccid.UpdateFlag(dt);

                    Console.WriteLine("Record updated in eol....");
                }
                else
                {
                    Console.WriteLine("No data Found...");
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteLogInfile(LogPath, ex.Message + " " + ex.StackTrace);
            }
        }

        private void ExportDataSetToExcel(DataTable dt, string File)
        {
            try
            {
                DataTable cpdt = dt.Copy();
                cpdt.Columns.Remove("ccuid");

                Console.WriteLine("===== Creating Excel for get data =====");

                using (XLWorkbook wb = new XLWorkbook())
                {
                    
                    wb.Worksheets.Add(cpdt, fileName);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    wb.SaveAs(File);
                }

                Console.WriteLine("===== Excel Created  =====");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



        private void SendEmail(string MailTo, string MailSubject, string file)
        {
            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                mail.To.Add(MailTo);
                mail.Bcc.Add(ConfigurationManager.AppSettings["SendMailBCC"].ToString());
                List<string> li = new List<string>();
                mail.IsBodyHtml = true;
                mail.Subject = MailSubject;
                mail.Body = "Dear Team,<br><br>";
                mail.Body = mail.Body + "Please find attached ICCID List for Bootstrap Plan Activation.<br><br>";
                mail.Body = mail.Body + "Thanks <br><br>";
               // mail.Body = mail.Body + "*This is an automatically generated email, please do not reply*";
                Attachment attachment = new Attachment(file);
                mail.Attachments.Add(attachment);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "128.9.24.24";
                smtp.EnableSsl = false;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = "info@email.com";
                NetworkCred.Password = "email_password";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("Email id of Gmail", "Password of Gmail");
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
