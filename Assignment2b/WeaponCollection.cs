using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Assignment2b
{
    public class WeaponCollection : List<Weapon>, IPeristence, IXmlSerializable, IJsonSerializable, ICsvSerializable
    {
        // ===========================
        // CSV Implementation
        // ===========================
        public bool LoadCSV(string filename)
        {
            this.Clear();
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                return false;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string header = reader.ReadLine();

                    if (header == null)
                    {
                        return true;
                    }

                    while (reader.Peek() > -1)
                    {
                        string line = reader.ReadLine();
                        if (Weapon.TryPrase(line, out Weapon weapon))
                        {
                            this.Add(weapon);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV: {ex.Message}");
                return false;
            }
        }

        public bool SaveAsCSV(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("Name, Type, Image, Rarity, BaseAttack, SecondaryStat, Passive");
                    foreach (var weapon in this)
                    {
                        writer.WriteLine(weapon.ToString());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving CSV: {ex.Message}");
                return false;
            }
        }

        // ===========================
        // XML Implementation
        // ===========================
        public bool LoadXML(string filename)
        {
            this.Clear();
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    List<Weapon> loadedWeapons = (List<Weapon>)serializer.Deserialize(fs);
                    this.AddRange(loadedWeapons);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML: {ex.Message}");
                return false;
            }
        }

        public bool SaveAsXML(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));
                using (TextWriter writer = new StreamWriter(filename))
                {
                    List<Weapon> list = new List<Weapon>(this);
                    serializer.Serialize(writer, list);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving XML: {ex.Message}");
                return false;
            }
        }

        // ===========================
        // JSON Implementation
        // ===========================
        public bool LoadJSON(string filename)
        {
            this.Clear();
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                return false;
            }

            try
            {
                string jsonString = File.ReadAllText(filename);
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return true;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                List<Weapon> loadedWeapons = JsonSerializer.Deserialize<List<Weapon>>(jsonString, options);
                if (loadedWeapons != null)
                {
                    this.AddRange(loadedWeapons);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
                return false;
            }
        }

        public bool SaveAsJSON(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                string jsonString = JsonSerializer.Serialize((List<Weapon>)this, options);
                File.WriteAllText(filename, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving JSON: {ex.Message}");
                return false;
            }
        }

        // ===========================
        // General Load/Save (Dispatcher)
        // ===========================
        public bool Load(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            string extension = Path.GetExtension(filename).ToLower();

            switch (extension)
            {
                case ".csv":
                    return LoadCSV(filename);
                case ".xml":
                    return LoadXML(filename);
                case ".json":
                    return LoadJSON(filename);
                default:
                    throw new ArgumentException($"Unknown file format: {extension}");
            }
        }

        public bool Save(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            string extension = Path.GetExtension(filename).ToLower();

            switch (extension)
            {
                case ".csv":
                    return SaveAsCSV(filename);
                case ".xml":
                    return SaveAsXML(filename);
                case ".json":
                    return SaveAsJSON(filename);
                default:
                    throw new ArgumentException($"Unknown file format: {extension}");
            }
        }

        // ===========================
        // Helper Methods (Existing)
        // ===========================
        public int GetHighestBaseAttack()
        {
            if (this.Count == 0)
            {
                return 0;
            }
            int max = this[0].BaseAttack;
            foreach (var w in this)
            {
                if (w.BaseAttack > max)
                {
                    max = w.BaseAttack;
                }
            }
            return max;
        }

        public int GetLowestBaseAttack()
        {
            if (this.Count == 0)
            {
                return 0;
            }
            int min = this[0].BaseAttack;
            foreach (var w in this)
            {
                if (w.BaseAttack < min)
                {
                    min = w.BaseAttack;
                }
            }
            return min;
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            List<Weapon> filtered = new List<Weapon>();
            foreach (var w in this)
            {
                if (w.Type == type)
                {
                    filtered.Add(w);
                }
            }
            return filtered;
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            List<Weapon> filtered = new List<Weapon>();
            foreach (var w in this)
            {
                if (w.Rarity == stars)
                {
                    filtered.Add(w);
                }
            }
                return filtered;
        }

        public void SortBy(string columName)
        {
            if (string.IsNullOrEmpty(columName))
            {
                this.Sort(Weapon.CompareByName);
                return;
            }

            switch (columName.ToLower())
            {
                case "type":
                    this.Sort(Weapon.CompareByType);
                    break;
                case "rarity":
                    this.Sort(Weapon.CompareByRarity);
                    break;
                case "baseattack":
                    this.Sort(Weapon.CompareByBaseAttack);
                    break;
                case "name":
                default:
                    this.Sort(Weapon.CompareByName);
                    break;
            }
        }
    }
}