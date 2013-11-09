using System.Collections.Generic;
using System.IO;

namespace FramesForYourPics
{
    /// <summary>
    /// Contains a page list and initialize it
    /// </summary>
    public class PageList
    {
        private readonly string _inputFolder;
        private readonly List<Page> _pageList = new List<Page>();

        /// <summary>
        /// Constructor - Create a page list from the photos in the input folder 
        /// </summary>
        /// <param name="inputFolder">the source folder path</param>
        public PageList(string inputFolder)
        {
            //Set the input folder
            _inputFolder = inputFolder;
            
            //While there are still files in the temp folder
            while (Directory.GetFiles(_inputFolder).Length != 0)
            {
                //Create page from the files in the input folder and delete the handled files
                _pageList.Add(PageFactory.CreatePage(_inputFolder));
            }
        }

        /// <summary>
        /// Save the inner page list to the hard drive
        /// </summary>
        /// <param name="outputFolder">the destination folder</param>
        public void Save(string outputFolder = Constants.OutputFolder)
        {
            //Count the number of pages
            var pageNum = 0;

            //Save each page to the specified path
            foreach (var page in _pageList)
                page.Save(outputFolder + "Page" + (pageNum++) + Constants.JpegPhotoType);

        }
    }
}