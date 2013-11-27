using System.IO;

namespace FramesForYourPics
{
    /// <summary>
    /// Contains a page list and initialize it
    /// </summary>
    public class PageList
    {
        /// <summary>
        /// Constructor - Create a page list from the photos in the input folder 
        /// </summary>
        /// <param name="inputFolder">the source folder path</param>
        /// <param name="outputFolder">the path to the folder which the pages will be saved in</param>
        public static void CreatePageList(string inputFolder , string outputFolder = Constants.OutputFolder)
        {
            try
            {
                var numOfPagesCreated = 0;

                //While there are still files in the temp folder
                while (Directory.GetFiles(inputFolder).Length != 0)
                {
                    //Create page from the files in the input folder and delete the handled files
                    //A page is a disposable object which holds a inner bitmap image. By using the "using" statement 
                    //we automatically dispose the bitmap on completion
                    using (var p = Page.CreatePage(inputFolder))
                    {
                        //Save the page to the hard drive
                        p.Save(outputFolder + "Page" + (numOfPagesCreated++) + Constants.OutputFileType);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                //If the directory is invalid create it
                Directory.CreateDirectory(inputFolder);
            }


        }
    }
}