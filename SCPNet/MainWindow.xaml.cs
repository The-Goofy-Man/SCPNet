// This code is licensed under the CC BY-NC-SA License. See licence.md in the root of the repo for more information.
//
// last updated: 08/11/2025 by: Dr. Shermon

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;

// BOY DO I LOVE 15 FUCKING LINES OF IMPORTS FOR A SIMPLE BUTTON CLICK :) 08/06/2025 - Dr. Shermon
// Intelisence, please stop autocompleting my code. 08/06/2025 - Dr. Shermon
namespace SCPNet
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
         

        // unless you you change something DONT TOUCH THIS.
        public async void MyButton_OnClick(object sender, RoutedEventArgs e)
        {   
            
            var button = sender as Button;
            var frame = ViewFrame as Frame; // Cast MainFrame to Frame type // i love C# ;(
            var whyDoINeedThis = Ring; // i did it for the colors, also it doesn't work without this, so i had to do it. 09/06/2025 - Dr. Shermon
            if (button != null)
            {
                var id = id_box.Text;
                if (!string.IsNullOrEmpty(id))
                {
                    button.Content = "Logging In..."; // Changes button text on click
                    whyDoINeedThis.IsActive = true; // makes progress ring go brrrrr
                    await Task.Delay(2500);

                    LogInPanel.Visibility = Visibility.Collapsed; // hides the grid, so it doesn't show the progress ring
                    LogInPanel.Opacity = 0; // makes it not see-able
                    
                    frame.Opacity = 1;
                    frame.Visibility = Visibility.Visible;
                }
                else
                {
                    button.Content = "Please Enter A Username, like Dr. Alto Clef ";
                }
            }
            else
            {
                 // cant be bothered to do anything here, if the button is null, then it just won't do anything. 08/06/2025 - Dr. Shermon
                // I know this is bad practice, but I don't care. 08/06/2025 - The-GoofyMan
            }
        }

        public void Search(object sender, TextChangedEventArgs e)
        {
            var doc = DocInput.Text; // Gets the text from the DocInput TextBox
            if (!string.IsNullOrEmpty(doc)) // Checks if the text is not empty
            {
                // Here you would implement the search logic, for now, we just show a message
                var messageDialog = new ContentDialog
                {
                    Title = "Search",
                    Content = $"Searching for: {doc}",
                    CloseButtonText = "OK"
                };
                 messageDialog.XamlRoot = DocInput.XamlRoot;
                 messageDialog.ShowAsync();
            }
            else
            {
                // If the input is empty, show a message
                var messageDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please enter a document name to search.",
                    CloseButtonText = "OK"
                };
                messageDialog.ShowAsync();
            }
        }
    }
}