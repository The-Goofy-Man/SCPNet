// This code is licensed under the CC BY-NC-SA License. See licence.md in the root of the repo for more information.
//
// last updated: 08/11/2025 by: Dr. Sherman


// useful geometry/spacing : https://learn.microsoft.com/en-us/windows/apps/design/basics/content-basics
using ABI.System;
using HtmlAgilityPack; // THIS FUCKING LIBRARY IS A GODSEND, it makes parsing HTML so much easier! 08/06/2025 - Dr. Sherman
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Web.Http;
using static System.Collections.Specialized.BitVector32;
using HttpClient = System.Net.Http.HttpClient;
using Microsoft.UI.Xaml.Documents;
using System.Xml;
using Windows.UI.Text;


namespace SCPNet
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task HtmlLoggerTaskTask(HtmlNode message)
        {
            string logFilePath = "app.log"; // You can change the file name or path as needed
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";
            await File.AppendAllTextAsync(logFilePath, logEntry);
        }
        // unless you you change something in MainWindow.xaml DON'T TOUCH THIS.
        public async void MyButton_OnClick(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            var frame = ViewFrame as Frame; // Cast MainFrame to Frame type // i love C# ;(
            var whyDoINeedThis =
                Ring; // i did it for the colors, also it doesn't work without this, so i had to do it. 09/06/2025 - Dr. Sherman
            if (button != null)
            {
                var id = id_box.Text;
                if (!string.IsNullOrEmpty(id))
                {
                    button.Content = "Logging In..."; // Changes button text on click
                    whyDoINeedThis.IsActive = true; // makes progress ring go brrrrr
                    await Task.Delay(2500);

                    LogInPanel.Visibility =
                        Visibility.Collapsed; // hides the grid, so it doesn't show the progress ring
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
                // cant be bothered to do anything here, if the button is null, then it just won't do anything. 08/06/2025 - Dr. Sherman
                // I know this is bad practice, but I don't care. 08/06/2025 - The-GoofyMan
            }
        }

        private string ConvertBasicHtmlToXaml(string html)
        {
            // Very basic conversion for demonstration purposes
            string xaml = html
                .Replace("<b>", "<Run FontWeight=\"Bold\">").Replace("</b>", "</Run>")
                .Replace("<i>", "<Run FontStyle=\"Italic\">").Replace("</i>", "</Run>")
                .Replace("<u>", "<Run TextDecorations=\"Underline\">").Replace("</u>", "</Run>")
                .Replace("<br>", "<LineBreak/>").Replace("<br/>", "<LineBreak/>")
                .Replace("<p>", "<Paragraph>").Replace("</p>", "</Paragraph>");
            // Wrap in a Section if not already
            if (!xaml.Contains("<Section"))
                xaml = $"<Section xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph>{xaml}</Paragraph></Section>";
            return xaml;
        }

        // NO TOUCHY >:( - Dr. Sherman
        private void SetHtmlToRichTextBlock(string html)
        {
            // Very basic HTML parsing for demonstration
            var paragraph = new Paragraph();
            int idx = 0;
            while (idx < html.Length)
            {
                if (html.Substring(idx).StartsWith("<b>"))
                {
                    idx += 3;
                    int end = html.IndexOf("</b>", idx, StringComparison.OrdinalIgnoreCase);
                    if (end == -1) break;
                    string boldText = html.Substring(idx, end - idx);
                    paragraph.Inlines.Add(new Run { Text = boldText, FontWeight = new Windows.UI.Text.FontWeight(2600) });
                    idx = end + 4;
                }
                else if (html.Substring(idx).StartsWith("<i>"))
                {
                    idx += 3;
                    int end = html.IndexOf("</i>", idx, StringComparison.OrdinalIgnoreCase);
                    if (end == -1) break;
                    string italicText = html.Substring(idx, end - idx);
                    paragraph.Inlines.Add(new Run { Text = italicText, FontStyle = Windows.UI.Text.FontStyle.Italic });
                    idx = end + 4;
                }
                else if (html.Substring(idx).StartsWith("<u>"))
                {
                    idx += 3;
                    int end = html.IndexOf("</u>", idx, StringComparison.OrdinalIgnoreCase);
                    if (end == -1) break;
                    string underlineText = html.Substring(idx, end - idx);
                    var run = new Run { Text = underlineText };
                    run.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
                    paragraph.Inlines.Add(run);
                    idx = end + 4;
                }
                else if (html.Substring(idx).StartsWith("<br>") || html.Substring(idx).StartsWith("<br/>"))
                {
                    paragraph.Inlines.Add(new LineBreak());
                    idx += html.Substring(idx).StartsWith("<br/>") ? 5 : 4;
                }
                else if (html.Substring(idx).StartsWith("<p>"))
                {
                    idx += 3;
                }
                else if (html.Substring(idx).StartsWith("</p>"))
                {
                    idx += 4;
                    // Optionally, add a new paragraph here if you want to support multiple paragraphs
                }
                else
                {
                    int nextTag = html.IndexOf('<', idx);
                    if (nextTag == -1) nextTag = html.Length;
                    string text = html.Substring(idx, nextTag - idx);
                    paragraph.Inlines.Add(new Run { Text = text });
                    idx = nextTag;
                }
            }

            Unit.Blocks.Clear();
            Unit.Blocks.Add(paragraph);
        }

        public async void Search(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DocInput.Text))
            {
                string docname = DocInput.Text.ToLower().Replace(" ", "-");
                string url = $"https://scp-wiki.wikidot.com/{docname}";

                string htmlContent;
                using (HttpClient client = new HttpClient())
                {
                    htmlContent = await client.GetStringAsync(url);
                }

                // Save to file to avoid re-fetching and to allow for reading later
                string fileName = $"{docname}.html";
                await File.WriteAllTextAsync(fileName, htmlContent);

                string fileContent = await File.ReadAllTextAsync(fileName);

                // Parse and display only the <div id="page-content">
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(fileContent);
                var pageContentDiv = doc.GetElementbyId("page-content");

                if (pageContentDiv != null)
                {
                    SetHtmlToRichTextBlock(pageContentDiv.InnerHtml);
                }
                else
                {
                    warningbar.IsOpen = true; // Show warning if the div is not found
                }



                // You can now use pageContentDiv.InnerHtml to display the content in your app
                // For example, set it to a TextBlock or WebView
            }
        }
    }
}