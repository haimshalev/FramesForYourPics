using System.Collections.ObjectModel;

namespace FramesForYourPics
{
   
    /// <summary>
    /// An observable lost of photo objects, used for binding to a list box
    /// </summary>
    public class PhotoList : ObservableCollection<Photo>
    {
        private readonly MainWindow _mainWindow;

        public PhotoList(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Clear the photo list working in the ui thread
        /// </summary>
        public void ClearInUIThread()
        {
            if (!_mainWindow.Dispatcher.CheckAccess())
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                _mainWindow.Dispatcher.Invoke(new ParameterLessDelegate(ClearInUIThread),
                            new object[] {});
                return;
            }

            // this code can only be reached
            // by the user interface thread
            Clear();
        }

        /// <summary>
        /// Add photo to the photo list working in the ui thread
        /// </summary>
        /// <param name="photo">the photo to add</param>
        public void AddInUIThread(Photo photo)
        {
            if (!_mainWindow.Dispatcher.CheckAccess())
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                _mainWindow.Dispatcher.Invoke(new PhotoParameterDelegate(AddInUIThread),
                            new object[] { photo });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            Add(photo);
        }
    }
}