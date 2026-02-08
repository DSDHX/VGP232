using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Assignment2b
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection WeaponCollection;
        private string inputPath;

        const string INPUT_FILE = "data2.csv";
        const string CSV_OUT = "weapons.csv";
        const string JSON_OUT = "weapons.json";
        const string XML_OUT = "weapons.xml";
        const string EMPTY_CSV = "empty.csv";
        const string EMPTY_JSON = "empty.json";
        const string EMPTY_XML = "empty.xml";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            WeaponCollection = new WeaponCollection();

            if (File.Exists(inputPath))
            {
                WeaponCollection.Load(inputPath);
            }
        }

        [TearDown]
        public void CleanUp()
        {
            string[] filesToDelete = { CSV_OUT, JSON_OUT, XML_OUT, EMPTY_CSV, EMPTY_JSON, EMPTY_XML };

            foreach (var file in filesToDelete)
            {
                string path = CombineToAppPath(file);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        // ==========================================
        //  Test LoadJson Valid (4 Tests)
        // ==========================================

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            string jsonPath = CombineToAppPath(JSON_OUT);
            Assert.That(WeaponCollection.Save(jsonPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.Load(jsonPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            string jsonPath = CombineToAppPath(JSON_OUT);
            Assert.That(WeaponCollection.SaveAsJSON(jsonPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.Load(jsonPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            string jsonPath = CombineToAppPath(JSON_OUT);
            Assert.That(WeaponCollection.SaveAsJSON(jsonPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.LoadJSON(jsonPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            string jsonPath = CombineToAppPath(JSON_OUT);
            Assert.That(WeaponCollection.Save(jsonPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.LoadJSON(jsonPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        // ==========================================
        //  Test LoadCsv Valid (2 Tests)
        // ==========================================

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            string csvPath = CombineToAppPath(CSV_OUT);
            Assert.That(WeaponCollection.Save(csvPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.Load(csvPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            string csvPath = CombineToAppPath(CSV_OUT);
            Assert.That(WeaponCollection.SaveAsCSV(csvPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.LoadCSV(csvPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        // ==========================================
        //  Test LoadXML Valid (2 Tests)
        // ==========================================

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            string xmlPath = CombineToAppPath(XML_OUT);
            Assert.That(WeaponCollection.Save(xmlPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.Load(xmlPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            string xmlPath = CombineToAppPath(XML_OUT);
            Assert.That(WeaponCollection.SaveAsXML(xmlPath), Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.LoadXML(xmlPath), Is.True);

            Assert.That(newCollection.Count, Is.EqualTo(95));
        }

        // ==========================================
        //  Test Save Empty (3 Tests)
        // ==========================================

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            WeaponCollection emptyCol = new WeaponCollection();
            string path = CombineToAppPath(EMPTY_JSON);

            Assert.That(emptyCol.SaveAsJSON(path), Is.True);

            WeaponCollection loadedCol = new WeaponCollection();
            Assert.That(loadedCol.Load(path), Is.True);
            Assert.That(loadedCol.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            WeaponCollection emptyCol = new WeaponCollection();
            string path = CombineToAppPath(EMPTY_CSV);

            Assert.That(emptyCol.SaveAsCSV(path), Is.True);

            WeaponCollection loadedCol = new WeaponCollection();
            Assert.That(loadedCol.Load(path), Is.True);
            Assert.That(loadedCol.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            WeaponCollection emptyCol = new WeaponCollection();
            string path = CombineToAppPath(EMPTY_XML);

            Assert.That(emptyCol.SaveAsXML(path), Is.True);

            WeaponCollection loadedCol = new WeaponCollection();
            Assert.That(loadedCol.Load(path), Is.True);
            Assert.That(loadedCol.Count, Is.EqualTo(0));
        }

        // ==========================================
        //  Test Load InvalidFormat (4 Tests)
        // ==========================================

        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            string jsonPath = CombineToAppPath(JSON_OUT);
            WeaponCollection.SaveAsJSON(jsonPath);

            WeaponCollection newCol = new WeaponCollection();
            bool result = newCol.LoadXML(jsonPath);

            Assert.That(result, Is.False);
            Assert.That(newCol.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            string xmlPath = CombineToAppPath(XML_OUT);
            WeaponCollection.SaveAsXML(xmlPath);

            WeaponCollection newCol = new WeaponCollection();
            bool result = newCol.LoadJSON(xmlPath);

            Assert.That(result, Is.False);
            Assert.That(newCol.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            WeaponCollection newCol = new WeaponCollection();
            bool result = newCol.LoadXML(inputPath);

            Assert.That(result, Is.False);
            Assert.That(newCol.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            WeaponCollection newCol = new WeaponCollection();
            bool result = newCol.LoadJSON(inputPath);

            Assert.That(result, Is.False);
            Assert.That(newCol.Count, Is.EqualTo(0));
        }

        // ==========================================
        //  Existing Tests (Retained for completeness)
        // ==========================================

        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            int actual = WeaponCollection.GetHighestBaseAttack();
            Assert.That(actual, Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            int actual = WeaponCollection.GetLowestBaseAttack();
            Assert.That(actual, Is.EqualTo(23));
        }

        [TestCase(Weapon.WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(Weapon.WeaponType type, int expectedValue)
        {
            List<Weapon> result = WeaponCollection.GetAllWeaponsOfType(type);
            Assert.That(result.Count, Is.EqualTo(expectedValue));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            List<Weapon> result = WeaponCollection.GetAllWeaponsOfRarity(stars);
            Assert.That(result.Count, Is.EqualTo(expectedValue));
        }
    }
}