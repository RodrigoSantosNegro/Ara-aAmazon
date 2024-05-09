
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

partial class Program
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();

        driver.Navigate().GoToUrl("https://www.amazon.es/");

        var title = driver.Title;

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

        var textBox = driver.FindElement(By.Id("twotabsearchtextbox"));
        var submitButton = driver.FindElement(By.Id("nav-search-submit-button"));

        var rejectCookiesButton = driver.FindElement(By.Id("sp-cc-rejectall-link"));
        rejectCookiesButton.Click();

        textBox.SendKeys("pañales dodot talla 4");
        submitButton.Click();

        

        driver.Quit();
    }
}