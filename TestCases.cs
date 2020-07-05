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

            IWebElement firstLastName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementToBeClickable(By.CssSelector("a.account")));

            // Verifying that the account was created
            if (firstLastName.Text == (name + " " + lastname))
            {
                Assert.IsTrue(firstLastName.Text == (name + " " + lastname));
                Console.WriteLine("signupTesting: " + "Signup test successful");
            }
            else
            {
                Console.WriteLine("signupTesting: " + "Signup test failed");
            }

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

            IWebElement firstLastName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementToBeClickable(By.CssSelector("a.account")));

            // Verifying correctly account login
            if (firstLastName.Text == (name + " " + lastname))
            {
                Assert.IsTrue(firstLastName.Text == (name + " " + lastname));
                Console.WriteLine("loginTesting: " + "Login test successful");
            }
            else
            {
                Console.WriteLine("loginTesting: " + "Login test failed");
            }
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
                IList<IWebElement> myProduct = _driver.FindElements(By.ClassName("product-name"));
                myProduct[1].Click();

                IWebElement wishlistButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                    ElementToBeClickable(By.CssSelector("a[title='Add to my wishlist']")));
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
                    Console.WriteLine("autoCreatedWishlist: There is correct product in the wishlist");
                }
                else
                {
                    Console.WriteLine("autoCreatedWishlist: There is not correct product in the wishlist");
                }
            }
            else
            {
                Console.WriteLine("autoCreatedWishlist: There is more than one product in the wishlist");
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

            IList<IWebElement> myProduct = _driver.FindElements(By.ClassName("product-name"));
            myProduct[3].Click();

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
                Console.WriteLine("manuallyCreatedWishlist: There is correct product in the wishlist");
            }
            else
            {
                Console.WriteLine("manuallyCreatedWishlist: There is not correct product in the wishlist");
            }

            // Thread.Sleep(5000);
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
                Console.WriteLine("All 3 products are in the cart");
            }
            else
            {
                Console.WriteLine("Less than 3 products in the cart");
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
                    Console.WriteLine("The price of products are not equal, impossible to calculate the amount");
                }
            }
            Console.WriteLine("Summ of the products of cart: " + summ + " in the cart");
        }
    }
}
