using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FramesForYourPics.Annotations;

namespace FramesForYourPics
{
    /// <summary>
    /// Holds all the data which saved for every item template in the UI
    /// Uses two way data binding to change the UI and the source
    /// </summary>
    public class Photo : INotifyPropertyChanged
    {
        public Photo(string picturePath , bool setPicture = false)
        {
            // At first each picture need to be printed once 
            NumberOfTimes = 1;

            //Create the scaled image path
            var scaledImageTempPath = Constants.TempScaledFilesFolder + Path.GetFileName(picturePath);

            //Load the image and resize it
            using (var img = Image.FromFile(picturePath))
            {
                var outputImage = new Bitmap(img, Constants.ScaledWidth, Constants.ScaledHeight);

               //Save the scaled image to hard drive
                outputImage.Save(scaledImageTempPath,ImageFormat.Jpeg);
            }

            //Set the picture path
            _picturePath = scaledImageTempPath;

            //Set the picture itself
            EnablePicture(setPicture);
        }

        /// <summary>
        /// Set or remove the refernce to the image
        /// </summary>
        /// <param name="enable">flag which indicate if to enable or disable the image</param>
        public void EnablePicture(bool enable)
        {
            //If we need to enable set the bitmap else remove the refernce
            Picture = enable ? BitmapFromUri(new Uri(Path.GetFullPath(_picturePath))) : null;
            if (Picture != null) Picture.Freeze();
        }

        public static BitmapImage BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        #region Members

        private readonly string _picturePath;
        private BitmapImage _picture;
        private int _numberOfTimes;

        #endregion

        #region Properties

        public string PicturePath
        {
            get { return _picturePath; }
        }

        public BitmapImage Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }

        public int NumberOfTimes
        {
            get { return _numberOfTimes; }
            set { _numberOfTimes = value; OnPropertyChanged(); }
        }

        public ICommand PlusCommand
        {
            get
            {
                //Create the command
                return new RelayCommand(param => PlusNumberOfPhotos());
            }
        }

        public ICommand MinusCommand
        {
            get
            {
                //Create the command
                return new RelayCommand(param => MinusNumberOfPhotos());
            }
        }

        #endregion

        /// <summary>
        /// Add one more copy of this photo to print
        /// </summary>
        private void PlusNumberOfPhotos()
        {
            NumberOfTimes++;
        }

        /// <summary>
        /// Sub one copy of this photo
        /// </summary>
        private void MinusNumberOfPhotos()
        {
            if (NumberOfTimes != 0)
                NumberOfTimes--;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}