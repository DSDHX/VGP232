using System.Collections.Generic;

namespace Assignment3
{
    public class SpriteSheetProject
    {
        public string OutputDirectory { get; set; }
        public string OutputFile { get; set; }
        public int Columns { get; set; }
        public bool IncludeMetaData { get; set; }
        public List<string> ImagePaths { get; set; } = new List<string>();
    }
}