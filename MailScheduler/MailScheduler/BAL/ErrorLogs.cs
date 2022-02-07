using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MailScheduler.BAL
{
    class ErrorLogs
    {
        public static bool WriteLogInfile(string FilePath, string strMessage)
        {
            try
            {
                FileStream objFilestream = null;
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                //if (!File.Exists(FilePath + "\\Log.txt"))
                //{
                //    File.Create(FilePath + "\\Log.txt");
                //}
                objFilestream = new FileStream(FilePath + "\\Log.txt", FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage +" - "+DateTime.Now.ToShortDateString());
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
