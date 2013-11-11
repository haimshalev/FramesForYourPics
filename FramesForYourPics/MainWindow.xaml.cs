using System.Windows;
using System.Windows.Forms;
using FramesForYourPics.Messages;
using MessageBox = System.Windows.MessageBox;

namespace FramesForYourPics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly PhotoList _photoItems = new PhotoList();
        private readonly FramesForYourPicsLogic _logic;

        public MainWindow()
        {
            InitializeComponent();

            //Set the data binding 
            lvPhotos.DataContext = _photoItems;

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
                _logic.GetPhotosFromFolder(new PhotosRequest(folderDialog.SelectedPath, _photoItems , this));
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
            _logic.GetPhotosFromFolder(new PhotosRequest(_photoItems , this));
        }

        /// <summary>
        /// Send a create pages request to the logic class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreatePages_Click(object sender, RoutedEventArgs e)
        {
            //Send a create pages request to the logic class
            _logic.CreateFramedPages(new CreatePagesRequest(_photoItems ,this));
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
               _logic.SetFramePath(new SetFrameCommand(openFileDialog.FileName));
           }
        }

        /// <summary>
        /// Notify the user that he forgot to set a frame
        /// </summary>
        public void NotifyOnAMissingFrame()
        {
            MessageBox.Show("אנא בחר מסגרת תרם הבקשה להדפסה");
        }
    }
}

