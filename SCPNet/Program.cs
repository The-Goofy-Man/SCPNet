using HtmlAgilityPack;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http;
// shouldnt this be using System..Net; ;3
using System.Text.RegularExpressions;
// man just call this regex, it's not that hard.

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("SCPNet");
        Console.WriteLine("v.[Data Expunged].0.0.1");

        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("--------------------"); // ASCII break statement

        // this is to loop the program until the personell decides to exit
        //
        while (true) 
        {
            string welcomeMessage = "Enter your Document, it must be exact, type clear to clear the console if you need a reminder on how to search, type 'help'";
            Console.Write(welcomeMessage);
            Console.WriteLine("");
            Console.WriteLine("User O5-1 [The Administrator]: ");
            string Document = Console.ReadLine();
            if (Document.ToLower() == "clear")
            {
                Console.Clear();
                Console.WriteLine(welcomeMessage);
                continue; // Restart loop after clearing
            }
            if (Document.ToLower() == "help")
            {
                Console.WriteLine("Type the SCP document name exactly (e.g., SCP-173 or scp-173). Type 'clear' to clear the console. to get subdocuments use the ~ like [Ex Doc]~[Ex Doc]");
                continue; // Restart loop after help
            }
            string URL = "https://scp-wiki.wikidot.com/" + Document.Replace(" ", "-").Replace("SCP-", "scp-");
            using HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(URL);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                continue;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Error 404: Document not found. Please check the document name and try again.");
                continue;
            }
            else if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: Unable to fetch document for an anomalous reason. (HTTP status code: {(int)response.StatusCode}).");
                continue;
            }

            string htmlContent = await response.Content.ReadAsStringAsync();

            // Check for Wikidot 404 marker in the HTML content
            if (htmlContent.Contains("This page doesn't exist."))
            {
                Console.WriteLine("Error 404: Document not found on SCPNet. Please check the correct document name and try again");
                Console.WriteLine($"{Document} is the document name you entered");
                continue;
            }

            // Save to file to avoid re-fetching and to allow for reading later
            string fileName = $"{Document}.html";
            await File.WriteAllTextAsync(fileName, htmlContent);

            string fileContent = await File.ReadAllTextAsync(fileName);

            // Parse and display only the <div id="page-content"> // why can you put comments in comments? I don't know, but I like it! //
            var doc = new HtmlDocument();
            doc.LoadHtml(fileContent);
            var pageContentDiv = doc.GetElementbyId("page-content");
            if (pageContentDiv != null)
            {
                // Replace <em> and <i> tags with underscores for italics in console
                foreach (var italicNode in pageContentDiv.Descendants().Where(n =>
                    n.Name == "em" || n.Name == "i").ToList())
                {
                    var italicText = "_" + italicNode.InnerText + "_";
                    italicNode.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(italicText), italicNode);
                }

                // Decode HTML entities to Unicode characters
                string decodedText = WebUtility.HtmlDecode(pageContentDiv.InnerText.Trim());

                // Add an empty line after lines like "Item #: ..." or "rating: ..."
                string formattedText = Regex.Replace(
                    decodedText,
                    @"^(.*?:.*)$",
                    "$1\n",
                    RegexOptions.Multiline);

                Console.WriteLine("--------------------"); // ASCII break before output
                Console.WriteLine(formattedText);
                Console.WriteLine("--------------------"); // ASCII break after output
            }
            else
            {
                Console.WriteLine("Could not find document, the SCPNet is down. Contact Database staff for further info.");
            }
        }
    }
}