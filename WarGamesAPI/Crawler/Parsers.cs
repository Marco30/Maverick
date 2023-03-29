using HtmlAgilityPack;
using System.Text.RegularExpressions;
using WarGamesAPI.Model;

namespace WarGamesAPI.Crawler;

public class Parsers
{
    /*Parse UserData from page html*/
    public static UserDto ParseHtmlUserData(string htmlPage)
    {

        UserDto userData = new UserDto();
        Address address = new Address();
        // Loads the page you want to scrape/Crawl
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlPage);



        // ParseErrors is an ArrayList containing any errors from the Load statement
        if (htmlDocument.ParseErrors != null && htmlDocument.ParseErrors.Count() > 0)
        {
            // Handle any parse errors as required
            Console.WriteLine("parse errors");
            return null;
        }

        if (htmlDocument.DocumentNode != null)
        {
            Console.WriteLine("text från sidan");
            //xpath to body tag to verify page
            HtmlAgilityPack.HtmlNode bodyNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body");

            //check if body tag exist in page to verify that we can scrape data
            if (bodyNode != null)
            {


                var givenName = htmlDocument.DocumentNode.SelectSingleNode("//span[@title='Detta är personens tilltalsnamn']");

                userData.FirstName = givenName.InnerText.Trim();

                var surname = htmlDocument.DocumentNode.SelectSingleNode("//span[@title='Detta är ett efternamn']");

                userData.LastName = surname.InnerText.Trim();

                userData.FullName = givenName.InnerText.Trim() + " " + surname.InnerText.Trim();

                // we set gender and city in for loop

                foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//div[@class='col_block1']"))
                {

                    string innerChildNodesText = node.InnerText.Trim();

                    string[] lines = innerChildNodesText.Split(
                        new string[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );

                    if (lines != null && lines.Length > 1)
                    {

                        /* Console.WriteLine(lines[0]);
                     Console.WriteLine(lines[1]);*/

                        if (lines[0] == "Län")
                        {
                            string[] cityArray = lines[1].Split(' ');
                            cityArray[0] = cityArray[0].Remove(cityArray[0].Length - 1);
                            address.City = CapitalizeFirstLetter(cityArray[0]);
                        }


                        if (lines[0] == "Kön")
                        {
                            userData.Gender = CapitalizeFirstLetter(lines[1]);
                            break;
                        }

                    }

                }

                var phone = htmlDocument.DocumentNode.SelectNodes("/html/body/div[1]/div[3]/div/div/div[1]/div/div[10]/div/div/a");
                    
                if(phone!= null) { 

                    if(phone != null) {
                        for (int i = 0; i < phone.Count; i++)
                        {

                            var phonenumber = PhonenumberTrim(phone[i].InnerText.Trim());

                            phonenumber = PhonenumberTrim(phonenumber);

                            if (CheckIfPhonenumberIsMobilePhone(phonenumber))
                            {
                                userData.MobilePhoneNumber = phonenumber;
                            }
                            else
                            {
                                userData.PhoneNumber = phonenumber;
                            }

                        }
                    }
                    

                }

                var Street = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div/div/div[1]/div/div[7]/div[1]/span[2]");

                address.Street = Street.InnerText.Trim();

                /* var city = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div/div/div[1]/div/div[7]/div[1]/div[4]/span[2]");

                address.City = CityTrim(city.InnerText.Trim()); */

                var zipCodeAndMunicipality = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div/div/div[1]/div/div[7]/div[1]/span[3]");

                string[] arrayZipCodeAndMunicipality = zipCodeAndMunicipality.InnerText.Trim().Split(' ');

                address.ZipCode = arrayZipCodeAndMunicipality[0];

                address.Municipality = arrayZipCodeAndMunicipality[1];

                //userData.Address = address;

                return userData;

            }

        }

        // bad error
        Console.WriteLine("bad error");
        return null;

    }

    public static string CapitalizeFirstLetter(string s)
    {
        if (String.IsNullOrEmpty(s))
            return s;
        if (s.Length == 1)
            return s.ToUpper();
        return s.Remove(1).ToUpper() + s.Substring(1);
    }

    public static string PhonenumberTrim(string phonenumber)
    {

        phonenumber = Regex.Replace(phonenumber, @"\s+", "");
        phonenumber = Regex.Replace(phonenumber, @"-+", "");


        return phonenumber;
    }

    public static string CityTrim(string city)
    {
        string[] cityArray = city.Split(' ');

        city = cityArray[0].TrimEnd('s');

        return city;
    }

    public static string GetStringBetween(string STR, string FirstString, string LastString)
    {
        string FinalString;
        int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
        int Pos2 = STR.IndexOf(LastString);
        FinalString = STR.Substring(Pos1, Pos2 - Pos1);
        return FinalString;
    }

    public static bool CheckIfPhonenumberIsMobilePhone(string phonenumber)
    {

        var isMobilePhone = false;


        var rx = new Regex(@"^(([+]46)\s*(7)|07)[02369]\s*(\d{4})\s*(\d{3})$");

        if (rx.IsMatch(phonenumber))
        {
            isMobilePhone = true;
        }
        else
        {
            isMobilePhone = false;
        }


        return isMobilePhone;
    }

    
}