using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2b
{
    public interface IJsonSerializable
    {
        bool LoadJSON(string path);
        bool SaveAsJSON(string path);
    }
}