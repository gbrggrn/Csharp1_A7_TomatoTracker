using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomatoTracker
{
    //Declare delagate that will contain methods with parameters:string and out string.
    delegate bool Validations(string toValidate, out string errorMessage);

    /// <summary>
    /// Class holds validation methods used in the application.
    /// </summary>
    class ValidateInput
    {
        /// <summary>
        /// Initializes an array of Validations delegate type.
        /// Stores two validation methods in it.
        /// </summary>
        private static Validations[] validations = new Validations[]
        {
                ValidateBreedName,
                ValidateOtherNotes
        };

        private bool isValid;
        private string[]? errorMessages;
        private string[]? input;

        /// <summary>
        /// Constructor initializes other instance variables.
        /// </summary>
        public ValidateInput()
        {
            isValid = true;
            errorMessages = new string[validations.Length];
            input = new string[validations.Length];
        }

        /// <summary>
        /// Properties for isValid.
        /// </summary>
        internal bool IsValid
        {
            get => isValid;
            set => isValid = value;
        }

        /// <summary>
        /// Get property for errorMessages.
        /// </summary>
        internal string[] ErrorMessages => errorMessages!;

        /// <summary>
        /// Properties for input.
        /// </summary>
        internal string[] Input
        {
            get => input!;
            set => input = value;
        }

        /// <summary>
        /// Arranges input in a array of strings.
        /// Invokes the Validations delegate methods on each point of the input-array.
        /// Stores eventual errormessages.
        /// </summary>
        /// <param name="breedNameIn">The breed name to be validated</param>
        /// <param name="otherNotesIn">Other notes to be validated</param>
        internal void ValidateBreed(string breedNameIn,
            string otherNotesIn)
        {;
            Input = new string[] { breedNameIn, otherNotesIn };
            IsValid = true;

            for (int i = 0; i < Input.Length; i++)
            {
                if (!validations[i].Invoke(Input[i], out string errorMessage))
                {
                    IsValid = false;
                }

                ErrorMessages[i] = errorMessage;
            }
        }

        /// <summary>
        /// Checks the string breedName for null/whitespace and length.
        /// </summary>
        /// <param name="breedName">The breed name to be validated</param>
        /// <param name="errorMessage">The eventual errorMessage as an "out" parameter</param>
        /// <returns>true if input is valid : false if not</returns>
        private static bool ValidateBreedName(string breedName, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(breedName))
            {
                errorMessage = "Breed name can't be empty.";
                return false;
            }

            if (breedName.Length > 60)
            {
                errorMessage = "Breed name can't be longer than 60 characters.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Checks the string otherNotes for null/whitespace and length.
        /// </summary>
        /// <param name="otherNotes">Other notes to be validated</param>
        /// <param name="errorMessage">The eventual errorMessage as an "out" parameter</param>
        /// <returns>true if input is valid : false if not</returns>
        private static bool ValidateOtherNotes(string otherNotes, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(otherNotes))
            {
                errorMessage = "Other notes can't be empty.";
                return false;
            }

            if (otherNotes.Length > 500)
            {
                errorMessage = "Other notes can't be longer than 500 characters.";
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
