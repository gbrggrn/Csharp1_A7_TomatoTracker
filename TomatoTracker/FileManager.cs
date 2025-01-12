using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomatoTracker
{
    /// <summary>
    /// Class holds file operations.
    /// </summary>
    internal class FileManager
    {
        string errorMessage;
        string serializedFileContent;

        /// <summary>
        /// Constructor initializes instance variables.
        /// </summary>
        public FileManager()
        {
            errorMessage = String.Empty;
            serializedFileContent = String.Empty;
        }

        /// <summary>
        /// Properties for errorMessage.
        /// </summary>
        public string ErrorMessage
        {
            get => errorMessage;
            set => errorMessage = value;
        }

        /// <summary>
        /// Properties for serializedFileContent.
        /// </summary>
        public string SerializedFileContent
        {
            get => serializedFileContent;
            set => serializedFileContent = value;
        }

        /// <summary>
        /// Tries to read a file from a given path.
        /// If successful: saves the content to SerializedFileContent.
        /// If unsuccessful: catches exceptions, generates errorMessage and saves it.
        /// </summary>
        /// <param name="path">The path of the file to be opened</param>
        /// <returns>true if successful opening : false if not</returns>
        internal bool TryReadFile(string path)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    SerializedFileContent = streamReader.ReadToEnd();

                    return true;
                }
            }
            catch (FileFormatException)
            {
                ErrorMessage = "Error: wrong file format.";
                return false;
            }
            catch (Exception)
            {
                ErrorMessage = "Error: unexpected error occured";
                return false;
            }
        }

        /// <summary>
        /// Tries to save a file to a given path.
        /// If successful: writes to the file.
        /// If unsuccesful: catches exceptions, generates errorMessage and saves it.
        /// </summary>
        /// <param name="path">The path to save to</param>
        /// <param name="serializedDataIn">The serialized data to save as string</param>
        /// <returns>true if successful save : false if not</returns>
        internal bool TrySaveFile(string path, string serializedDataIn)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    streamWriter.Write(serializedDataIn);
                }

                return true;
            } catch (DirectoryNotFoundException)
            {
                ErrorMessage = "Error: directory not found.";
                return false;
            } catch (Exception)
            {
                ErrorMessage = "Error: unexpected error occured.";
                return false;
            }
        }
    }
}
