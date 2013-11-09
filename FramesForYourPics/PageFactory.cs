using System.IO;

namespace FramesForYourPics
{
    /// <summary>
    /// Factory for pages 
    /// </summary>
    public class PageFactory
    {
        /// <summary>
        /// Creates a page from a list of photos in a specified folder
        /// </summary>
        /// <param name="inputFolder">the source folder</param>
        /// <returns>if there are photos in the folder this methods returns a page
        /// else returning null</returns>
        public static Page CreatePage(string inputFolder)
        {
            //if there is no files in the input folder return null
            if (Directory.GetFiles(inputFolder).Length == 0) return null;

            //Create an empty page
            var page = new Page();

            //Count the number of photos we try to insert to the page
            var photoCounter = 0;

            foreach (var photoPath in Directory.GetFiles(inputFolder))
            {
                //Add the photo to the page
                page.AddPhoto(photoPath);

                //Delete the temporary photo
                File.Delete(photoPath);

                //Increment the photo counter and if we handled the max number of photos, break
                if (++photoCounter == Constants.NumberOfPhotosInAPage) break;
            }

            //Return the created page
            return page;
        }
    }
}