using Microsoft.Toolkit.Wpf.UI.XamlHost;
using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;

namespace ModernWpfDemo
{
    public partial class MainWindow
    {
        private ProgressRing progressRing;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            progressRing.IsActive = true;

            Geolocator geolocator = new Geolocator() { DesiredAccuracyInMeters = 5 };
            Geoposition pos = await geolocator.GetGeopositionAsync();
            string longitude = pos.Coordinate.Point.Position.Longitude.ToString();
            string latitude = pos.Coordinate.Point.Position.Latitude.ToString();
            LongBlock.Text = longitude;
            LatBlock.Text = latitude;

            progressRing.IsActive = false;
        }

        private void WindowsXamlHost_ChildChanged(object sender, EventArgs e)
        {
            if (sender is WindowsXamlHost xamlHost && xamlHost.Child is ProgressRing ring)
            {
                progressRing = ring;
                progressRing.Width = 150;
            }
        }
    }
}
