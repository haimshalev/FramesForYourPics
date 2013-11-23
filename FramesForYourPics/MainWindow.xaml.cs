using System.Windows;
using System.Windows.Forms;
using FramesForYourPics.Messages;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace FramesForYourPics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly UiPageList _uiPageList;
        private readonly FramesForYourPicsLogic _logic;

        public MainWindow()
        {
            InitializeComponent();

            _uiPageList = new UiPageList(this);

            //Set the data binding 
            btn_NextPage.DataContext = _uiPageList.Paging;
            btn_PreviousPage.DataContext = _uiPageList.Paging;

            //Create an FramesForYourPicsLogic instance
            _logic = new FramesForYourPicsLogic();
        }

        /// <summary>
        /// Opens a folder browser dialog which allows the user to select the input photos folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoosePhotosFolder_Click(object sender, RoutedEventArgs e)
        {
            //Open a folder dialog
            var folderDialog = new FolderBrowserDialog();
            var dialogResult = folderDialog.ShowDialog();

            //If the user choose a folder
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //Send the folder path to the logic module
                _logic.Notify(new PhotosRequest(folderDialog.SelectedPath, _uiPageList, this));

                //Set the data binding of the page to the new created photo list
                lvPhotos.DataContext = _uiPageList.GetCurrentPage();

                //Set the text box content
                tbInputFolder.Text = folderDialog.SelectedPath;
            }

        }

        /// <summary>
        /// Send a photo folder refresh request to the logic class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshPhostosFolder_Click(object sender, RoutedEventArgs e)
        {

            //Send a refresh request for the logic module
            _logic.Notify(new PhotosRequest(_uiPageList, this));

        }

        /// <summary>
        /// Send a create pages request to the logic class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreatePages_Click(object sender, RoutedEventArgs e)
        {
            btnCreatePages.Content = "מייצר דפים להדפסה , אנא המתן עד להעלמות כל התמונות";

            //Send a create pages request to the logic class
            _logic.Notify(new CreatePagesRequest(_uiPageList, this));

        }

        /// <summary>
        /// Give the user a chance to select a frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseFramePath_Click(object sender, RoutedEventArgs e)
        {
            //Open a file dialog 
            // Create an instance of the open file dialog box.
            var openFileDialog = new OpenFileDialog
                {
                    Filter = @"png Files (.png)|*.png|All Files (*.*)|*.*",
                    FilterIndex = 1,
                    Multiselect = false
                };

            // Call the ShowDialog method to show the dialog box.
           if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           {
               //set the current frame
               _logic.Notify(new SetFrameCommand(openFileDialog.FileName));

               //Set the text box content
               tbFramePath.Text = openFileDialog.FileName;
           }

        }

        /// <summary>
        /// Notify the user that he forgot to set a frame
        /// </summary>
        public void NotifyOnAMissingFrame()
        {
            if (!Dispatcher.CheckAccess())
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                Dispatcher.Invoke(new ParameterLessDelegate(NotifyOnAMissingFrame),
                            new object[] { });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            MessageBox.Show("אנא בחר מסגרת");
        }

        /// <summary>
        /// Notify the user that u pictures are missing 
        /// </summary>
        /// <param name="u">the number of photos missing to fill all pages</param>
        public void NotifyOnMissingPhotos(int u)
        {
            if (!Dispatcher.CheckAccess())
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                Dispatcher.Invoke(new IntParameterDelegate(NotifyOnMissingPhotos),
                            new object[] { u });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            var msg = "שים לב ישנם " + u + " מקומות נוספים בדף , לחץ שוב לאישור";
            btnCreatePages.Content = msg;
        }

        /// <summary>
        /// Bind the photo list to the main list view control
        /// </summary>
        /// <param name="photoList">the photo list to bind</param>
        public void SetListViewDataBinding(PhotoList photoList)
        {
            if (!Dispatcher.CheckAccess())
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                Dispatcher.Invoke(new PhotoListParameterDelegate(SetListViewDataBinding),
                            new object[] { photoList });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            lvPhotos.DataContext = photoList;
        }

        /// <summary>
        /// Restore all the buttons content
        /// </summary>
        public void RestoreDefatultContent()
        {
            if (!Dispatcher.CheckAccess())
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                Dispatcher.Invoke(new ParameterLessDelegate(RestoreDefatultContent),
                            new object[] { });
                return;
            }

            // this code can only be reached
            // by the user interface thread
            btnCreatePages.Content = "ייצא דפים להפסה";
        }

        /// <summary>
        /// Iterate the page list and change the data context if the list view accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NextPage_Click(object sender, RoutedEventArgs e)
        {
            SetListViewDataBinding(_uiPageList.SetNextPage());
        }

        /// <summary>
        /// Iterate the page list and change the data context if the list view accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            SetListViewDataBinding(_uiPageList.SetPreviousPage());
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

