
using System;
using System.IO;
namespace UnityEngine
{
    /// <summary>
    /// 系统日志模块
    /// </summary>
    public class Debugger
    {
        public static bool EnableLog;           // 是否启用日志，仅可控制普通级别的日志的启用与关闭，LogError和LogWarn都是始终启用的。
        public static bool EnableTime = true;
        public static bool EnableSave = false;  // 是否允许保存日志，即把日志写入到文件中
        public static bool EnableStack = false;
        public static string LogFileDir = Application.persistentDataPath + "/DebuggerLog/";
        public static string LogFileName = "";
        public static string Prefix = ">";     // 用于与Unity默认的系统日志做区分。本日志系统输出的日志头部都会带上这个标记。
        public static string Nextfix = "<";     // 用于与Unity默认的系统日志做区分。本日志系统输出的日志头部都会带上这个标记。
        public static StreamWriter LogFileWriter = null;
        public static bool UseUnityEngine;

        private static string GetLogText(string tag, string message)
        {
            string str = "";
            if (EnableTime)
            {
                str = DateTime.Now.ToString("HH:mm:ss.fff") + " ";
            }
            return (str + tag + "::" + message);
        }

        private static string GetLogTime()
        {
            string str = "";
            if (EnableTime)
            {
                str = DateTime.Now.ToString("HH:mm:ss.fff") + " ";
            }
            return Nextfix + Nextfix + str + Prefix + Prefix;
        }

        public static void Log(object message)
        {
            if (!Debugger.EnableLog)
                return;

            string str = GetLogTime() + message;
            Debug.Log(str, null);
            LogToFile("[I]" + str, false);
        }

        public static void LogFomat(object message, params object[] args)
        {
            string strformat = string.Format(message.ToString(), args);
            if (!Debugger.EnableLog)
                return;

            string str = GetLogTime() + strformat;
            Debug.Log(str, null);
            LogToFile("[I]" + str, false);
        }

        public static void Log(object message, Object context)
        {
            if (!Debugger.EnableLog)
                return;

            string str = GetLogTime() + message;
            Debug.Log(Prefix + str, context);
            LogToFile("[I]" + str, false);
        }

        public static void LogFomat(object message, Object context, params object[] args)
        {
            string strformat = string.Format(message.ToString(), args);
            if (!Debugger.EnableLog)
                return;

            string str = GetLogTime() + strformat;
            Debug.Log(str, context);
            LogToFile("[I]" + str, false);
        }

        public static void Log(string tag, string message)
        {
            if (!Debugger.EnableLog)
                return;

            message = GetLogText(tag, message);
            Debug.Log(message, null);
            LogToFile("[I]" + message, false);
        }

        public static void LogFomat(string tag, string format, params object[] args)
        {
            if (!Debugger.EnableLog)
                return;

            string logText = GetLogText(tag, string.Format(format, args));
            Debug.Log(logText, null);
            LogToFile("[I]" + logText, false);
        }

        public static void LogError(object message)
        {
            string str = GetLogTime() + message;
            Debug.LogError(str, null);
            LogToFile("[E]" + str, true);
        }

        public static void LogErrorFormat(object message, params object[] args)
        {
            string str = GetLogTime() + string.Format(message.ToString(), args);
            Debug.LogError(str, null);
            LogToFile("[E]" + str, true);
        }

        public static void LogError(object message, Object context)
        {
            string str = GetLogTime() + message;
            Debug.LogError(str, context);
            LogToFile("[E]" + str, true);
        }

        public static void LogErrorFormat(object message, Object context, params object[] args)
        {
            string str = GetLogTime() + string.Format(message.ToString(), args);
            Debug.LogError(str, context);
            LogToFile("[E]" + str, true);
        }

        public static void LogError(string tag, string message)
        {
            message = GetLogText(tag, message);
            Debug.LogError(message, null);
            LogToFile("[E]" + message, true);
        }

        public static void LogErrorFormat(string tag, string format, params object[] args)
        {
            string logText = GetLogText(tag, string.Format(format, args));
            Debug.LogError(logText, null);
            LogToFile("[E]" + logText, true);
        }

        /// <summary>
        /// 将日志写入到文件中
        /// </summary>
        /// <param name="message"></param>
        /// <param name="EnableStack"></param>
        private static void LogToFile(string message, bool EnableStack = false)
        {
            if (!Debugger.EnableSave)
                return;

            if (LogFileWriter == null)
            {
                LogFileName = DateTime.Now.GetDateTimeFormats('s')[0].ToString();
                LogFileName = LogFileName.Replace("-", "_");
                LogFileName = LogFileName.Replace(":", "_");
                LogFileName = LogFileName.Replace(" ", "");
                LogFileName = LogFileName + ".log";
                if (string.IsNullOrEmpty(LogFileDir))
                {
                    try
                    {
                        if (UseUnityEngine)
                        {
                            LogFileDir = Application.persistentDataPath + "/DebuggerLog/";
                        }
                        else
                        {
                            LogFileDir = AppDomain.CurrentDomain.BaseDirectory + "/DebuggerLog/";
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.Log(Prefix + "获取 Application.persistentDataPath 报错！" + exception.Message, null);
                        return;
                    }
                }
                string path = LogFileDir + LogFileName;
                try
                {
                    if (!Directory.Exists(LogFileDir))
                    {
                        Directory.CreateDirectory(LogFileDir);
                    }
                    LogFileWriter = File.AppendText(path);
                    LogFileWriter.AutoFlush = true;
                }
                catch (Exception exception2)
                {
                    LogFileWriter = null;
                    Debug.Log("LogToCache() " + exception2.Message + exception2.StackTrace, null);
                    return;
                }
            }
            if (LogFileWriter != null)
            {
                try
                {
                    LogFileWriter.WriteLine(message);
                    if ((EnableStack || Debugger.EnableStack) && UseUnityEngine)
                    {
                        LogFileWriter.WriteLine(StackTraceUtility.ExtractStackTrace());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void LogWarning(object message)
        {
            string str = GetLogTime() + message;
            Debug.LogWarning(str, null);
            LogToFile("[W]" + str, false);
        }

        public static void LogWarningFormat(object message, params object[] args)
        {
            string str = GetLogTime() + string.Format(message.ToString(), args);
            Debug.LogWarning(str, null);
            LogToFile("[W]" + str, false);
        }

        public static void LogWarning(object message, Object context)
        {
            string str = GetLogTime() + message;
            Debug.LogWarning(str, context);
            LogToFile("[W]" + str, false);
        }

        public static void LogWarningFormat(object message, Object context, params object[] args)
        {
            string str = GetLogTime() + string.Format(message.ToString(), args);
            Debug.LogWarning(str, context);
            LogToFile("[W]" + str, false);
        }

        public static void LogWarning(string tag, string message)
        {
            message = GetLogText(tag, message);
            Debug.LogWarning(message, null);
            LogToFile("[W]" + message, false);
        }

        public static void LogWarningFormat(string tag, string format, params object[] args)
        {
            string logText = GetLogText(tag, string.Format(format, args));
            Debug.LogWarning(logText, null);
            LogToFile("[W]" + logText, false);
        }

    }

}
