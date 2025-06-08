// This code is licensed under the CC BY-NC-SA License. See licence.md in the root of the repo for more information. if in terminal, type 'cat ./../licence.md' to read the license.
//
// last updated: 08/06/2025 by: The-Goofy-Man

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

// BOY DO I LOVE 15 FUCKING LINES OF IMPORTS FOR A SIMPLE BUTTON CLICK :) 08/06/2025 - The-Goofy-Man
// Intelesence, please stop autocompleating my comments. 08/06/2025 - The-Goofy-Man
namespace SCPNet
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
         
        public void MyButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ring = Ring;
            if (button != null)
            {
                button.Content = "Logging In..."; // Changes button text on click
                ring.IsActive = true; // makes ring go brrrrr
                Thread.Sleep(2500); // waits 2.5 seconds for the "1984" Experience 
                MainFrame.Navigate(typeof(Page2));
            }
            else
            {
                // ill write this later 08/06/2025 - The-Goofy-Man
            }
        }
    }
}

// BOY DO I LOVE C# :) 08/06/2024 - The-Goofy-Man