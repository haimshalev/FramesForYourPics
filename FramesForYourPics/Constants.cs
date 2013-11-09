namespace FramesForYourPics
{
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
        public const string JpegPhotoType = ".jpeg";
        public const string JpgPhotoType = ".jpg";
        
        //The photos types which will read from the hard drive
        public const string PhotoTypes = "*" + JpegPhotoType; 
        
        #endregion

        #region Paths Constants

        //A path to a temporary folder which will save the create merged photos
        public const string TempFolder = @"TempFolder\";

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
