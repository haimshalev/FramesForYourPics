namespace FramesForYourPics.Messages
{
    public class PhotosRequest : FramesForYourPicsMessage
    {
        private readonly string _folderPath;

        public PhotosRequest(PhotoList photoList, MainWindow callingWindow)
            : this(null, photoList, callingWindow)
        {
        }

        public PhotosRequest(string folderPath, PhotoList photoList , MainWindow callingWindow)
            : base(photoList, callingWindow)
        {
            _folderPath = folderPath;
        }

        public string FolderPath
        {
            get { return _folderPath; }
        }
    }
}