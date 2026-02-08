using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2b
{
    public interface ICsvSerializable
    {
        bool LoadCSV(string path);
        bool SaveAsCSV(string path);
    }
}