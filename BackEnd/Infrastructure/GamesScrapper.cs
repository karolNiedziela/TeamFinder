using BackEnd.Data;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.WebScrapper
{
    public static class GamesScrapper 
    {
        public static ScrapingBrowser _scrapingBrowser = new ScrapingBrowser();

        public async static Task<List<string>> GetPageDetails(string url)
        {
            var pageDetails = new List<string>();
            var htmlNode = await GetHtmlAsync(url);
            
            foreach (var row in htmlNode.SelectNodes("//tr"))
            {
                foreach (var cell in row.SelectNodes("//td"))
                {
                    var description = cell.InnerText;
                    
                    description = description.Replace("\t", "");
                    description = description.Replace("\n", "");

                    if (description == "Unlock the top 100 and detailed metrics in Newzoo ProLearn more ")
                    {
                        return pageDetails;
                    }

                    if (description.Length >= 4)
                    {
                        pageDetails.Add(description);
                    }                 
                }
            }

            return new List<string>();
        }

        public static List<Game> AssignData(List<string> pageDetails)
        {

            var games = new List<Game>();
            for (int i = 0; i < pageDetails.Count(); i += 2)
            {
                var game = new Game();

                game.Name = pageDetails[i];
                game.Publisher = pageDetails[i + 1];

                games.Add(game);
            }

            return games;
        }
        public async static Task<HtmlNode> GetHtmlAsync(string url)
        {
            WebPage webpage = await _scrapingBrowser.NavigateToPageAsync(new Uri(url));

            return webpage.Html;
        }
    }
}
