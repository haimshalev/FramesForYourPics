namespace FramesForYourPics.Messages
{
    public class SetFrameCommand
    {
        public SetFrameCommand(string framePath)
        {
            FramePath = framePath;
        }

        public string FramePath;
    }
}