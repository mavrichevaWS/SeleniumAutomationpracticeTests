using System;
using System.Threading;
using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automationpractice
{
    public class MainTestClass
    {
        private string _url;
        private IWebDriver _driver;

        public MainTestClass(string url, IWebDriver driver)
        {
            _url = url;
            _driver = driver;
            _driver.Navigate().GoToUrl(_url);
        }

        public void signupTesting(string email, string accountPassword, string name, string lastname, WebDriverWait wait)
        {
            IWebElement signupField = _driver.FindElement(By.Id("email_create"));
            signupField.SendKeys(email);

            IWebElement enterButton = _driver.FindElement(By.CssSelector("button[name='SubmitCreate']"));
            enterButton.Click();

            IWebElement firstName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementToBeClickable(By.CssSelector("input[name='customer_firstname']")));
            firstName.SendKeys(name);

            IWebElement lastName = _driver.FindElement(By.CssSelector("input[name='customer_lastname']"));
            lastName.SendKeys(lastname);

            IWebElement password = _driver.FindElement(By.CssSelector("input[name='passwd']"));
            password.SendKeys(accountPassword);

            IWebElement address = _driver.FindElement(By.CssSelector("input[name='address1']"));
            address.SendKeys("725 NE 166th St Miami, Florida(FL), 33162");

            IWebElement city = _driver.FindElement(By.CssSelector("input[name='city']"));
            city.SendKeys("Miami");

            SelectElement state = new SelectElement(_driver.FindElement(By.CssSelector("select[name='id_state']")));
            state.SelectByText("Florida");

            IWebElement postCode = _driver.FindElement(By.CssSelector("input[name='postcode']"));
            postCode.SendKeys("33162");

            SelectElement country = new SelectElement(_driver.FindElement(By.CssSelector("select[name='id_country']")));
            country.SelectByText("United States");

            IWebElement mobilePhone = _driver.FindElement(By.CssSelector("input[name='phone_mobile']"));
            mobilePhone.SendKeys("+1-305-200-0147");

            IWebElement alias = _driver.FindElement(By.CssSelector("input[name='alias']"));
            alias.SendKeys("Miami, Florida(FL)");

            IWebElement submitAccount = _driver.FindElement(By.CssSelector("button[name='submitAccount']"));
            submitAccount.Click();

            signupOrLoginChecking.signupOrLoginCheck(name, lastname, "signup", wait);

            IWebElement logOut = _driver.FindElement(By.CssSelector("a[title='Log me out']"));
            logOut.Click();
        }

        public void loginTesting(string email, string accountPassword, string name, string lastname, WebDriverWait wait)
        {
            IWebElement loginFieldEmail = _driver.FindElement(By.Id("email"));
            loginFieldEmail.SendKeys(email);

            IWebElement loginFieldPassword = _driver.FindElement(By.Id("passwd"));
            loginFieldPassword.SendKeys(accountPassword);

            IWebElement enterButton = _driver.FindElement(By.CssSelector("button[name='SubmitLogin']"));
            enterButton.Click();

            signupOrLoginChecking.signupOrLoginCheck(name, lastname, "login", wait);
        }

        public void autoCreatedWishlist(string myWishlist, WebDriverWait wait)
        {
            IWebElement myWishlists = _driver.FindElement(By.CssSelector("a[title='My wishlists']"));
            myWishlists.Click();

            Thread.Sleep(3000);

            // Checking that wishlist is already created
            Boolean isExist = _driver.FindElements(By.Id("block-history")).Count > 0;
            if (isExist == false)
            {
                wishlistChecking.wishlistCheck(1, "autoCreatedWishlist:", myWishlist, _driver, wait);
            }
            else
            {
                Console.WriteLine("autoCreatedWishlist: there is more than one product in the wishlist");
            }
            
        }

        public void manuallyCreatedWishlist(string myWishlist, WebDriverWait wait)
        {
            _driver.Navigate().GoToUrl(myWishlist);

            IWebElement deleteFromWishlist = _driver.FindElement(By.CssSelector("tbody > tr:first-child > td:last-child > a"));
            deleteFromWishlist.Click();

            var confirm = _driver.SwitchTo().Alert();
            confirm.Accept();

            IWebElement wishlistName = _driver.FindElement(By.Id("name"));
            wishlistName.SendKeys("Lena's wishlist");

            IWebElement submitWishlist = _driver.FindElement(By.Id("submitWishlist"));
            submitWishlist.Click();

            Thread.Sleep(3000);

            wishlistChecking.wishlistCheck(3, "manuallyCreatedWishlist:", myWishlist, _driver, wait);
        }

        public void addProductsToCart(string myWishlist, WebDriverWait wait)
        {
            int[] numOfProducts = { 1, 3, 5 };
            int[] basketProducts = { 1, 2, 3 };
            int[] products = { 0, 1, 2 };
            int counter = 0;

            float[] prices = new float[0];
            float summ = 0;

            string[] names = new string[0];

            for (int i = 0; i < numOfProducts.Length; i++)
            {
                _driver.Navigate().GoToUrl(myWishlist);

                IList<IWebElement> myProduct = _driver.FindElements(By.ClassName("product-name"));
                myProduct[numOfProducts[i]].Click();

                IWebElement addToChartButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                    ElementToBeClickable(By.Id("add_to_cart")));
                addToChartButton.Click();

                IWebElement nameOfProduct = _driver.FindElement(By.TagName("h1"));
                Array.Resize(ref names, names.Length + 1);
                names[names.GetUpperBound(0)] = nameOfProduct.Text;

                IWebElement price = _driver.FindElement(By.Id("our_price_display"));

                if (price.Text.StartsWith("$"))
                {
                    Array.Resize(ref prices, prices.Length + 1);
                    // If you have english locale, please delete ".Replace('.', ',')"
                    prices[prices.GetUpperBound(0)] = float.Parse(price.Text.Substring(1).Replace('.', ','));
                }

                IWebElement continueShoppingButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                    ElementToBeClickable(By.CssSelector("span.continue")));
                continueShoppingButton.Click();
            }

            IWebElement viewShoppingCart = _driver.FindElement(By.CssSelector("a[title='View my shopping cart']"));
            viewShoppingCart.Click();

            for (int i = 0; i < basketProducts.Length; i++) {
                IList<IWebElement> myProduct = _driver.FindElements(By.CssSelector("p.product-name > a"));
                if (names[i] == myProduct[basketProducts[i]].Text) {
                    Assert.IsTrue(names[i] == myProduct[basketProducts[i]].Text);
                    Console.WriteLine("addProductsToCart: " + names[i] + " in the cart. Product");
                    counter++;
                }
            }

            if (counter == 3)
            {
                Console.WriteLine("all 3 products are in the cart");
            }
            else
            {
                Console.WriteLine("less than 3 products in the cart");
            }

            // Check that products in the detailed page and in the basket are the same, if it is true - count the summ
            IList<IWebElement> priceOfProduct = _driver.FindElements(By.CssSelector("td.cart_total > span.price"));
            for (int i = 0; i < products.Length; i++)
            {
                if (prices[i] == float.Parse(priceOfProduct[products[i]].Text.Substring(1).Replace('.', ',')))
                {
                    Assert.IsTrue(prices[i] == float.Parse(priceOfProduct[products[i]].Text.Substring(1).Replace('.', ',')));
                    summ += prices[i];
                }
                else
                {
                    Console.WriteLine("the price of products are not equal, impossible to calculate the amount");
                }
            }
            Console.WriteLine("summ of the products of cart: " + summ + " in the cart");
        }
    }
}
