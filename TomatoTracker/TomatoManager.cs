using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomatoTracker
{
    /// <summary>
    /// Manages the collection of instances of Tomato.
    /// </summary>
    class TomatoManager
    {
        List<Tomato> tomatoes;

        /// <summary>
        /// Constructor initializes a new collection to hold instances of Tomato.
        /// </summary>
        public TomatoManager()
        {
            tomatoes = new();
        }

        /// <summary>
        /// Adds an instance of Tomato to the collection.
        /// </summary>
        /// <param name="currentBreedIn">The instance of Tomato to be added</param>
        public void AddBreed(Tomato currentBreedIn)
        {
            tomatoes.Add(currentBreedIn);
        }

        /// <summary>
        /// Removes an instance of Tomato from the collection.
        /// </summary>
        /// <param name="indexToRemove">Index of the instance that is to be removed</param>
        public void RemoveBreed(int indexToRemove)
        {
            tomatoes.RemoveAt(indexToRemove);
        }

        /// <summary>
        /// Returns a specific instance of Tomato from the collection.
        /// </summary>
        /// <param name="indexToGet">Index of the instance to return</param>
        /// <returns>The instance of Tomato corresponding to the index</returns>
        public Tomato GetBreed(int indexToGet)
        {
            return tomatoes[indexToGet];
        }

        /// <summary>
        /// Returns the whole collection of instances of Tomato.
        /// </summary>
        /// <returns>The collection of instances of Tomato</returns>
        public List<Tomato> GetBreedList()
        {
            return tomatoes;
        }

        /// <summary>
        /// Overwrites an instance of Tomato in the collection of instances of Tomato.
        /// </summary>
        /// <param name="currentBreedIn">The instance of Tomato to write</param>
        /// <param name="indexToEditIn">Index of the instance of Tomato to overwrite</param>
        public void EditBreed(Tomato currentBreedIn, int indexToEditIn)
        {
            tomatoes[indexToEditIn] = currentBreedIn;
        }

        /// <summary>
        /// Serializes the collection of Tomatoes into a single string.
        /// </summary>
        /// <returns>The serialized string of Tomatoes data</returns>
        internal string SerializeTomatoes()
        {
            string serializedTomatoes = String.Empty;

            foreach (var tomato in tomatoes)
            {
                serializedTomatoes += tomato.SerializeTomato() + "||";
            }

            return serializedTomatoes;
        }

        /// <summary>
        /// Deserializes a serialized string of tomato-data and saves it to a new collection.
        /// </summary>
        /// <param name="serializedTomatoesIn">The serialized string of Tomato-data</param>
        internal void DeSerializeTomatoes(string serializedTomatoesIn)
        {
            string[] wholeTomato = serializedTomatoesIn.Split("||");

            string[] removeLast = wholeTomato.Take(wholeTomato.Length - 1).ToArray();

            tomatoes = new();

            for (int i = 0; i < removeLast.Length; i++)
            {
                string unsplitTomato = removeLast[i];

                string[] splitTomato = unsplitTomato.Split("|");

                Tomato tomato = new();

                //First datapoint
                tomato.BreedName = splitTomato[0].Trim();

                //Second datapoint
                tomato.PlantDate = ParsePlantDate(splitTomato[1].Trim());

                //Third datapoint
                tomato.TypeOfTomato = ParseTypeOfTomato(splitTomato[2].Trim());

                //Fourth datapoint
                tomato.OtherNotes = splitTomato[3].Trim();

                //Add to breed-list
                tomatoes.Add(tomato);
            }
        }

        /// <summary>
        /// Parses the plantDate from string -> DateTime.
        /// </summary>
        /// <param name="plantDateIn">The plantDate as string</param>
        /// <returns>The plantDate as DateTime</returns>
        private DateTime ParsePlantDate(string plantDateIn)
        {
            if (DateTime.TryParseExact(plantDateIn.Trim(),
                    "MM/dd/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedPlantDate))
            {
                return parsedPlantDate;
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Parses the typeOfTomato from string -> Enums.TomatoType.
        /// </summary>
        /// <param name="typeOfTomatoIn">The typeOfTomato as string</param>
        /// <returns>The typeOfTomato as enum if true, else fallback value "Unknown"</returns>
        private Enums.TomatoType ParseTypeOfTomato(string typeOfTomatoIn)
        {
            foreach (Enums.TomatoType type in Enum.GetValues(typeof(Enums.TomatoType)))
            {
                if (typeOfTomatoIn == type.ToString())
                {
                    return type;
                }
            }

            return Enums.TomatoType.Unknown;
        }
    }
}
