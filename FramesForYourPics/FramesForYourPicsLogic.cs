using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FramesForYourPics.Messages;

namespace FramesForYourPics
{
    public class FramesForYourPicsLogic
    {
        private string _inputPhotosPath;
        private string _framePath;

        /// <summary>
        /// Set the path to the frame photo
        /// </summary>
        /// <param name="setFrameCommand">The path to the frame source</param>
        public void SetFramePath(SetFrameCommand setFrameCommand)
        {
            _framePath = setFrameCommand.FramePath;
        }

        /// <summary>
        /// Get photos request and fills the user interface with all the images from the specified folder
        /// </summary>
        /// <param name="photosRequest"></param>
        public void GetPhotosFromFolder(PhotosRequest photosRequest)
        {
            //If there is a new folder path update the current one
            if (photosRequest.FolderPath != null)
                _inputPhotosPath = photosRequest.FolderPath;

            if (_inputPhotosPath == null) return;

            //Clear the photo list
            photosRequest.OutputPhotoList.Clear();

            //Get the file list which supported
            var files = from file in Directory.EnumerateFiles(_inputPhotosPath, "*.*", SearchOption.AllDirectories)
                        where file.EndsWith(Constants.JpgPhotoType) || file.EndsWith(Constants.JpegPhotoType)
                        select file;

            //Iterate over all the files
            foreach (var filepath in files)
            {
                //Insert the photo to the user interface photo list
                photosRequest.OutputPhotoList.Add(new Photo(filepath));   
            }
        }

        /// <summary>
        /// Create the final pages for the picture in the request
        /// </summary>
        /// <param name="createPagesRequest">holds the photos to merge and to store in pages</param>
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

            //Merge all the photos with frames
            MergePhotosWithFrame(createPagesRequest.OutputPhotoList);

            //Create PageList from the temp folder
            var pageList = new PageList(Constants.TempFolder);

            //Save the page list to the hard drive
            pageList.Save();

            //Clear the output list allowing the user to delete or replace the images
            createPagesRequest.OutputPhotoList.Clear();
        }

        /// <summary>
        /// Merge all the photos in the photo list with the specified frame and save them to the temp folder
        /// </summary>
        /// <param name="photoList">a list which holds all the photos to merge</param>
        private void MergePhotosWithFrame(IEnumerable<Photo> photoList)
        {
            foreach (var photo in photoList)
            {
                //Merge the photo to the frame
                using (Image img = Image.FromFile(photo.PicturePath), frameImg = Image.FromFile(_framePath))
                {
                    var outputImage = ImageManipulationUtils.MergePhotoAndFrame(img, frameImg);

                    //Save the merged image to the hard drive the number of times specified by the user
                    for (var i = 0; i < photo.NumberOfTimes; i++)
                    {
                        //Create the merged photo path
                        var mergedPhotoTempPath = Constants.TempFolder + Path.GetFileNameWithoutExtension(photo.PicturePath) + "_" + i + Path.GetExtension(photo.PicturePath);

                        //Create the temp directory
                        Directory.CreateDirectory(Constants.TempFolder);

                        //Save the merged photo
                        outputImage.Save(mergedPhotoTempPath, ImageFormat.Jpeg);
                    }
                }
            }
        }
    }
}