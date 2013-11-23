using FramesForYourPics.MultiThreadedFramework;

namespace FramesForYourPics.Messages
{
    public class SetFrameCommand : Message
    {
        public SetFrameCommand(string framePath)
        {
            FramePath = framePath;
        }

        public string FramePath;
    }
}