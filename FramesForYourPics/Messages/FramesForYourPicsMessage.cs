using FramesForYourPics.MultiThreadedFramework;

namespace FramesForYourPics.Messages
{
    public class FramesForYourPicsMessage : Message
    {
        public UiPageList OutputPhotoList { get; set; }

        public MainWindow CallingWindow { get; set; }

        public FramesForYourPicsMessage(UiPageList pageList , MainWindow callingWindow)
        {
            OutputPhotoList = pageList;
            CallingWindow = callingWindow;
        }
    }
}