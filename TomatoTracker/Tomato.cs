using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TomatoTracker
{
    /// <summary>
    /// Data structure class for Tomato.
    /// </summary>
    internal class Tomato
    {
        private string breedName;
        private DateTime plantDate;
        private Enums.TomatoType typeOfTomato;
        private string otherNotes;

        /// <summary>
        /// Constructor initializes instance variables.
        /// </summary>
        public Tomato()
        {
            breedName = string.Empty;
            plantDate = DateTime.Now;
            typeOfTomato = Enums.TomatoType.Unknown;
            otherNotes = string.Empty;
        }

        /// <summary>
        /// Properties for breedName.
        /// </summary>
        public string BreedName
        {
            get => breedName;
            set => breedName = value;
        }

        /// <summary>
        /// Properties for plantDate.
        /// </summary>
        public DateTime PlantDate
        {
            get => plantDate;
            set => plantDate = value;
        }

        /// <summary>
        /// Properties for typeOfTomato.
        /// </summary>
        public Enums.TomatoType TypeOfTomato
        {
            get => typeOfTomato;
            set => typeOfTomato = value;
        }

        /// <summary>
        /// Properties for otherNotes.
        /// </summary>
        public string OtherNotes
        {
            get => otherNotes; 
            set => otherNotes = value;
        }

        /// <summary>
        /// Serializes this instance of Tomato into a string and returns it.
        /// </summary>
        /// <returns>The serialized data as a string</returns>
        internal string SerializeTomato() => $"{BreedName}|{PlantDate.ToString()}|{TypeOfTomato.ToString()}|{OtherNotes}";
    }
}
