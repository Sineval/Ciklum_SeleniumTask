using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace CiklumSeleniumTask.PageObjects
{
    class StiboPage
    {
        private IWebDriver driver;
        private Actions action;
        private WebDriverWait wait;

        public StiboPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
            action = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        [FindsBy(How = How.ClassName, Using = "search")]
        public IWebElement SearchField { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='stats']")]
        public IWebElement EnergyText { get; set; }

        [FindsBy(How = How.ClassName, Using = "ais-hits--item")]
        public IList<IWebElement> SearchResultsList { get; set; } // <- For some reason is 'null'...

        [FindsBy(How = How.XPath, Using = "//*[@id='pagination']/div/ul/li[6]/a")]
        public IWebElement Pagination { get; set; }

        [FindsBy(How = How.ClassName, Using = "icon-facebook")]
        public IWebElement FacebookIcon { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='hs_menu_wrapper_my_menu']/ul/li[7]/a")]
        public IWebElement BlogLink { get; set; }

        [FindsBy(How = How.Name, Using = "email")]
        public IWebElement BlogEmailField { get; set; }

        [FindsBy(How = How.ClassName, Using = "hs-error-msgs")]
        public IList<IWebElement> BlogEmailFieldError { get; set; } // <- For some reason is 'null'...

        [FindsBy(How = How.XPath, Using = "//input[@type='submit']")]
        public IWebElement BlogEmailSend { get; set; }

        [FindsBy(How = How.ClassName, Using = "submitted-message")]
        public IWebElement BlogEmailCorrectMessage { get; set; }

        [FindsBy(How = How.ClassName, Using = "icon-linkedin")]
        public IWebElement LinkedInIcon { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(text(), 'Products')]")]
        public IWebElement ProductsLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(text(), 'Customer Master Data Management')]")]
        public IWebElement CmdmLink { get; set; }

        public void SearchForEnergy()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SearchField)).Click();
            action.SendKeys("energy").Build().Perform();
        }

        public void SearchForAboutUsLink()
        {
            var check = false;
            var page = 1;

            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(1)); // Dirty hack to avoid "Stale Element Exception" 
                IList<IWebElement> SearchResultsList = driver.FindElements(By.ClassName("ais-hits--item"));
                if (SearchResultsList.Any(r => r.Text.Contains("About Us")))
                {
                    Trace.WriteLine(string.Format("\n'About Us' link found on search result page: {0}\n", page));
                    var url = SearchResultsList.First(f => f.Text.Contains("About Us"));
                    driver.Navigate().GoToUrl(url.Text.Split('\n')[1].Split('\r')[0]); // Workaround, since I couldn't find way to click on the link...
                    check = true;
                }
                else
                {
                    page++;
                    Pagination.Click();
                }
            }
            while (check == false);
        }

        public void ClickOnBlogLink()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(BlogLink)).Click();
        }

        public void BlogClickOnEmailField()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(BlogEmailField)).Click();
            BlogEmailField.Clear();
        }

        public void BlogWrongEmailGetErrorMessage()
        {
            IList<IWebElement> BlogEmailFieldError = driver.FindElements(By.ClassName("hs-error-msgs"));
            foreach (IWebElement e in BlogEmailFieldError)
            {
                Trace.WriteLine(e.Text);
            }
        }

        public void BlogEmailClickSend()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(BlogEmailSend)).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("submitted-message")));
        }

        public LinkedInPage GoToLinkedInPage()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(LinkedInIcon)).Click();
            return new LinkedInPage(driver);
        }

        public void GoToCMDM()
        {
            action = new Actions(driver);
            action.MoveToElement(ProductsLink).MoveToElement(CmdmLink).Click().Build().Perform();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[contains(text(), 'Customer Master Data Management')]")));
        }
    }
}
