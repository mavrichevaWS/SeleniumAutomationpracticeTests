using System;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace Automationpractice
{
    class signupOrLoginChecking
    {
        public static void signupOrLoginCheck(string name, string lastname, string test, WebDriverWait wait)
        {
            IWebElement firstLastName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementToBeClickable(By.CssSelector("a.account")));

            // Verifying that the account was created or login successful
            if (firstLastName.Text == (name + " " + lastname))
            {
                Assert.IsTrue(firstLastName.Text == (name + " " + lastname));
                Console.WriteLine(test + "Testing: " + test + " test successful");
            }
            else
            {
                Console.WriteLine(test + "Testing: " + test + " test failed");
            }
        }
    }
}
