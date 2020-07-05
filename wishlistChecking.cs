using System;
using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automationpractice
{
    class wishlistChecking
    {
        public static void wishlistCheck(int test, string name, string myWishlist, IWebDriver _driver, WebDriverWait wait)
        {
            IList<IWebElement> myProduct = _driver.FindElements(By.ClassName("product-name"));
            myProduct[test].Click();

            IWebElement wishlistButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementToBeClickable(By.Id("wishlist_button")));
            wishlistButton.Click();

            IWebElement nameOfProduct = _driver.FindElement(By.TagName("h1"));
            string productName = nameOfProduct.Text;

            IWebElement popUpText = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementToBeClickable(By.ClassName("fancybox-error")));

            Assert.IsTrue(popUpText.Text == "Added to your wishlist.");

            _driver.Navigate().GoToUrl(myWishlist);

            IWebElement productInWishlist = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                    ElementToBeClickable(By.CssSelector("tbody > tr:first-child > td:nth-of-type(5) > a")));
            productInWishlist.Click();

            IWebElement nameOfProductInWishlist = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                        ElementToBeClickable(By.CssSelector("a.lnkdel + p.product-name")));

            if (nameOfProductInWishlist.Text.Contains(productName))
            {
                Assert.IsTrue(nameOfProductInWishlist.Text.Contains(productName));
                Console.WriteLine(name + " there is correct product in the wishlist");
            }
            else
            {
                Console.WriteLine(name + " there is not correct product in the wishlist");
            }
        }
    }
}
