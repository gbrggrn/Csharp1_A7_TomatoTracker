using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TomatoTracker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private bool isClosedViaOK;
        private readonly ValidateInput validateInput;
        private Tomato currentBreed;
        private int indexIfChanging;

        /// <summary>
        /// Constructor initializes instance variables and configures the comboBox & DatePicker.
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            validateInput = new();
            isClosedViaOK = false;
            currentBreed = new Tomato();
            indexIfChanging = -1;
            LoadTypeComboBox();
            ConfigPlantDatePicker();
        }

        /// <summary>
        /// Properties for the boolean flag isClosedViaOk.
        /// </summary>
        public bool IsClosedViaOK
        {
            get => isClosedViaOK;
            set => isClosedViaOK = value;
        }

        /// <summary>
        /// Returns indexIfChanging.
        /// </summary>
        public int IndexIfChanging => indexIfChanging;

        /// <summary>
        /// Returns the current instance of Tomato.
        /// </summary>
        internal Tomato CurrentBreed => currentBreed;

        /// <summary>
        /// Loads values to typeComboBox.
        /// </summary>
        private void LoadTypeComboBox()
        {
            typeComboBox.ItemsSource = Enum.GetValues(typeof(Enums.TomatoType));
            typeComboBox.SelectedItem = Enums.TomatoType.Unknown;
        }

        /// <summary>
        /// Configures plantDatePicker to use selected time: today.
        /// </summary>
        private void ConfigPlantDatePicker()
        {
            plantDatePicker.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Retrieves the user input and sends it for validation.
        /// If not valid: Displays errormessages.
        /// If valid: saves the input to currentBreed.
        ///           Closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string breedName = breedNameTxtBox.Text;
            DateTime plantDate = (DateTime)plantDatePicker.SelectedDate!;
            Enums.TomatoType typeOfTomato = (Enums.TomatoType)typeComboBox.SelectedItem;
            TextRange otherNotesRange = new TextRange(otherNotesRichTxtBox.Document.ContentStart, otherNotesRichTxtBox.Document.ContentEnd);
            string otherNotes = otherNotesRange.Text.Trim();

            validateInput.ValidateBreed(breedName, otherNotes);

            if (!validateInput.IsValid)
            {
                InputErrorDisplay();
                return;
            }
            else
            {
                SaveInput(breedName, plantDate, typeOfTomato, otherNotes);

                IsClosedViaOK = true;
                this.Close();
            }
        }

        /// <summary>
        /// Sets isClosedViaOk to false if true.
        /// Closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (isClosedViaOK)
            {
                isClosedViaOK = false;
            }

            this.Close();
        }

        /// <summary>
        /// Configures the GUI to reflect action: editing.
        /// Loads the information from the instance of Tomato to be edited.
        /// </summary>
        /// <param name="breedToEditIn">The instance of tomato to edit</param>
        /// <param name="indexToChangeIn">The breedList index of the instance of Tomato to edit</param>
        internal void EditBreed(Tomato breedToEditIn, int indexToChangeIn)
        {
            currentBreed = breedToEditIn;
            indexIfChanging = indexToChangeIn;
            tomatoTrackerLabel.Content = $"Edit {breedToEditIn.BreedName}";

            breedNameTxtBox.Text = currentBreed.BreedName;
            plantDatePicker.SelectedDate = currentBreed.PlantDate;
            typeComboBox.SelectedItem = currentBreed.TypeOfTomato;
            otherNotesRichTxtBox.AppendText(currentBreed.OtherNotes);
        }

        /// <summary>
        /// Fetches and displays errormessages.
        /// </summary>
        private void InputErrorDisplay()
        {
            string formattedErrorMessages = string.Empty;

            foreach (var errorMessage in validateInput.ErrorMessages)
            {
                formattedErrorMessages += errorMessage + "\n";
            }

            MessageBox.Show($"{formattedErrorMessages}",
                "Input Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Saves input to the current instance of Tomato.
        /// </summary>
        /// <param name="breedNameIn">The breed name as string</param>
        /// <param name="plantDateIn">The plant date as DateTime</param>
        /// <param name="typeOfTomatoIn">The type of tomato as an enum</param>
        /// <param name="otherNotesIn">Other notes as string</param>
        private void SaveInput(string breedNameIn, 
            DateTime plantDateIn, 
            Enums.TomatoType typeOfTomatoIn, 
            string otherNotesIn)
        {
            currentBreed.BreedName = breedNameIn;
            currentBreed.PlantDate = plantDateIn;
            currentBreed.TypeOfTomato = typeOfTomatoIn;
            currentBreed.OtherNotes = otherNotesIn;
        }
    }
}
