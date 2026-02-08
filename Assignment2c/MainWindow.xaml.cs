using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WeaponLib;

namespace Assignment2c
{
    public partial class MainWindow : Window
    {
        private WeaponCollection mWeaponCollection;

        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();

            WeaponListBox.ItemsSource = mWeaponCollection;

            PopulateTypeComboBox();
        }

        private void PopulateTypeComboBox()
        {
            var types = new List<string>();
            types.Add("All"); // Add "All" option
            types.AddRange(Enum.GetNames(typeof(Weapon.WeaponType)));

            FilterTypeComboBox.ItemsSource = types;
            FilterTypeComboBox.SelectedIndex = 0; // Set default selection to "All"
        }

        // Helper function to apply both filters (Type and Name)
        private void RefreshWeaponList()
        {
            if (mWeaponCollection == null)
            {
                return;
            }

            IEnumerable<Weapon> filteredList = mWeaponCollection;

            if (FilterTypeComboBox.SelectedItem != null)
            {
                string selectedType = FilterTypeComboBox.SelectedItem.ToString();
                if (selectedType != "All" && Enum.TryParse(selectedType, out Weapon.WeaponType typeEnum))
                {
                    // Better to use LINQ
                    filteredList = mWeaponCollection.GetAllWeaponsOfType(typeEnum);
                }
            }

            string filterText = FilterNameTextBox.Text;
            if (!string.IsNullOrEmpty(filterText))
            {
                filteredList = filteredList.Where(w => w.Name != null && 
                                                       w.Name.StartsWith(filterText, StringComparison.OrdinalIgnoreCase));
            }

            WeaponListBox.ItemsSource = filteredList.ToList();
        }

        // ============================
        // Event Handlers
        // ============================

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files|*.csv|JSON Files|*.json|XML Files|*.xml|All Files|*.*";


            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    mWeaponCollection.Load(openFileDialog.FileName);
                    RefreshWeaponList();
                    MessageBox.Show("Loaded successfully!", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error");
                }
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv|JSON Files|*.json|XML Files|*.xml";
            saveFileDialog.FileName = "weapons";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    mWeaponCollection.Save(saveFileDialog.FileName);
                    MessageBox.Show("Saved successfully!", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error");
                }
            }
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.IsChecked == true)
            {
                if (mWeaponCollection == null)
                {
                    return;
                }
                string sortBy = rb.Tag.ToString();
                mWeaponCollection.SortBy(sortBy);
                RefreshWeaponList();
            }
        }

        private void FilterTypeOnlySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshWeaponList();
        }

        private void FilterNameTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshWeaponList();
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            Weapon selectedWeapon = WeaponListBox.SelectedItem as Weapon;
            if (selectedWeapon != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedWeapon.Name}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    mWeaponCollection.Remove(selectedWeapon);
                    RefreshWeaponList();
                }
            }
            else
            {
                MessageBox.Show("Please select a weapon to remove.", "Info");
            }
        }

        // Placeholder for EditWeaponWindow
        private void AddClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Uncomment the code below when you implement EditWeaponWindow in the next assignment part.

            EditWeaponWindow addWindow = new EditWeaponWindow(); // Add mode (default constructor)

            if (addWindow.ShowDialog() == true) // Check if DialogResult is true (Save clicked)
            {
                if (addWindow.TempWeapon != null)
                {
                    mWeaponCollection.Add(addWindow.TempWeapon);
                    RefreshWeaponList();
                }
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            Weapon selectedWeapon = WeaponListBox.SelectedItem as Weapon;
            if (selectedWeapon == null)
            {
                MessageBox.Show("Please select a weapon to edit.", "Info");
                return;
            }

            // TODO: Uncomment the code below when you implement EditWeaponWindow

            // Pass the selected weapon to the window
            EditWeaponWindow editWindow = new EditWeaponWindow(selectedWeapon);

            if (editWindow.ShowDialog() == true)
            {
                // Ensure the list updates (since the object reference might be modified directly, 
                // refreshing the list forces the UI to redraw the text)
                RefreshWeaponList();
            }
        }
    }
}