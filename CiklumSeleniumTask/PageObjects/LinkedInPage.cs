using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace CiklumSeleniumTask.PageObjects
{
    class LinkedInPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public LinkedInPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        [FindsBy(How = How.XPath, Using = "/html/body/header/div/nav/ul/li[2]/a")]
        public IWebElement JoinLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='join-form']")]
        public IWebElement JoinButton { get; set; }

        public void ClickOnJoin()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(JoinLink)).Click();
        }

        public void WaitForWindowToLoad()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(JoinButton));
        }

        public void CloseLinkeinTab()
        {
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }
    }
}
