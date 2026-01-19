using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection WeaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            WeaponCollection = new WeaponCollection();

            if (File.Exists(inputPath))
            {
                WeaponCollection.Load(inputPath);
            }
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            // Expected Value: 48
            // TODO: call WeaponCollection.GetHighestBaseAttack()
            // and confirm that it matches the expected value using asserts. (DONE)
            int actual = WeaponCollection.GetHighestBaseAttack();
            Assert.That(actual, Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            // Expected Value: 23
            // TODO: call WeaponCollection.GetLowestBaseAttack()
            // and confirm that it matches the expected value using asserts. (DONE)
            int actual = WeaponCollection.GetLowestBaseAttack();
            Assert.That(actual, Is.EqualTo(23));
        }

        [TestCase(Weapon.WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(Weapon.WeaponType type, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfType(type)
            // and confirm that the weapons list returns Count matches the expected value using asserts. (DONE)
            List<Weapon> result = WeaponCollection.GetAllWeaponsOfType(type);
            Assert.That(result.Count, Is.EqualTo(expectedValue));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars)
            // and confirm that the weapons list returns Count matches the expected value using asserts. (DONE)
            List<Weapon> result = WeaponCollection.GetAllWeaponsOfRarity(stars);
            Assert.That(result.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            // TODO: load returns true, expect WeaponCollection with count of 95. (DONE)
            bool result = WeaponCollection.Load(inputPath);
            Assert.That(result, Is.True);
            Assert.That(WeaponCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            // TODO: load returns false, expect an empty WeaponCollection (DONE)
            bool result = WeaponCollection.Load("Does_Not_Exist.csv");
            Assert.That(result, Is.False);
            Assert.That(WeaponCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty. (DONE)
            if (WeaponCollection.Count == 0)
            {
                WeaponCollection.Load(inputPath);
            }

            bool saveResult = WeaponCollection.Save(outputPath);
            Assert.That(saveResult, Is.True);

            WeaponCollection newCollection = new WeaponCollection();
            bool loadResult = newCollection.Load(outputPath);

            Assert.That(loadResult, Is.True);
            Assert.That(newCollection.Count, Is.EqualTo(WeaponCollection.Count));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.Save(outputPath));
            Assert.That(WeaponCollection.Load(outputPath));
            Assert.That(WeaponCollection.Count == 0);
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            // TODO: uncomment this once you added the Type1 and Type2
            Weapon expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = Weapon.WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented. (DONE)
            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive (DONE)
            bool result = Weapon.TryPrase(line, out actual);

            Assert.That(result, Is.True);
            Assert.That(actual, Is.Not.True);
            Assert.That(expected.Name, Is.EqualTo(actual.Name));
            Assert.That(expected.Type, Is.EqualTo(actual.Type));
            Assert.That(expected.Image, Is.EqualTo(actual.Image));
            Assert.That(expected.Rarity, Is.EqualTo(actual.Rarity));
            Assert.That(expected.BaseAttack, Is.EqualTo(actual.BaseAttack));
            Assert.That(expected.SecondaryStat, Is.EqualTo(actual.SecondaryStat));
            Assert.That(expected.Passive, Is.EqualTo(actual.Passive));
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null. (DONE)
            string badLine = "1,Bulbasaur,A,B,C,65,65";
            bool result = Weapon.TryPrase(badLine, out Weapon actual);

            Assert.That(result, Is.False);
            Assert.That(actual, Is.Null);
        }
    }
}