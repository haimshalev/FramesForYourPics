using System.IO;

namespace FramesForYourPics
{
    /// <summary>
    /// Contains a page list and initialize it
    /// </summary>
    public class PageList
    {
        private readonly int _numOfPagesCreated;

        /// <summary>
        /// Constructor - Create a page list from the photos in the input folder 
        /// </summary>
        /// <param name="inputFolder">the source folder path</param>
        /// <param name="outputFolder">the path to the folder which the pages will be saved in</param>
        public PageList(string inputFolder , string outputFolder = Constants.OutputFolder)
        {
            //While there are still files in the temp folder
            while (Directory.GetFiles(inputFolder).Length != 0)
            {
                //Create page from the files in the input folder and delete the handled files
                var p = PageFactory.CreatePage(inputFolder);

                //Save the page to the hard drive
                p.Save(outputFolder + "Page" + (_numOfPagesCreated++) + Constants.OutputFileType);
            }
        }
    }
}