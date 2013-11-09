namespace FramesForYourPics.Messages
{
    public class FramesForYourPicsMessage
    {
        public PhotoList OutputPhotoList { get; set; }

        public MainWindow CallingWindow { get; set; }

        public FramesForYourPicsMessage(PhotoList photoList , MainWindow callingWindow)
        {
            OutputPhotoList = photoList;
            CallingWindow = callingWindow;
        }
    }
}