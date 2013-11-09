using System.Drawing;
using System.Drawing.Drawing2D;

namespace FramesForYourPics
{
    /// <summary>
    /// Holds all the image manipulation static methods
    /// </summary>
    public class ImageManipulationUtils
    {
        /// <summary>
        /// Merge the specified photo to the specified frame
        /// </summary>
        /// <param name="originalImage">The image without the frame</param>
        /// <param name="frame">The frame to merge</param>
        /// <returns>return the Image of the merged photo</returns>
        public static Image MergePhotoAndFrame(Image originalImage, Image frame)
        {
            //Create a bitmap from the original image and scale it to the scaling size
            var outputImage = new Bitmap(originalImage, Constants.ScaledWidth, Constants.ScaledHeight);

            //Create a canvas to work with 
            using (var canvas = Graphics.FromImage(outputImage))
            {
                //Set the interpolation mode to high quality
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //Draw the frame over the original image
                canvas.DrawImage(frame, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                 new Rectangle(0, 0, frame.Width, frame.Height), GraphicsUnit.Pixel);
            }

            //Return the new image
            return outputImage;
        }

        /// <summary>
        /// Draw the photo on the canvas in the specified location
        /// </summary>
        /// <param name="canvas">The canvas to draw on</param>
        /// <param name="photo">The photo to draw</param>
        /// <param name="xPos">The x start position</param>
        /// <param name="yPos">The y start position</param>
        public static void DrawImageOnBitmap(Image canvas, Image photo, int xPos, int yPos)
        {
            //Create a graphics object from the canvas
            using (var g = Graphics.FromImage(canvas))
            {
                //draw the image to the canvas
                g.DrawImage(photo, xPos, yPos, Constants.ScaledWidth, Constants.ScaledHeight);
            }
        }
    }
}