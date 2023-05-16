using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.Http;
class Program
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://revolut.me/adaman4gk");
        
        Console.WriteLine("Podaj kwotę:");
        string kwota = Console.ReadLine();




        // Przelicz kwotę na TRY (tureckie liry)
        decimal kwotaPLN = decimal.Parse(kwota);
        decimal kursTRY = GetExchangeRate("PLN", "TRY");
        decimal kwotaTRY = kwotaPLN * kursTRY;

        Console.WriteLine($"{kwotaPLN} PLN = {kwotaTRY} TRY");




        // Znajdź element i wpisz wartość
        IWebElement inputField = driver.FindElement(By.CssSelector("[data-testid='amount-field-input']"));
        inputField.Clear();
        inputField.SendKeys(kwota);
    }

    static decimal GetExchangeRate(string baseCurrency, string targetCurrency)
    {
        string accessKey = "access"; 

        string url = $"http://apilayer.net/api/live?access_key={accessKey}&source={baseCurrency}&currencies={targetCurrency}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseContent = response.Content.ReadAsStringAsync().Result;
                dynamic data = JObject.Parse(responseContent);
                decimal rate = data.quotes[$"{baseCurrency}{targetCurrency}"];
                return rate;
            }
        }

        return 0;
    }











   


    }
