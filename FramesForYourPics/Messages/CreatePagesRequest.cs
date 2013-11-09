namespace FramesForYourPics.Messages
{
    public class CreatePagesRequest : FramesForYourPicsMessage
    {
        public CreatePagesRequest(PhotoList photoList, MainWindow callingWindow) : base(photoList, callingWindow) { }
    }
}