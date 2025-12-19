using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ExpenseTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // Set the window icon
            try
            {
                BitmapImage iconBitmap = new BitmapImage();
                iconBitmap.BeginInit();
                iconBitmap.UriSource = new Uri("Resources/expense-icon.png", UriKind.Relative);
                iconBitmap.CacheOption = BitmapCacheOption.OnLoad;
                iconBitmap.EndInit();
                iconBitmap.Freeze();
                this.Icon = iconBitmap;
            }
            catch (Exception ex)
            {
                // If icon loading fails, continue without it
                System.Diagnostics.Debug.WriteLine($"Warning: Could not load icon - {ex.Message}");
            }
        }
    }
}
