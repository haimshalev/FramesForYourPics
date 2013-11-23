namespace FramesForYourPics.Messages
{
    public class CreatePagesRequest : FramesForYourPicsMessage
    {
        public CreatePagesRequest(UiPageList pageList, MainWindow callingWindow) : base(pageList, callingWindow) { }
    }
}