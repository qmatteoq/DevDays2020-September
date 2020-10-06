using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUISample.Services;
using System.Threading.Tasks;
using System.Reflection;

// The Blank Window item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinUISample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Activated += MainWindow_Activated;
            splitView.Loaded += SplitView_Loaded;
        }

        private void SplitView_Loaded(object sender, RoutedEventArgs e)
        {
            string currentTheme = splitView.RequestedTheme == ElementTheme.Default ? App.Current.RequestedTheme.ToString() : splitView.RequestedTheme.ToString();
            themePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            RegistryService registryService = new RegistryService();
            if (registryService.IsFirstTimeLaunch())
            {
                FirstLaunchTip.IsOpen = true;
            }
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.IsEnabled = false;
            AutoSaveTip.IsOpen = true;
            DataProgress.IsActive = true;

            SaveData(txtName.Text, txtSurname.Text, datePicker.Date.Value);

            await Task.Delay(5000);
            DataProgress.IsActive = false;
            myButton.IsEnabled = true;
            AutoSaveTip.IsOpen = false;
        }

        public void SaveData(string name, string surname, DateTimeOffset date)
        {
            string file = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\file.txt";
            string data = $"Name: {name} - Surname: {surname} - Birth date: {date:d}";
            File.WriteAllText(file, data);
        }

        private void OnResetApp(object sender, RoutedEventArgs e)
        {
            RegistryService registryService = new RegistryService();
            registryService.ResetApp();
        }

        void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
            if (selectedTheme != null)
            {
                ((sender as RadioButton).XamlRoot.Content as SplitView).RequestedTheme = GetEnum<ElementTheme>(selectedTheme);
            }
        }

        private TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
