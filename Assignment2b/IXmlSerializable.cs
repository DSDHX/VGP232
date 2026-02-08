using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2b
{
    public interface IXmlSerializable
    {
        bool LoadXML(string path);
        bool SaveAsXML(string path);
    }
}