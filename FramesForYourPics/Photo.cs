using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FramesForYourPics.Annotations;

namespace FramesForYourPics
{

    /// <summary>
    /// An observable lost of photo objects, used for binding to a list box
    /// </summary>
    public class PhotoList : ObservableCollection<Photo>{}

    /// <summary>
    /// Holds all the data which saved for every item template in the UI
    /// Uses two way data binding to change the UI and the source
    /// </summary>
    public class Photo : INotifyPropertyChanged
    {
        public Photo(string picturePath)
        {
            // At first each picture need to be printed once 
            NumberOfTimes = 1;

            //Set the picture path
            _picturePath = picturePath;

            //Set the picture itself
            Picture = new BitmapImage(new Uri(Path.GetFullPath(_picturePath)));
        }

        #region Members

        private readonly string _picturePath;
        private BitmapImage _picture;
        private uint _numberOfTimes;

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

        public uint NumberOfTimes
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