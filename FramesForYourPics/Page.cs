using System.Drawing;
using System.Drawing.Imaging;

namespace FramesForYourPics
{
    /// <summary>
    /// Page object will hold 
    /// </summary>
    public class Page
    {
        #region Members

        private readonly Image _pageBitmap;
        private int _currentLine;
        private int _currentColumn;
        private int _numOfPhotos; 

        #endregion

        public Page()
        {
            //Create a bitmap which can hold 8 photos (4X2)
            _pageBitmap = new Bitmap(Constants.ScaledWidth * Constants.NumberOfColumnsInAPage , Constants.ScaledHeight * Constants.NumberOfRowsInAPage);

            //Fill it with a white pixels
            using (var grp = Graphics.FromImage(_pageBitmap))
            {
                grp.FillRectangle(
                    Brushes.White, 0, 0, _pageBitmap.Width, _pageBitmap.Height);
            }
        }

        /// <summary>
        ///Save the page to the specified FilePath
        /// </summary>
        /// <param name="outputFilePath">the destination file path</param>
        public void Save(string outputFilePath)
        {
            //Save the page to the specified FilePath
            _pageBitmap.Save(outputFilePath, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Adds a photo to the current free place in the page
        /// </summary>
        /// <param name="photoPath">the path for the source photo</param>
        public void AddPhoto(string photoPath)
        {
            //If we exceeded the number of photos allowed in a page
            if (_numOfPhotos == Constants.NumberOfPhotosInAPage) return;

            //Load the photo from the hard disk
            using (var photo = Image.FromFile(photoPath))
            {
                //Draw the image on the bitmap
                ImageManipulationUtils.DrawImageOnBitmap(_pageBitmap, photo, _currentColumn * Constants.ScaledWidth, _currentLine * Constants.ScaledHeight);

                #region Update the class counters
                
                //Update the current column (0 or 1)
                _currentColumn = (_currentColumn + 1) % 2;

                //If the column is zero we need to change line 
                if (_currentColumn == 0) _currentLine++;

                //Increment the number of photos
                _numOfPhotos++;
 
                #endregion
            }
        }
    }
}