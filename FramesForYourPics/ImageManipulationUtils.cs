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
        /// Concatenate the given images horizontally
        /// </summary>
        /// <param name="firstImage">The left image</param>
        /// <param name="secondImage">The right image</param>
        /// <returns>A horizontal concatenated image</returns>
        public static Image ConcatenateTwoImagesHorizontal(Image firstImage, Image secondImage)
        {
            //Create the concatenate image placeholder (1 X 2)
            var concateImage = new Bitmap(firstImage.Width + secondImage.Width, firstImage.Height);

            //Create a graphics object for the new image
            using (var g = Graphics.FromImage(concateImage))
            {
                //draw firstImage to upper left corner
                g.DrawImage(firstImage, 0, 0, firstImage.Width, firstImage.Height);

                //draw the second image along the first image to the right
                g.DrawImage(secondImage, firstImage.Width, 0, secondImage.Width, secondImage.Height);
            }

            //Return the concatenate image
            return concateImage;
        }

        /// <summary>
        /// Concatenate the given images vertically
        /// </summary>
        /// <param name="firstImage">The upper image</param>
        /// <param name="secondImage">The lower image</param>
        /// <returns>A vertical concatenated image</returns>
        public static Image ConcatenateTwoImagesVertical(Image firstImage, Image secondImage)
        {
            //Create the concatenate image placeholder (2 X 1)
            var concateImage = new Bitmap(firstImage.Width, firstImage.Height + secondImage.Height);

            //Create a graphics object for the new image
            using (var g = Graphics.FromImage(concateImage))
            {
                //draw firstImage to upper left corner
                g.DrawImage(firstImage, 0, 0, firstImage.Width, firstImage.Height);

                //draw the second image along the first image to the right
                g.DrawImage(secondImage, 0, firstImage.Height, secondImage.Width, secondImage.Height);
            }

            //Return the concatenate image
            return concateImage;
        }

    }
}