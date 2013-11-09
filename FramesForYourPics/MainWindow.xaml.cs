using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;

namespace FramesForYourPics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string TestPhoto = @"Resources\TestPhoto.jpg";
        private const string MyFrame = @"Resources\MyFrame.png";
        private const string OutputPhoto = @"Resources\OutputPhoto.jpeg";
        private const string OutputPage = @"Resources\OutputPage.jpeg";

        private readonly PhotoList _photoItems = new PhotoList();

        public MainWindow()
        {
            InitializeComponent();

            //Set the data binding 
            lvPhotos.DataContext = _photoItems;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Image outputImage;

            _photoItems.Add(new Photo(TestPhoto));

            //Merge the two photos
            using (Image photo = Image.FromFile(TestPhoto), mainFrame = Image.FromFile(MyFrame))
            {
                outputImage = ImageManipulationUtils.MergePhotoAndFrame(photo, mainFrame);

                outputImage.Save(OutputPhoto, ImageFormat.Jpeg);
            }

            //Concatenate the two photos
            using (Image photo1 = Image.FromFile(OutputPhoto), photo2 = Image.FromFile(OutputPhoto))
            {
                outputImage = ImageManipulationUtils.ConcatenateTwoImagesHorizontal(photo1, photo2);

                outputImage.Save(OutputPage, ImageFormat.Jpeg);
            }

            //Concatenate the two lines

            using (Image line1 = Image.FromFile(OutputPage), line2 = Image.FromFile(OutputPage))
            {
                outputImage = ImageManipulationUtils.ConcatenateTwoImagesVertical(line1, line2);
            }

            if (outputImage != null)
                outputImage.Save(OutputPage, ImageFormat.Jpeg);
        }
    }
}

