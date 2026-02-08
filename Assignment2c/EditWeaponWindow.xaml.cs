using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WeaponLib;

namespace Assignment2c
{
    public partial class EditWeaponWindow : Window
    {
        public Weapon TempWeapon { get; set; }

        // true = Edit, false = Add
        private bool _isEditMode = false;

        // ============================
        // Constructors
        // ============================

        // Constructor for ADD mode
        public EditWeaponWindow()
        {
            InitializeComponent();
            
            TempWeapon = new Weapon();
            _isEditMode = false;

            SetUp();
        }

        // Constructor for EDIT mode
        public EditWeaponWindow(Weapon weaponToEdit)
        {
            InitializeComponent();

            TempWeapon = weaponToEdit;
            _isEditMode = true;

            SetUp();
        }

        // ============================
        // Core Logic
        // ============================

        private void SetUp()
        {
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(Weapon.WeaponType));

            RarityComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };

            if (_isEditMode)
            {
                // --- Edit Mode ---
                this.Title = "Edit Weapon";
                SubmitButton.Content = "Save";

                NameTextBox.Text = TempWeapon.Name;
                TypeComboBox.SelectedItem = TempWeapon.Type;
                ImageUrlTextBox.Text = TempWeapon.Image;
                RarityComboBox.SelectedItem = TempWeapon.Rarity;
                BaseAttackTextBox.Text = TempWeapon.BaseAttack.ToString();
                SecondaryStatTextBox.Text = TempWeapon.SecondaryStat;
                PassiveTextBox.Text = TempWeapon.Passive;

                UpdateImagePreview(TempWeapon.Image);
            }
            else
            {
                // --- Add Mode ---
                this.Title = "Add Weapon";
                SubmitButton.Content = "Add";

                TypeComboBox.SelectedIndex = 0;
                RarityComboBox.SelectedIndex = 0;
            }
        }

        private void UpdateImagePreview(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                WeaponImagePreview.Source = bitmap;
            }
            catch
            {
                WeaponImagePreview.Source = null;
            }
        }

        // ============================
        // Event Handlers
        // ============================

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(BaseAttackTextBox.Text, out int baseAttack))
            {
                MessageBox.Show("Base Attack must be a valid number.", "Validation Error");
                return;
            }

            TempWeapon.Name = NameTextBox.Text;

            if (TypeComboBox.SelectedItem != null)
            {
                TempWeapon.Type = (Weapon.WeaponType)TypeComboBox.SelectedItem;
            }

            TempWeapon.Image = ImageUrlTextBox.Text;

            if (RarityComboBox.SelectedItem != null)
            {
                TempWeapon.Rarity = (int)RarityComboBox.SelectedItem;
            }

            TempWeapon.BaseAttack = baseAttack;
            TempWeapon.SecondaryStat = SecondaryStatTextBox.Text;
            TempWeapon.Passive = PassiveTextBox.Text;

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // BONUS: Generate Random Values
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();

            int randomAttack = rand.Next(20, 51);
            BaseAttackTextBox.Text = randomAttack.ToString();

            int randomRarity = rand.Next(1, 6);
            RarityComboBox.SelectedItem = randomRarity;

            Array values = Enum.GetValues(typeof(Weapon.WeaponType));
            Weapon.WeaponType randomType = (Weapon.WeaponType)values.GetValue(rand.Next(values.Length));
            TypeComboBox.SelectedItem = randomType;
        }

        private void ImageUrlTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateImagePreview(ImageUrlTextBox.Text);
        }
    }
}