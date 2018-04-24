using System;
using NUnit.Framework;
using CiklumSeleniumTask.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using System.Threading;

namespace CiklumSeleniumTask
{
    public class TestClass
    {
        private IWebDriver driver;
        private Actions action;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [Test]
        public void CiklumTask()
        {
            // 1. Open web browser and go to google.com page.
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.google.com");

            // 2.  Search for "stibo systems" string.
            action = new Actions(driver);
            action.SendKeys("stibo systems").SendKeys(Keys.Enter).Build().Perform();

            // 3.  Click the link leading to "www.stibosystems.com".
            var objGoogle = new GooglePage(driver);

            objGoogle.GoToStiboPage();

            // 4.  Click search and type "energy".
            var objStibo = new StiboPage(driver);

            objStibo.SearchForEnergy();

            // 5.  Return in console number of found results.
            Thread.Sleep(TimeSpan.FromSeconds(1)); // Hack for making sure that correct info is taken out from element
            Trace.WriteLine(objStibo.EnergyText.Text);

            // 6.  In search results for "energy" look for "About Us" link and return on which page it was found and click on the link.
            objStibo.SearchForAboutUsLink();

            // 7.  Assert that Facebook icon is present on the page.
            Assert.IsTrue(objStibo.FacebookIcon.Displayed);

            // 8.  Click on "Blog" link.
            objStibo.ClickOnBlogLink();

            // 9.  In "Follow Blog" email input field provide email with incorrect format and return in console error message.
            objStibo.BlogClickOnEmailField();

            action = new Actions(driver);
            action.SendKeys("ThisIsNotAnEmail").SendKeys(Keys.Enter).Build().Perform();

            objStibo.BlogWrongEmailGetErrorMessage();

            // 10.  In "Follow Blog" email input field provide correct email and click send.
            objStibo.BlogClickOnEmailField();

            action = new Actions(driver);
            action.SendKeys("thisIsAnEmail@email.com").Build().Perform();

            objStibo.BlogEmailClickSend();

            // 11.  Assert that "Thanks for Subscribing!" message was returned.
            Assert.IsTrue(objStibo.BlogEmailCorrectMessage.Displayed);

            // 12.   Click on LinkedIn icon. Page will open in new tab/window.
            objStibo.GoToLinkedInPage();
            var objLinkedIn = new LinkedInPage(driver);

            // 13.   Click "Join LinkedIn" (no user should be signed in).
            // objLinkedIn.ClickOnJoin(); <-- Join button is not available in LinkedIn page opened by Selenium. LinkedIn link leads directly to Sign-up form

            // 14.   Assert that signup screen is shown.
            objLinkedIn.WaitForWindowToLoad();
            Assert.IsTrue(objLinkedIn.JoinButton.Displayed);

            // 15.   Close newly opened window/tab and go back to previously opened StiboSystems Blog page.
            objLinkedIn.CloseLinkeinTab();

            // 16.   In "CATEGORIES" click on "Customer Master Data Management (CMDM)" category.
            objStibo.GoToCMDM();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
