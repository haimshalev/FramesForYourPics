namespace FramesForYourPics.Messages
{
    public class PhotosRequest : FramesForYourPicsMessage
    {
        private readonly string _folderPath;

        public PhotosRequest(UiPageList pageList, MainWindow callingWindow)
            : this(null, pageList, callingWindow)
        {
        }

        public PhotosRequest(string folderPath, UiPageList pageList, MainWindow callingWindow)
            : base(pageList, callingWindow)
        {
            _folderPath = folderPath;
        }

        public string FolderPath
        {
            get { return _folderPath; }
        }
    }
}