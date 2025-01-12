using Microsoft.Win32;
using System.Reflection;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TomatoTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int maxBreeds = 20;
        private TomatoManager tomatoManager;
        private FileManager fileManager;
        private const int daysToView = 21;
        private const int germinationDays = 15;

        /// <summary>
        /// Constructor initializes instance variables and programmatically sets the title of the toDoToday-box.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            tomatoManager = new();
            fileManager = new();
            toDoTodayLabel.Content = $"To do today {DateTime.Now.ToShortDateString()}";
        }

        /// <summary>
        /// Reacts to click of the addBreedButton.
        /// Displays input window.
        /// Checks boolean flag to save or not.
        /// If flag = true: tells tomatoManager to add the current instance of Tomato.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBreedButton_Click(object sender, RoutedEventArgs e)
        {
            if (breedsListBox.Items.Count < maxBreeds)
            {
                Window1 addBreedWindow = new();
                addBreedWindow.ShowDialog();

                if (addBreedWindow.IsClosedViaOK)
                {
                    tomatoManager.AddBreed(addBreedWindow.CurrentBreed);
                    UpdateBreedListBox(addBreedWindow.CurrentBreed);
                }
            }
            else
            {
                MessageBox.Show("You have reached the max number of breeds (20).",
                    "Max number of breeds reached",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
        }

        /// <summary>
        /// Adds the newly created breeds name to the breedsListBox.
        /// </summary>
        /// <param name="currentBreedIn"></param>
        private void UpdateBreedListBox(Tomato currentBreedIn)
        {
            breedsListBox.Items.Add(currentBreedIn.BreedName);
            UpdateDashboard();
        }

        /// <summary>
        /// Reacts to click of the removeButton.
        /// If the user is sure: Removes the selected breed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (breedsListBox.Items.Count < 1)
            {
                MessageBox.Show("You need to add breeds first",
                    "No breeds added",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            if (breedsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("No breed was selected",
                    "No breed selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            
            int indexToRemove = breedsListBox.SelectedIndex;

            MessageBoxResult removeOrNot = MessageBox.Show(
                $"Are you sure you wish to remove {breedsListBox.Items.GetItemAt(indexToRemove)}",
                "Are you sure?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (removeOrNot == MessageBoxResult.Yes)
            {
                breedsListBox.Items.RemoveAt(indexToRemove);

                tomatoManager.RemoveBreed(indexToRemove);

                UpdateDashboard();

                MessageBox.Show("Breed successfully removed",
                    "Breed removed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Deselects all breeds in the breedsListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeselectButton_Click(object sender, RoutedEventArgs e)
        {
            if (breedsListBox.SelectedIndex != -1)
            {
                breedsListBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Reacts to click of the editButton.
        /// If breed is selected: retrieves the corresponding instance of Tomato and loads it to input window.
        /// If good return: edits breed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (breedsListBox.SelectedIndex != -1)
            {
                int indexToLoad = breedsListBox.SelectedIndex;
                Tomato breedToEdit = tomatoManager.GetBreed(indexToLoad);
                Window1 editBreedWindow = new Window1();
                editBreedWindow.EditBreed(breedToEdit, indexToLoad);
                editBreedWindow.ShowDialog();

                if (editBreedWindow.IsClosedViaOK)
                {
                    tomatoManager.EditBreed(editBreedWindow.CurrentBreed, editBreedWindow.IndexIfChanging);
                    ManualUpdateBreedsListBox();
                }
            }
            else
            {
                MessageBox.Show("No breed was selected",
                    "No breed selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Forcibly updates the breedsListBox.
        /// </summary>
        private void ManualUpdateBreedsListBox()
        {
            breedsListBox.Items.Clear();

            foreach (var tomato in tomatoManager.GetBreedList())
            {
                breedsListBox.Items.Add(tomato.BreedName);
            }
        }

        /// <summary>
        /// Reacts to click of the exitButton.
        /// Exits the application if user is sure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult exitOrNot = MessageBox.Show("Are you sure you want to exit?\nUnsaved changes will be lost.",
                "Exit?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            switch (exitOrNot)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        /// <summary>
        /// Reacts to click of saveButton.
        /// Opens a SaveFileDialog.
        /// Serializes the current Tomato data through TomatoManager and saves it through FileManager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Title = "Save Dashboard",
                Filter = "Text Files (*.txt)|*.txt",
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                string serializedDashboard = tomatoManager.SerializeTomatoes();

                if (fileManager.TrySaveFile(filePath, serializedDashboard)) 
                {
                    MessageBox.Show("Dashboard saved successfully",
                        "Successful Save",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"{fileManager.ErrorMessage}",
                        "Error: no save",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    fileManager.ErrorMessage = String.Empty;
                }
            }
        }

        /// <summary>
        /// Reacts to click of the loadButton.
        /// Opens a OpenFileDialog.
        /// Retrieves serialized data through FileManager and deserializes through TomatoManager.
        /// Calls methods to update the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Open Dashboard",
                Filter = "Text Files (*.txt)|*.txt",
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;

                if (fileManager.TryReadFile(filePath))
                {
                    tomatoManager.DeSerializeTomatoes(fileManager.SerializedFileContent);
                    fileManager.SerializedFileContent = String.Empty;
                    ManualUpdateBreedsListBox();
                    UpdateDashboard();

                    MessageBox.Show($"File: '{filePath}' currently displayed.",
                        "Successful loading",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"{fileManager.ErrorMessage}",
                        "Error: no loading",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    fileManager.ErrorMessage = String.Empty;
                }
            }
        }

        /// <summary>
        /// When called, updates the dashboard through helpers.
        /// </summary>
        private void UpdateDashboard()
        {
            Enums enums = new();
            List<Tomato> tomatoes = tomatoManager.GetBreedList();

            ClearDashboardListBoxes();
            UpdateListBox(tomatoes, enums, wateringListBox, "Water");
            UpdateListBox(tomatoes, enums, trellisListBox, "Trellis");
            UpdateHarvestListBox(tomatoes, enums);
        }

        /// <summary>
        /// When called, clears the listBoxes.
        /// </summary>
        private void ClearDashboardListBoxes()
        {
            wateringListBox.Items.Clear();
            trellisListBox.Items.Clear();
            harvestDateListBox.Items.Clear();
            toDoListBox.Items.Clear();
        }

        /// <summary>
        /// Updates the specified listboxes.
        /// </summary>
        /// <param name="tomatoesIn">The current collection of Tomatoes</param>
        /// <param name="enumsIn">A reference to the Enums-class that also holds dictionaries</param>
        /// <param name="listBoxIn">The listBox to edit</param>
        /// <param name="task">The task to be done as string</param>
        private void UpdateListBox(List<Tomato> tomatoesIn, Enums enumsIn, ListBox listBoxIn, string task)
        {
            DateTime today = DateTime.Now.Date;
            //List of tuples so that it can be sorted later
            var tasksToDisplay = new List<(DateTime, string)>();

            for (int i = 0; i < tomatoesIn.Count; i++)
            {
                int interval = 1;
                DateTime plantDate = tomatoesIn[i].PlantDate;

                if (task == "Water")
                {
                    interval = enumsIn.WateringIntervals[tomatoesIn[i].TypeOfTomato];
                }
                if (task == "Trellis")
                {
                    interval = enumsIn.TrellisIntervals[tomatoesIn[i].TypeOfTomato];
                }

                for (int j = 0; j < daysToView; j++)
                {
                    DateTime currentDate = today.AddDays(j);

                    int daysSincePlanted = (currentDate - plantDate).Days;

                    //Check if germination-period has passed
                    if (daysSincePlanted < germinationDays)
                    {
                        if (currentDate.Date == DateTime.Now.Date)
                        {
                            toDoListBox.Items.Add($"{task} {tomatoesIn[i].BreedName}");
                        }

                        continue;
                    }
                    
                    if (daysSincePlanted >= 0 && daysSincePlanted % interval == 0)
                    {
                        tasksToDisplay.Add((currentDate, $"{currentDate.ToShortDateString()} {task} {tomatoesIn[i].BreedName}"));
                    }
                }
            }

            //Sort the list on DateTime
            tasksToDisplay.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            //Display the string part of each item in sorted fashion
            foreach (var item in tasksToDisplay)
            {
                listBoxIn.Items.Add(item.Item2);
            }
        }

        /// <summary>
        /// Updates the harvestDateListBox.
        /// </summary>
        /// <param name="tomatoesIn">The current collection of Tomatoes</param>
        /// <param name="enumsIn">A reference to the Enums-class that also holds dictionaries</param>
        private void UpdateHarvestListBox(List<Tomato> tomatoesIn, Enums enumsIn)
        {
            for (int i = 0; i < tomatoesIn.Count; i++)
            {
                DateTime plantDate = tomatoesIn[i].PlantDate;
                int growthTime = enumsIn.GrowthTime[tomatoesIn[i].TypeOfTomato];
                plantDate = plantDate.AddDays(growthTime);

                harvestDateListBox.Items.Add($"{plantDate.ToShortDateString()} Harvest {tomatoesIn[i].BreedName}");
            }
        }

        /// <summary>
        /// If the user wants to: Starts a new Dashboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult newOrNot = MessageBox.Show("Do you wish to start a new Dashboard?\nUnsaved changes will be lost.",
                "New Dashboard?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (newOrNot == MessageBoxResult.Yes)
            {
                tomatoManager = new();
                fileManager = new();

                ClearDashboardListBoxes();
                breedsListBox.Items.Clear();
            }
        }

        /// <summary>
        /// Displays a help-box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{GetAssemblyInfo()}\nAdd your tomato breeds to see what to do today, and the coming three weeks.",
                "Help",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Helper to get info from Assemblyinfo.cs.
        /// </summary>
        /// <returns>Info as formatted string</returns>
        private string GetAssemblyInfo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
            string description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()!.Description;
            string author = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()!.Company;

            return $"App Title: {title}\n" +
                $"Description: {description}\n" +
                $"Author: {author}\n";
        }
    }
}