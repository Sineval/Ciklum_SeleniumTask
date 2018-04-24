using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace CiklumSeleniumTask.PageObjects
{
    class GooglePage
    {
        private IWebDriver driver;

        public GooglePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = ".//*[@id='rso']//div//h3/a")]
        public IList<IWebElement> GLinks { get; set; } // <- For some reason is 'null'...

        public StiboPage GoToStiboPage()
        {
            IList<IWebElement> GLinks = driver.FindElements(By.XPath((".//*[@id='rso']//div//h3/a")));
            GLinks.Where(a => a.GetAttribute("href").Contains("www.stibosystems.com")).First().Click();
            return new StiboPage(driver);
        }
    }
}
