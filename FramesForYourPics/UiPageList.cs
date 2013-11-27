using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FramesForYourPics.Annotations;
using FramesForYourPics.MultiThreadedFramework;

namespace FramesForYourPics
{
    /// <summary>
    /// Holds all pages data and images
    /// </summary>
    public class UiPageList : Communicator
    {
        /// <summary>
        /// Holds the paging metadata
        /// </summary>
        public class PagingData : INotifyPropertyChanged
        {
            private bool _allPagesSeen;
            private int _currentPageNum = 1;
            private int _numOfPhotos;
            private int _numOfPages;
            private bool _previousEnabled;
            private bool _nextEnabled;

            private readonly UiPageList _container;

            public PagingData(UiPageList container)
            {
                _container = container;
            }

            #region Properties

            public int CurrentPageNum
            {
                get { return _currentPageNum; }
                set
                {
                    var previousPageNum = _currentPageNum;

                    //Set the current page num
                    _currentPageNum = value;

                    //Enable or disable buttons
                    PreviousEnabled = _currentPageNum != 1;
                    NextEnabled = _currentPageNum != NumOfPages && NumOfPages != 0;
                    OnPropertyChanged();

                    //Load new page images and disload previous page images
                    LoadNewPageImages(_currentPageNum, previousPageNum);
                }
            }

            public bool PreviousEnabled
            {
                get { return _previousEnabled; }
                set
                {
                    _previousEnabled = value;
                    OnPropertyChanged();
                }
            }

            public bool NextEnabled
            {
                get { return _nextEnabled; }
                set
                {
                    _nextEnabled = value;
                    OnPropertyChanged();
                }
            }

            public bool AllPagesSeen
            {
                get { return _allPagesSeen; }
                set
                {
                    _allPagesSeen = value;
                    OnPropertyChanged();
                }
            }

            public int NumOfPhotos
            {
                get { return _numOfPhotos; }
                set
                {
                    _numOfPhotos = value;

                    //Update the number of pages 
                    NumOfPages = _numOfPhotos / Constants.NumberOfPhotosInAPage + 1;
                    if (_numOfPhotos % Constants.NumberOfPhotosInAPage == 0) NumOfPages--;

                    //Update the Next button
                    NextEnabled = CurrentPageNum != NumOfPages;
                }

            }

            public int NumOfPages
            {
                get { return _numOfPages; }
                set
                {
                    _numOfPages = value;

                    //If we exceeding the number of pages
                    if (CurrentPageNum > NumOfPages)
                        //Set the currentPage to the first page
                        CurrentPageNum = 1;
                }
            }

            #endregion

            /// <summary>
            /// Unload all images in the previous page and load all the images in the current page
            /// </summary>
            /// <param name="currentPageNum">the number of the current page</param>
            /// <param name="previousPageNum">the number of the previous page</param>
            private void LoadNewPageImages(int currentPageNum, int previousPageNum)
            {
                //unload all images from the previous page
                if (_container._pageList.ContainsKey(previousPageNum))
                foreach (var photo in _container._pageList[previousPageNum])
                    photo.EnablePicture(false);

                //Enable all the images in the current page
                if (_container._pageList.ContainsKey(currentPageNum))
                foreach (var photo in _container._pageList[currentPageNum])
                    photo.EnablePicture(true);
            }

            /// <summary>
            /// Restart paging data to the initial state
            /// </summary>
            public void Restart()
            {
                AllPagesSeen = false;

                NumOfPhotos = 0;

                //The previous enabled and next enabled bools will update accordingly
                CurrentPageNum = 1;
            }

            #region NotifyPropertyProps
            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            } 
            #endregion
        }

        public PagingData Paging;

        private readonly Dictionary<int, PhotoList> _pageList;

        private readonly MainWindow _mainWindow;

        public UiPageList(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            _pageList = new Dictionary<int, PhotoList> { { 1, new PhotoList(_mainWindow) } };

            Paging = new PagingData(this);
        }

        /// <summary>
        /// Set the ui to the next page
        /// </summary>
        /// <returns>the current photo list to show</returns>
        public PhotoList SetNextPage()
        {
            return _pageList[++Paging.CurrentPageNum];
        }

        /// <summary>
        /// Set the ui to the previous page 
        /// </summary>
        /// <returns>the current photo list to show</returns>
        public PhotoList SetPreviousPage()
        {
            return _pageList[--Paging.CurrentPageNum];

        }

        /// <summary>
        /// Returns the photo list of the current page
        /// </summary>
        /// <returns></returns>
        public PhotoList GetCurrentPage()
        {
            return _pageList.ContainsKey(Paging.CurrentPageNum) ? _pageList[Paging.CurrentPageNum] : null;
        }

        /// <summary>
        /// Add the photo to the ui Page list
        /// </summary>
        /// <param name="photoPath">the path to the photo path</param>
        public void AddPhoto(string photoPath)
        {
            //Update the number of photos and all the relevent paging data
            Paging.NumOfPhotos++;

            //Add the photo to one of the lists
            AddPhotoToAPhotoList(photoPath);
        }

        /// <summary>
        /// Adds the photo to the correct photo list
        /// </summary>
        /// <param name="photoPath">the path to the photo path</param>
        private void AddPhotoToAPhotoList(string photoPath)
        {
            //If the page list isn't set
            if (!_pageList.ContainsKey(Paging.NumOfPages))
                //Create it
                _pageList.Add(Paging.NumOfPages, new PhotoList(_mainWindow));

            //If we in the correct page , show the image
            var loadImage = (Paging.CurrentPageNum == Paging.NumOfPages);

            //Add the photo to the created photo list
            _pageList[Paging.NumOfPages].AddInUIThread(new Photo(photoPath, loadImage));

            //Update the data context if we are in the right UI page
            if (loadImage)
                _mainWindow.SetListViewDataBinding(_pageList[Paging.NumOfPages]);
        }

        /// <summary>
        /// Return the number of photos in the page list
        /// </summary>
        /// <returns>the number of photos in the page list</returns>
        public int GetNumberOfPhotos()
        {
            return _pageList.Values.SelectMany(photolist => photolist).Aggregate(0, (current, photo) => current + photo.NumberOfTimes);
        }

        /// <summary>
        /// Get all the photo lists from the page list
        /// </summary>
        /// <returns>a photo list collection</returns>
        public Dictionary<int, PhotoList>.ValueCollection GetPages()
        {
            return _pageList.Values;
        }

        /// <summary>
        /// Clear all the data stored in the page list
        /// </summary>
        public void Clear()
        {
            //Remove all the photos from each photo list
            foreach (var photolist in _pageList.Values)
            {
                photolist.ClearInUIThread();
            }

            //Clear the dictionary
            _pageList.Clear();

            //Restart the paging data
            Paging.Restart();
        }
    }
}