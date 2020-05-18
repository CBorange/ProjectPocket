using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ErrorLogWriter
{
    // Singleton
    private ErrorLogWriter()
    {
            
    }
    private static ErrorLogWriter instance;
    public static ErrorLogWriter Instance
    {
        get
        {
            if (instance == null)
                instance = new ErrorLogWriter();
            return instance;
        }
    }

    public void WriteErrorLog(string str, string filename)
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + @"error.txt");
        sw.Write(str);
        sw.Close();
    }
}
