using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using System.Net;
using HtmlAgilityPack;
using OpenQA.Selenium.Support.UI;

namespace WarGamesAPI.Crawler;

public class Crawlers
{

    /*Selenium Get User Data Crawler function*/
    public static UserDto SeleniumGetUserInfoPagesCrawler(string pageURL, string socialSecurityNumber)
    {

        var userdata = new UserDto();

        var options = new ChromeOptions()
        {

        };

        // options.AddArguments(new List<string>() { "headless", "disable-gpu" });
        options.AddArguments("headless");
        options.AddArguments("window-size=1920,1080");
        options.AddArguments("start-maximized");



        IWebDriver browser = null;
        try
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            browser = new ChromeDriver(options);
        }
        catch (Exception e)
        {
            var chromeVersion = Parsers.GetStringBetween(e.Message, " is ", " with");
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), chromeVersion);
            browser = new ChromeDriver(options);

            // Console.WriteLine("{0} Exception caught.", e);
        }

        browser.Navigate().GoToUrl(pageURL);
        
  
        browser.FindElement(By.Id("inpField10")).SendKeys(socialSecurityNumber);
        browser.FindElement(By.Id("inpField10")).Submit();
        if (IsElementPresent(browser, By.XPath("/html/body/div[1]/div/div/div/div[2]/div/button[2]")))
        {
            browser.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[2]/div/button[2]")).Click();
        }
        Thread.Sleep(3000);

        browser.FindElement(By.XPath("/html/body/div[3]/div[2]/div/div[2]/div[2]/a")).Click();
        var userInfoPage = browser.PageSource;

        browser.Close();

        userdata = Parsers.ParseHtmlUserData(userInfoPage);

        return userdata;
    }

    private static bool IsElementPresent(IWebDriver browser, By by)
    {
        try
        {
            browser.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    /*function that is good for getting cookies and for extracting an html value from a static page. 
     * you have to use xpath to find the value, it shows how to scrape/Crawl.
     * but if you want to scrape specific values, you must do a specific function that does it for the page you want to scrape/Crawl.*/
    private static string StaticPagesCrawlerGetCookieData(string pgaeURL, bool getCookie = false, string XPatchToValue = null)
    {
        var cookieData = "";

        try
        {

            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest += request =>
            {
                // gets access to the cookie container
                var cookieContainer = request.CookieContainer;
                //  gets access to the request headers
                var headers = request.Headers;
                return true;
            };
            web.PostResponse += (request, response) =>
            {
                // response headers
                var headers = response.Headers;
                // cookies
                CookieCollection cookies = response.Cookies;

                //cookieData = cookies[0] + ";" + cookies[1] + ";";
                for (int i = 0; i < cookies.Count; i += 1)
                {
                    cookieData += cookies[i] + ";";
                }
                // Console.WriteLine(cookieData);
                // Console.WriteLine("cookie test");
            };

            // Loads the page you want to scrape/Crawl
            HtmlDocument document = web.Load(pgaeURL);

            var htmlDocument = document;

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDocument.ParseErrors != null && htmlDocument.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
                // Console.WriteLine("Handle any parse errors as required");
                return "error";
            }
            else
            {
                if (htmlDocument.DocumentNode != null)
                {
                    //Console.WriteLine("text från sidan");
                    //xpath to body tag to verify page
                    HtmlAgilityPack.HtmlNode bodyNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body");

                    //check if body tag exist in page to verify that we can scrape data
                    if (bodyNode != null)
                    {
                        // if true we send page cookie
                        if (getCookie)
                        {
                            return cookieData;
                        }

                        HtmlAgilityPack.HtmlNode bodyNodeValue = htmlDocument.DocumentNode.SelectSingleNode(XPatchToValue);
                        //Console.WriteLine(bodyNodeValue.InnerText);
                        string tagValue = bodyNodeValue.InnerText;

                        return tagValue;

                        /*var divs = htmlDocument.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("col-md-4 col-md-offset-4")).ToList();
                       
                        foreach (var div in divs)
                        {
                            Console.WriteLine(div.InnerHtml);
                            Console.WriteLine(div.InnerText);
                        }*/
                        //Console.WriteLine("Do something with bodyNode");
                        // Do something with bodyNode
                    }
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "error";
        }

        return "error";

    }

    
}