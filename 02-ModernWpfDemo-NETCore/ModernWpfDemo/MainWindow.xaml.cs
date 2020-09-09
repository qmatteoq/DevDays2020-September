using System;
using Windows.Devices.Geolocation;

namespace ModernWpfDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator() { DesiredAccuracyInMeters = 5 };
            Geoposition pos = await geolocator.GetGeopositionAsync();
            string longitude = pos.Coordinate.Point.Position.Longitude.ToString();
            string latitude = pos.Coordinate.Point.Position.Latitude.ToString();
            LongBlock.Text = longitude;
            LatBlock.Text = latitude;

            await TheMap.TrySetViewAsync(pos.Coordinate.Point, 15.0);
        }
    }
}
