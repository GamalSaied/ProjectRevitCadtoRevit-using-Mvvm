﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjectRevitFinal.Api;
using ProjectRevitFinal.Api.EventHandlers;
using ProjectRevitFinal.Domain;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.ViewModel;
using ProjectRevitFinalApp.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectRevitFinal.View
{
    /// <summary>
    /// Interaction logic for Levels.xaml
    /// </summary>
    public partial class Levels : UserControl, INotifyPropertyChanged
    {
        private static Levels _GetData;
        public static Levels GetData { get => _GetData; internal set => _GetData = value; }

        private BasePointType _selectedBasePointType;

        private ExternalEvent _createLevelExternalEvent;

        private readonly CreateLevelEventHandler _createLevelEventHandler;

        private ExternalEvent _deleteLevelExternalEvent;

        private readonly DeleteLevelEventHandler _deleteLevelEventHandler;
        private ObservableCollection<LevelModel> LevelList { get; }



            public Levels (List<LevelModel> levelList, CreateLevelEventHandler createLevelEventHandler, DeleteLevelEventHandler deleteLevelEventHandler, ExternalEvent createLevelExternalEvent, ExternalEvent deleteLevelExternalEvent)
            {
 
            _GetData = this;
            InitializeComponent();
            LevelList = new ObservableCollection<LevelModel>(levelList);
            LevelsDataGrid.ItemsSource = LevelList;
            SelectedBasePointType = BasePointTypeEnumValues.FirstOrDefault();

            // Set the initial state of the button
            IsAddLevelButtonEnabled = false;

            // Bind the button's IsEnabled property to the IsAddLevelButtonEnabled property
            AddLevelButton.SetBinding(Button.IsEnabledProperty, new System.Windows.Data.Binding("IsAddLevelButtonEnabled") { Source = this });

            // Attach the PreviewTextInput event handler to the ElevationTextBox
            ElevationTextBox.PreviewTextInput += ElevationTextBox_PreviewTextInput;

            // Set the MaxLength property of NameTextBox to 50 characters
            NameTextBox.MaxLength = 100;
            _createLevelEventHandler = createLevelEventHandler;
            _deleteLevelEventHandler = deleteLevelEventHandler;
            _createLevelExternalEvent = createLevelExternalEvent;
            _deleteLevelExternalEvent = deleteLevelExternalEvent;

        }

        private void AddLevelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(ElevationTextBox.Text))
            {
                MessageBox.Show("Level data not valid");
                return;
            }

            if (LevelList != null && LevelList.Any(l => l.Name == NameTextBox.Text))
            {
                MessageBox.Show("Level name already exists");
                return;
            }


            try
            {
                var level = new LevelModel(
                    NameTextBox.Text,
                    new Elevation(Convert.ToDouble(ElevationTextBox.Text) * 3.28084), // Convert meters to feet
                    (BasePointType)BasePointComboBox.SelectedValue
                );



                _createLevelEventHandler.Input = level;
                _createLevelExternalEvent.Raise();

                LevelList.Add(level); // Add the new level to the ObservableCollection


                // Clear the text boxes after adding the level
                NameTextBox.Text = "";
                ElevationTextBox.Text = "";
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid elevation format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected
            if (LevelsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a row to delete.");
            }

            // Get the selected item (assuming Level is the item type)
            LevelModel selectedLevel = (LevelModel)LevelsDataGrid.SelectedItem;

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the level '{selectedLevel.Name}'?", "Confirmation", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {

                _deleteLevelEventHandler.Input = selectedLevel;
                _deleteLevelExternalEvent.Raise();
                LevelList.Remove(selectedLevel);
            }
        }

        // Enum extension method to retrieve values for ComboBox
        public static IEnumerable<BasePointType> BasePointTypeValues()
        {
            return Enum.GetValues(typeof(BasePointType)).Cast<BasePointType>();
        }
        public IEnumerable<BasePointType> BasePointTypeEnumValues
        {
            get
            {
                return Enum.GetValues(typeof(BasePointType)) as IEnumerable<BasePointType>;
            }
        }
        public BasePointType SelectedBasePointType
        {
            get { return _selectedBasePointType; }
            set
            {
                _selectedBasePointType = value;
                // Notify property changed if you're implementing INotifyPropertyChanged
                // OnPropertyChanged("SelectedBasePointType");
            }
        }
        // Define a boolean property to track the button's enabled state
        private bool isAddLevelButtonEnabled;
        public bool IsAddLevelButtonEnabled
        {
            get { return isAddLevelButtonEnabled; }
            set
            {
                isAddLevelButtonEnabled = value;
                OnPropertyChanged(nameof(IsAddLevelButtonEnabled));
            }
        }
        // Event for property changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Event handler for text box changes
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if both NameTextBox and ElevationTextBox have values
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text) && !string.IsNullOrWhiteSpace(ElevationTextBox.Text))
            {
                IsAddLevelButtonEnabled = true; // Enable the button
            }
            else
            {
                IsAddLevelButtonEnabled = false; // Disable the button
            }
        }


        private void ElevationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Validate the entered text using a regular expression to allow only double values
            Regex regex = new Regex("[^0-9.-]+"); // Allow digits, a minus sign, and a decimal point
            e.Handled = regex.IsMatch(e.Text);
        }


        private void MyMainForm_Closed(object sender, EventArgs e)
        {
            _createLevelExternalEvent.Dispose();
            _deleteLevelExternalEvent.Dispose();
        }

        private void Button_Click_5(object sender, System.Windows.RoutedEventArgs e)
        {
            // Handle button click event
        }



    }
}
