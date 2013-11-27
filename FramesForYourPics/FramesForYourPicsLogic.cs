using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FramesForYourPics.Messages;
using FramesForYourPics.MultiThreadedFramework;

namespace FramesForYourPics
{
    public class FramesForYourPicsLogic : Communicator
    {
        private string _inputPhotosPath;
        private string _framePath;
        private readonly Constants.SupportedFileTypes _supportedFileTypes = new Constants.SupportedFileTypes();
        private int _lastTimeMissingPhotos;


        /// <summary>
        /// Set the path to the frame photo
        /// </summary>
        /// <param name="setFrameCommand">The path to the frame source</param>
        [MessageHandler]
        public void SetFramePath(SetFrameCommand setFrameCommand)
        {
            _framePath = setFrameCommand.FramePath;
        }

        /// <summary>
        /// Get photos request and fills the user interface with all the images from the specified folder
        /// </summary>
        /// <param name="photosRequest"></param>
        [MessageHandler]
        public void GetPhotosFromFolder(PhotosRequest photosRequest)
        {
            //If there is a new folder path update the current one
            if (photosRequest.FolderPath != null)
                _inputPhotosPath = photosRequest.FolderPath;

            if (_inputPhotosPath == null) return;

            //Clear all the previous data and arranging temp folders
            ClearAllTemproryData(photosRequest.OutputPhotoList);

            //Get the file list which supported
            var files = from file in Directory.EnumerateFiles(_inputPhotosPath, "*.*", SearchOption.AllDirectories)
                        where _supportedFileTypes.IsFileSupported(file)
                        select file;


            //Disable all buttons in gui
            photosRequest.CallingWindow.DisableAllButtons();

            //Iterate over all the files
            foreach (var filepath in files)
            {
                //Insert the photo to the user interface photo list
                photosRequest.OutputPhotoList.AddPhoto(filepath);   
            }

            //Enable all buttons in gui
            photosRequest.CallingWindow.EnableAllButtons();
        }

        /// <summary>
        /// Create the final pages for the picture in the request
        /// </summary>
        /// <param name="createPagesRequest">holds the photos to merge and to store in pages</param>
        [MessageHandler]
        public void CreateFramedPages(CreatePagesRequest createPagesRequest)
        {
            //If there is no frame to merge to alert the user and return
            if (_framePath == null)
            {
                //Alert the user that he need to select a frame
                createPagesRequest.CallingWindow.NotifyOnAMissingFrame();

                //Return
                return;
            }

            //Check if all pages are full
            var sumOfPhotos = createPagesRequest.OutputPhotoList.GetNumberOfPhotos();
            if ((sumOfPhotos % 8 != 0) && (_lastTimeMissingPhotos != (sumOfPhotos % 8)))
            {
                //Alert the user on the number of missing photos
                createPagesRequest.CallingWindow.NotifyOnMissingPhotos(8 - (sumOfPhotos % 8));

                _lastTimeMissingPhotos = sumOfPhotos % 8;

                //Return
                return;
            }

            //Disable all buttons in gui
            createPagesRequest.CallingWindow.DisableAllButtons();

            //Create outPutFolder
            DeletaAndCreateDirectory(Constants.OutputFolder);

            //Merge all the photos with frames
            MergePhotosWithFrame(createPagesRequest.OutputPhotoList);

            //Create PageList from the temp folder and save them to the hard drive
            PageList.CreatePageList(Constants.TempMergedFilesFolder);
            
            //Clears all the temprory folders and images in cache
            ClearAllTemproryData(createPagesRequest.OutputPhotoList);

            //Enable all buttons in gui
            createPagesRequest.CallingWindow.EnableAllButtons();
            createPagesRequest.CallingWindow.RestoreDefatultContent();
        }


        /// <summary>
        /// Clears all the temprory folders and images in cache
        /// </summary>
        private void ClearAllTemproryData(UiPageList pageList)
        {
            //Clear the page list
            pageList.Clear();

            //If a scaled directory is not empty lets delete it and create a new one
            DeletaAndCreateDirectory(Constants.TempScaledFilesFolder);

            //If a merged directory is not empty lets delete it and creat a new one
            DeletaAndCreateDirectory(Constants.TempMergedFilesFolder);

            _lastTimeMissingPhotos = 0;
        }

        /// <summary>
        /// Delets recursivly a directory and create a new one instead
        /// </summary>
        /// <param name="dirPath">the directory to delete and create</param>
        private static void DeletaAndCreateDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
            Directory.CreateDirectory(dirPath);
        }

        /// <summary>
        /// Merge all the photos in the photo list with the specified frame and save them to the temp folder
        /// </summary>
        private void MergePhotosWithFrame(UiPageList pageList)
        {
            foreach (var photo in pageList.GetPages().SelectMany(page => page))
            {
                //Merge the photo to the frame
                using (Image img = Image.FromFile(photo.PicturePath), frameImg = Image.FromFile(_framePath))
                {
                    var outputImage = ImageManipulationUtils.MergePhotoAndFrame(img, frameImg);

                    //Save the merged image to the hard drive the number of times specified by the user
                    for (var i = 0; i < photo.NumberOfTimes; i++)
                    {
                        //Create the merged photo path
                        var mergedPhotoTempPath = Constants.TempMergedFilesFolder + Path.GetFileNameWithoutExtension(photo.PicturePath) + "_" + i + Constants.OutputFileType;

                        //Create the temp directory
                        Directory.CreateDirectory(Constants.TempMergedFilesFolder);

                        //Save the merged photo
                        outputImage.Save(mergedPhotoTempPath, ImageFormat.Jpeg);
                    }
                }
            }
        }
    }
}