using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;


namespace Portal.Common
{
    public static class Logger
    {

        public static bool IsErrorEnabled
        {
            get
            {
                return false;
             //   return Convert.ToBoolean(ConfigurationSettings.AppSettings["IsErrorEnabled"]);
            }
        }



        private static StreamWriter sw = null;
        /// <summary>
        /// For checking the directory
        /// </summary>
        /// <param name="strLogPath"> Directory path </param>
        /// <returns> true if directory exits; false otherwise </returns>        
        public static bool CheckDirectory(string strLogPath)
        {
            try
            {
                if (!Directory.Exists(strLogPath))
                    Directory.CreateDirectory(strLogPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// specified error message and a reference to the inner exception that is 
        /// the cause of this exception.
        /// </summary>
        ///<param name="objException">The exception that is the cause of the current 
        /// exception.  If the innerException parameter is not a <see langword="null"/> reference, 
        /// the current exception is raised in a catch block that handles the inner 
        /// exception.
        /// </param>
        /// <param name="folderCreationMode">Specifies the folder creation type</param>
        /// <param name="mode">Specifies Product Mode</param>
        /// <param name="errorMode">Specifies Error Mode</param>
        /// <param name="searchId">SearchId for identifier.</param>
        /// <param name="portal">Specifies Portal</param>
        /// <param name="module">Specifies module</param>
        /// <returns> true if log was written successfully; false otherwise</returns>
        public static bool WriteErrorLog(Exception objException, FolderCreationMode folderCreationMode, ProductMode mode, ErrorMode errorMode, string searchId, string portal, string module)
        {
            bool bReturn = false;
            string strException = string.Empty;
            try
            {//Inactive conversation

                if (string.IsNullOrEmpty(searchId))
                {
                    searchId = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "").Replace(":", "").Replace("AM", "").Replace("PM", "");
                }

                string path = GetErrorLogFilePath(folderCreationMode, searchId);
                WriteException(path, objException.StackTrace);
                //LogException.clsLogException objLog = new LogException.clsLogException();
                //objLog.WriteException(Mode.ToString(), "Flights", objException.Message);//Module, Product, Error Msg

                //sw = new StreamWriter(GetErrorLogFilePath(folderCreationMode), true);
                //sw.WriteLine("Source		: " + objException.Source.ToString().Trim());
                //sw.WriteLine("Method		: " + objException.TargetSite.Name.ToString());
                //sw.WriteLine("Date		: " + DateTime.Now.ToLongTimeString());
                //sw.WriteLine("Time		: " + DateTime.Now.ToShortDateString());
                //sw.WriteLine("Computer	: " + Dns.GetHostName().ToString());
                //sw.WriteLine("Error		: " + objException.Message.ToString().Trim());
                //sw.WriteLine("Stack Trace	: " + objException.StackTrace.ToString().Trim());
                //sw.WriteLine("^^-------------------------------------------------------------------^^");
                //sw.Flush();
                //sw.Close();



                #region Save Log / Error to DB
                //TODO: Method to Save Error Message to DB
                #endregion
                #region Send Log / Error Email to admininistrators
                //TODO: EMail Method to Send Error Message
                #endregion

                #region Send Log / Error SMS to admininistrators
                //TODO: SMS Method to Send SMS for Critical Errors
                #endregion


                bReturn = true;
            }
            catch (Exception ex)
            {
                bReturn = false;
            }
            return bReturn;
        }



        public static bool WriteErrorLog(Exception objException)
        {
            bool bReturn = false;
            string strException = string.Empty;
            try
            {//Inactive conversation


                var searchId = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "").Replace(":", "").Replace("AM", "").Replace("PM", "");


                string path = GetErrorLogFilePath(FolderCreationMode.Day, searchId);
                WriteException(path, objException.StackTrace);
                //LogException.clsLogException objLog = new LogException.clsLogException();
                //objLog.WriteException(Mode.ToString(), "Flights", objException.Message);//Module, Product, Error Msg

                //sw = new StreamWriter(GetErrorLogFilePath(folderCreationMode), true);
                //sw.WriteLine("Source		: " + objException.Source.ToString().Trim());
                //sw.WriteLine("Method		: " + objException.TargetSite.Name.ToString());
                //sw.WriteLine("Date		: " + DateTime.Now.ToLongTimeString());
                //sw.WriteLine("Time		: " + DateTime.Now.ToShortDateString());
                //sw.WriteLine("Computer	: " + Dns.GetHostName().ToString());
                //sw.WriteLine("Error		: " + objException.Message.ToString().Trim());
                //sw.WriteLine("Stack Trace	: " + objException.StackTrace.ToString().Trim());
                //sw.WriteLine("^^-------------------------------------------------------------------^^");
                //sw.Flush();
                //sw.Close();



                #region Save Log / Error to DB
                //TODO: Method to Save Error Message to DB
                #endregion
                #region Send Log / Error Email to admininistrators
                //TODO: EMail Method to Send Error Message
                #endregion

                #region Send Log / Error SMS to admininistrators
                //TODO: SMS Method to Send SMS for Critical Errors
                #endregion


                bReturn = true;
            }
            catch (Exception ex)
            {
                bReturn = false;
            }
            return bReturn;
        }

        /// <summary>
        /// For serializing the object
        /// </summary>
        /// <param name="type">Type of the object to be serialized</param>
        /// <param name="ob">Object to be serialized</param>
        /// <param name="folderCreationMode">Specifies the folder name format</param>
        /// <param name="searchId">Search Id</param>
        /// <returns> true if log was written successfully; false otherwise</returns>
        internal static bool ObjectSerializer(Type type, Object ob, FolderCreationMode folderCreationMode, string searchId)
        {
            bool bReturn = false;
            try
            {
                var xmlDocSer = new XmlSerializer(type);
                sw = new StreamWriter(GetErrorLogFilePath(folderCreationMode, searchId), true);
                xmlDocSer.Serialize(sw, ob);
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine("^^-------------------------------------------------------------------^^");
                sw.Flush();
                sw.Close();
                bReturn = true;
            }
            catch (Exception)
            {
                bReturn = false;
            }
            return bReturn;
        }

        /// <summary>
        ///  for writing the string message
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="folderCreationMode"></param>
        /// <param name="searchId">SearchId for identifier.</param>
        /// <returns> true if log was written successfully; false otherwise</returns>
        internal static bool MessageWriter(string strMessage, FolderCreationMode folderCreationMode, string searchId)
        {
            bool bReturn = false;
            try
            {

                sw = new StreamWriter(GetErrorLogFilePath(folderCreationMode, searchId), true);
                sw.WriteLine("Message		: " + strMessage);
                sw.WriteLine("^^-------------------------------------------------------------------^^");
                sw.Flush();
                sw.Close();
                bReturn = true;
            }
            catch (Exception)
            {
                bReturn = false;
            }
            return bReturn;
        }



        /// <summary>
        /// Create Directory 
        ///</summary>
        internal static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// For setting the log Path of the application
        /// </summary>
        /// <param name="folderCreationMode">Specifies the folder name format</param>
        /// <param name="searchId">SearchId for identifier.</param>
        /// <returns></returns>
        internal static string GetErrorLogFilePath(FolderCreationMode folderCreationMode, string searchId)
        {
            try
            {

                string folderName = string.Empty;

                // get the base directory
                switch (folderCreationMode)
                {
                    case FolderCreationMode.Day:
                        folderName = DateTime.Today.ToString("yyyy-MM-dd");
                        break;
                    case FolderCreationMode.Hour:
                        folderName = DateTime.Today.ToString("yyyy-MM-dd-HH");
                        break;
                    case FolderCreationMode.Month:
                        folderName = DateTime.Today.ToString("yyyy-MM");
                        break;
                    case FolderCreationMode.Year:
                        folderName = DateTime.Today.ToString("yyyy");
                        break;
                    default:
                        break;
                }
                var baseDir = ConfigurationSettings.AppSettings["ErrorPath"].ToString(CultureInfo.InvariantCulture) + "\\" + folderName + "\\" + searchId;

                // search the file below the current directory
                string retFilePath = baseDir + "\\" + searchId + ".txt";

                // if exists, return the path
                if (File.Exists(retFilePath) == true)
                    return retFilePath;
                //create a text file
                else
                {
                    if (false == CheckDirectory(baseDir))
                        return string.Empty;

                    var fs = new FileStream(retFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }

                return retFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// For setting the log Path of the application
        /// </summary>
        /// <param name="folderCreationMode">Specifies the folder name format</param>
        /// <param name="providerType">Specifies provider type </param>
        /// <param name="searchId">SearchId for identifier.</param>
        /// <param name="suffix">File Name suffix</param>
        /// <returns></returns>
        internal static string GetXmlLogFilePath(FolderCreationMode folderCreationMode, string providerType, string searchId, string suffix)
        {
            try
            {

                string folderName = string.Empty;
                string retFilePath = String.Empty;

                // get the base directory
                switch (folderCreationMode)
                {
                    case FolderCreationMode.Day:
                        folderName = DateTime.Today.ToString("yyyy-MM-dd");
                        break;
                    case FolderCreationMode.Hour:
                        folderName = DateTime.Today.ToString("yyyy-MM-dd-HH");
                        break;
                    case FolderCreationMode.Month:
                        folderName = DateTime.Today.ToString("yyyy-MM");
                        break;
                    case FolderCreationMode.Year:
                        folderName = DateTime.Today.ToString("yyyy");
                        break;
                    default:
                        break;
                }
                var baseDir = ConfigurationSettings.AppSettings["LogPath"].ToString(CultureInfo.InvariantCulture) + "\\" + folderName + "\\" + searchId;

                // search the file below the current directory
                if (string.IsNullOrEmpty(suffix))
                {
                    retFilePath = string.Format("{0}\\{1}.xml", baseDir, providerType);
                }
                else
                {
                    retFilePath = baseDir + "\\" + providerType + "_" + suffix + ".xml";
                }


                // if exists, return the path
                if (File.Exists(retFilePath) == true)
                {
                    //Create New Version
                    if (retFilePath.LastIndexOf("-") > -1)
                    {
                        var FileName = retFilePath.Replace(".xml", "");
                        string strVersion = FileName.Substring(FileName.LastIndexOf("-") + 1);
                        if (IsNumber(strVersion))
                        {
                            int intVersion = Convert.ToInt32(strVersion) + 1;
                            retFilePath = FileName + "-" + intVersion.ToString() + ".xml";
                        }
                    }
                    else
                    {
                        var FileName = retFilePath.Replace(".xml", "");
                        retFilePath = FileName + "-1" + ".xml";
                    }

                    return retFilePath;
                }

                //create a text file
                else
                {
                    if (false == CheckDirectory(baseDir))
                        return string.Empty;

                    var fs = new FileStream(retFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }

                return retFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// For reading the log xml log path
        /// </summary>
        /// <param name="folderCreationMode">Specifies the folder name format</param>
        /// <param name="providerType">Specifies provider type </param>
        /// <param name="searchId">SearchId for identifier.</param>
        /// <param name="suffix">File Name suffix</param>
        /// <returns></returns>
        internal static string GetReadXmlLogFilePath(FolderCreationMode folderCreationMode, string providerType, string searchId, string suffix)
        {
            try
            {

                string folderName = string.Empty;
                string retFilePath = String.Empty;

                // get the base directory
                switch (folderCreationMode)
                {
                    case FolderCreationMode.Day:
                        folderName = DateTime.Today.ToString("yyyy-MM-dd");
                        break;
                    case FolderCreationMode.Hour:
                        folderName = DateTime.Today.ToString("yyyy-MM-dd-HH");
                        break;
                    case FolderCreationMode.Month:
                        folderName = DateTime.Today.ToString("yyyy-MM");
                        break;
                    case FolderCreationMode.Year:
                        folderName = DateTime.Today.ToString("yyyy");
                        break;
                    default:
                        break;
                }
                var baseDir = ConfigurationSettings.AppSettings["LogPath"].ToString(CultureInfo.InvariantCulture) + "\\" + folderName + "\\" + searchId;

                // search the file below the current directory
                if (string.IsNullOrEmpty(suffix))
                {
                    retFilePath = string.Format("{0}\\{1}.xml", baseDir, providerType);
                }
                else
                {
                    retFilePath = baseDir + "\\" + providerType + "_" + suffix + ".xml";
                }

                return retFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        /// <summary>
        ///Write / append exception
        /// </summary>
        internal static void WriteException(string path, string message)
        {
            try
            {
                //var sbError = new StringBuilder("URL: " + HttpContext.Current.Request.Url.ToString() + System.Environment.NewLine);
                var sbError = new StringBuilder(System.Environment.NewLine);
                sbError.Append(message.ToString(CultureInfo.InvariantCulture));
                sbError.Append(System.Environment.NewLine + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++" + System.Environment.NewLine);
                int intMessageLength = sbError.Length;

                //string strFilePath = System.Configuration.ConfigurationSettings.AppSettings["ErrorFilePath"];
                //strFilePath = strFilePath + "\\" + eModule + "\\" + eProduct + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString("00") + "\\" + DateTime.Now.Day.ToString("00");
                //DirectoryInfo objDirectInfo = new DirectoryInfo(strFilePath);
                //if (!objDirectInfo.Exists)
                //{
                //    objDirectInfo.Create();
                //}
                //strFilePath = strFilePath + "/Error.txt";
                System.IO.StreamWriter sw;
                sw = System.IO.File.AppendText(path);
                sw.WriteLine(sbError.ToString(0, intMessageLength));
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        /// <include file='summary.xml' path='Summary/Members[@name="SaveLogXml"]/*' />
        ///<param name="folderCreationMode">Specifies the folder creation type</param>
        /// <param name="mode">Specifies Product Mode</param>
        /// <param name="providerType">Specifies Provider type</param>
        /// <param name="searchId">SearchId for identifier.</param>
        ///<param name="suffix">Specifies for distinguish of xml</param>
        ///<param name="contents">Xml String</param>
        ///<returns> true if log was written successfully; false otherwise</returns>
        public static void SaveLogXml(FolderCreationMode folderCreationMode, ProductMode mode, ProviderType providerType, string searchId, string suffix, string contents)
        {
            try
            {
                if (string.IsNullOrEmpty(searchId))
                {
                    searchId = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "").Replace(":", "").Replace("AM", "").Replace("PM", "");
                }
                //var path = GetXmlLogFilePath(folderCreationMode, providerType.ToString(), searchId, suffix);

                var path = HttpContext.Current.Server.MapPath("~/Log.txt");
                if (contents != null) File.WriteAllText(path, contents);
            }
            catch (Exception ex)
            {

            }
        }

        /// <include file='summary.xml' path='Summary/Members[@name="SaveLogXml"]/*' />
        ///<param name="folderCreationMode">Specifies the folder creation type</param>
        /// <param name="mode">Specifies Product Mode</param>
        /// <param name="providerType">Specifies Provider type</param>
        /// <param name="searchId">SearchId for identifier.</param>
        ///<param name="suffix">Specifies for distinguish of xml</param>
        ///<param name="objType">The object that holds the serialized object data</param>
        ///<returns> true if log was written successfully; false otherwise</returns>
        public static void SaveLogXml(FolderCreationMode folderCreationMode, ProductMode mode, ProviderType providerType, string searchId, string suffix, object objType)
        {
            if (objType == null) throw new ArgumentNullException("objType");
            try
            {
                if (string.IsNullOrEmpty(searchId))
                {
                    searchId = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "").Replace(":", "").Replace("AM", "").Replace("PM", "");
                }
                var path = GetXmlLogFilePath(folderCreationMode, providerType.ToString(), searchId, suffix);
                File.WriteAllText(path, SerializeAnObject(objType));
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Read log xml string
        /// </summary>
        ///<param name="folderCreationMode">Specifies the folder creation type</param>
        /// <param name="mode">Specifies Product Mode</param>
        /// <param name="providerType">Specifies Provider type</param>
        /// <param name="searchId">SearchId for identifier.</param>
        ///<param name="suffix">Specifies for distinguish of xml</param>
        ///<returns>Log xml string</returns>
        public static string ReadLogXml(FolderCreationMode folderCreationMode, ProductMode mode, ProviderType providerType, string searchId, string suffix)
        {
            var strReadContents = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(searchId))
                {
                    searchId = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "").Replace(":", "").Replace("AM", "").Replace("PM", "");
                }
                var path = GetReadXmlLogFilePath(folderCreationMode, providerType.ToString(), searchId, suffix);
                if (File.Exists(path) == true)
                {
                    strReadContents = File.ReadAllText(path);
                }
                else
                {
                    strReadContents = "File not found";
                }
            }
            catch (Exception ex)
            { }
            return strReadContents;
        }

        /// <summary>
        /// Read object from log xml string in case if not found from MemCache or Memcahche failed
        /// </summary>
        ///<param name="folderCreationMode">Specifies the folder creation type</param>
        /// <param name="mode">Specifies Product Mode</param>
        /// <param name="providerType">Specifies Provider type</param>
        /// <param name="searchId">SearchId for identifier.</param>
        ///<param name="suffix">Specifies for distinguish of xml</param>
        ///<param name="type">Specified for the tgype of object to be retured</param>
        ///<returns>Object of the Type or null</returns>
        public static object GetLogXmlToObject(FolderCreationMode folderCreationMode, ProductMode mode, ProviderType providerType, string searchId, string suffix, Type type)
        {
            try
            {
                var strXml = ReadLogXml(folderCreationMode, mode, providerType, searchId, suffix);
                if (!string.IsNullOrEmpty(strXml) && strXml != "File not found")
                {
                    StringReader reader;
                    XmlSerializer serz = new XmlSerializer(type);
                    reader = new StringReader((string)strXml);
                    return serz.Deserialize(reader);
                }
            }
            catch (Exception ex)
            { WriteErrorLog(ex, folderCreationMode, mode, ErrorMode.Normal, searchId, "BMT", "Utilities"); }
            return null;
        }


        /// <summary>
        /// Serialize object
        /// </summary>
        internal static string SerializeAnObject(object anObject)
        {
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            using (var stringWriter = new StringWriter())
            {
                stringWriter.NewLine = "";
                var writer = XmlWriter.Create(stringWriter, settings);
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                var xs = new XmlSerializer(anObject.GetType());
                xs.Serialize(writer, anObject, namespaces);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Number Validation With Regular Expression
        /// </summary>
        /// <param name="inputvalue">Specifies the string</param>
        internal static bool IsNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }

        public static void SaveLog(String message)
        {
            try
            {
                DeleteLog();
                var path = HttpContext.Current.Server.MapPath("~/LogFiles/Log_" + DateTime.Now.ToString("ddMMMyyyy") + ".txt");
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }

                if (message != null)
                    File.AppendAllText(path, message + Environment.NewLine);

            }
            catch (Exception)
            {

            }
        }
        public static void Log(string typeName, string methodName, string ex)
        {
            try
            {
                String exceptionMessage = DateTime.Now.ToString("HH:mm:ss tt") + Environment.NewLine + "Source : " + typeName + Environment.NewLine + "Method : " + methodName + Environment.NewLine + "Exception : " + ex + Environment.NewLine + Environment.NewLine + "===================================================================================================";
                var path = HttpContext.Current.Server.MapPath("~/LogFiles/Log_" + DateTime.Now.ToString("ddMMMyyyy") + ".txt");
                if (exceptionMessage != null)
                {
                    File.AppendAllText(path, exceptionMessage + Environment.NewLine);
                }
            }
            catch (Exception)
            {

            }
        }


        public static void SaveErrorLog(string Source, string Method, Exception ex)
        {
            //string LogPath = ConfigurationManager.AppSettings["LogPath"].ToString();
            //string fileName = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            //string filePath = LogPath + fileName;
            var path = HttpContext.Current.Server.MapPath("~/LogFiles/Log_" + DateTime.Now.ToString("ddMMMyyyy") + ".txt");

            var type = ex.GetType();
            var errorLineNo = ex.LineNumber();

            try
            {
                if (File.Exists(path))
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss tt"));
                        writer.WriteLine("Source : " + Source);
                        writer.WriteLine("Method Name : " + Method);
                        writer.WriteLine("Error Line No : " + errorLineNo);
                        writer.WriteLine("Exception Type : " + type);
                        writer.WriteLine("Details: " + ex.Message);
                        writer.WriteLine("---------------------------------------------------------------------------------------------------------------------------");
                        writer.WriteLine();
                        writer.Close();
                    }
                }
                else
                {
                    StreamWriter writer = File.CreateText(path);
                    writer.WriteLine(DateTime.Now.ToString("HH:mm:ss tt"));
                    writer.WriteLine("Source : " + Source);
                    writer.WriteLine("Method Name : " + Method);
                    writer.WriteLine("Error Line No : " + errorLineNo);
                    writer.WriteLine("Exception Type : " + type);
                    writer.WriteLine("Details: " + ex.Message);
                    writer.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                    writer.WriteLine();
                    writer.Close();
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public static void SaveErrorLog(string Source, string Method, string ex)
        {
            var path = HttpContext.Current.Server.MapPath("~/LogFiles/Log_" + DateTime.Now.ToString("ddMMMyyyy") + ".txt");

            var type = ex.GetType();
            var errorLineNo = 1;

            try
            {
                if (File.Exists(path))
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss tt"));
                        writer.WriteLine("Source : " + Source);
                        writer.WriteLine("Method Name : " + Method);
                        writer.WriteLine("Error Line No : " + errorLineNo);
                        writer.WriteLine("Exception Type : " + type);
                        writer.WriteLine("Details: " + ex);
                        writer.WriteLine("---------------------------------------------------------------------------------------------------------------------------");
                        writer.WriteLine();
                        writer.Close();
                    }
                }
                else
                {
                    StreamWriter writer = File.CreateText(path);
                    writer.WriteLine(DateTime.Now.ToString("HH:mm:ss tt"));
                    writer.WriteLine("Source : " + Source);
                    writer.WriteLine("Method Name : " + Method);
                    writer.WriteLine("Error Line No : " + errorLineNo);
                    writer.WriteLine("Exception Type : " + type);
                    writer.WriteLine("Details: " + ex);
                    writer.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                    writer.WriteLine();
                    writer.Close();
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public static int LineNumber(this Exception e)
        {

            int linenum = 0;
            try
            {

                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

            }
            catch
            {

                //Stack trace is not available!

            }

            return linenum;

        }

        public static void DeleteLog()
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath("~/LogFiles/Log_" + DateTime.Now.AddDays(-10).ToString("ddMMMyyyy") + ".txt");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception)
            {

            }
        }
        public static void DeleteVahaanLog(String fileName)
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath("~/Content/VahaanLogFiles/" + fileName + ".txt");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception)
            {

            }
        }
        public static void SaveVahaanLog(String message,String fileName)
        {
            try
            {
                DeleteVahaanLog(fileName);
                var path = HttpContext.Current.Server.MapPath("~/Content/VahaanLogFiles/" + fileName + ".txt");
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }

                if (message != null)
                    File.AppendAllText(path, message + Environment.NewLine);
            }
            catch (Exception)
            {

            }
        }
    }
}
