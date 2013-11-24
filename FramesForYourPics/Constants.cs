using System.Collections.Generic;
using System.Linq;

namespace FramesForYourPics
{
    #region Public Delegates
    public delegate void ParameterLessDelegate();

    public delegate void PhotoParameterDelegate(Photo photo);

    public delegate void PhotoListParameterDelegate(PhotoList photoList);

    public delegate void IntParameterDelegate(int num);

    public delegate void IntIntParameterDelegate(int num1, int num2); 
    #endregion

    class Constants
    {
        #region Photos Constants

        //Every merged image will have this width
        public const int ScaledWidth = 640;

        //Every merged image will have this height
        public const int ScaledHeight = 480; 
        
        #endregion

        #region Supported Types

        //The type of photos to save and open
        public class SupportedFileTypes
        {
            private readonly List<string> _supportedFileTypes; 
            
            public SupportedFileTypes()
            {
                _supportedFileTypes = new List<string> {".jpeg", ".jpg", ".JPG"};
            }

            public bool IsFileSupported(string filePath)
            {
                //Return true is the file path ends with one of the supported file types
                return _supportedFileTypes.Any(filePath.EndsWith);
            }
        }

        //The type of every file which created during the runtime
        public const string OutputFileType = ".jpeg";
        
        #endregion

        #region Paths Constants

        //The path to the temporary folder which will save the scaled original photos
        public const string TempScaledFilesFolder = @"C:\FramesForYourPics\TempScaledFolder\";

        //A path to a temporary folder which will save the create merged photos
        public const string TempMergedFilesFolder = @"C:\FramesForYourPics\TempMergedFolder\";

        //A path to the output folder which will save the output pages
        public const string OutputFolder = @"OutputFolder\"; 

        #endregion

        #region Page Constants

        //Set the number of photos allowed to save in one page
        public const int NumberOfPhotosInAPage = 8;

        //Number of columns in a page
        public const int NumberOfColumnsInAPage = 2;

        //Number of rows in a page
        public const int NumberOfRowsInAPage = 4; 
        
        #endregion

    }
}
