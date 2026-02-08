using System;
using System.Collections.Generic;
using System.Text;

namespace WeaponLib
{
    public interface IJsonSerializable
    {
        bool LoadJSON(string path);
        bool SaveAsJSON(string path);
    }
}