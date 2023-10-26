using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoneRightAddressAPI.DAL.Error_Logs
{
    /// <summary>
    /// Class to Handle Exceptions and Log them.
    /// </summary>
    public static class Generate_Exception
    {
        #region Methods
        /// <summary>
        /// Creates a Error Log file in wwwroot > Error_Log.
        /// A file is created for each day. All the exceptions are stored according to DateTime.
        /// </summary>
        /// <param name="exception">Exception in code.</param>
        /// <returns>Returns the Exception passed in parameter after creating or updating the file</returns>
        public static Exception WriteErrorLog(Exception exception)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(Directory.GetCurrentDirectory() + "/wwwroot/error_log/DoneRightAddressAPI_Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString() + "\n Message: " + exception.Message.Trim() + "\n Source Project: " + exception.Source.Trim() + "\n Stack Trace: " + exception.StackTrace.Trim());
                sw.Flush();
                sw.Close();
                return exception;
            }
            catch
            {
                return exception;
            }
        }
        #endregion
    }
}
