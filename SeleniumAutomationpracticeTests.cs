using System;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

using Allure.Commons;
using Allure.NUnit.Attributes;
using Allure.Commons.Model;

namespace Automationpractice
{
    [TestFixture]
    [AllureSuite("Automationpractice tests")]
    public class WebDriverTasks : AllureReport
    {
        // Add functionality for using Chrome browser
        public static IWebDriver driver = new ChromeDriver(@"C:\Users\user\source\repos\Automationpractice\Drivers");

        // Add functionality for using Firefox browser, in order to use it, please comments line above
        // public static IWebDriver driver = new FirefoxDriver(@"C:\Users\user\source\repos\Automationpractice\Drivers");

        // Load driver for using tests with SeleniumGrid and Saucelabs
        // public static RemoteWebDriver driver;

        public WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 20));

        // Initialize a driver
        [SetUp]
        public void Setup()
        {
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);
            Console.WriteLine("set up has started");
            AllureLifecycle.Instance.SetCurrentTestActionInException(() =>
            {
                AllureLifecycle.Instance.AddAttachment("Step Screenshot", AllureLifecycle.AttachFormat.ImagePng,
                driver.TakeScreenshot().AsByteArray);
            });
        }

        // Method for making screenshot for falling tests
        [TearDown]
        public void TeardownTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
            {
                if (TestContext.CurrentContext.Result.Outcome.Label == "Error")
                {
                    Console.WriteLine("Test is in Error");
                    AllureLifecycle.Instance.AddAttachment("Step Screenshot", AllureLifecycle.AttachFormat.ImagePng,
                    driver.TakeScreenshot().AsByteArray);
                }
                else
                {
                    Console.WriteLine("Test Failed");
                    AllureLifecycle.Instance.AddAttachment("Step Screenshot", AllureLifecycle.AttachFormat.ImagePng,
                    driver.TakeScreenshot().AsByteArray);
                }
            }
            driver.Quit();
        }

        [TestCase(TestName = "All automation practice tests")]
        [AllureSubSuite("All automation practice")]
        [AllureSeverity(SeverityLevel.Critical)]
        [AllureLink("Some link")]
        [AllureTest("All automation practice test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("11111")]
        [AllureTms("01234")]
        public void AllAutomationpracticeTests()
        {
            // Using options for run tests with Selenium Grid
            /* ChromeOptions Options = new ChromeOptions();
            Options.PlatformName = "windows";
            Options.AddAdditionalCapability("platform", "WIN8_1", true);
            Options.AddAdditionalCapability("version", "latest", true); */

            // We configure launch remotely with SeleniumGrid in a current machne using node 
            /* driver = new RemoteWebDriver(new Uri("http://192.168.100.4:5555/wd/hub/"), Options.ToCapabilities(),
                TimeSpan.FromSeconds(600)); // NOTE: connection timeout of 600 seconds or more required for time to launch grid nodes if non are available
            */

            // Launch tests on Sauselabs
            /* driver = new RemoteWebDriver(new Uri("https://lenamavricheva:35f90510-0c41-419a-925a-811973864966@ondemand.eu-central-1.saucelabs.com:443/wd/hub"),
                 BrowserSettings.webDriverSetting(TestContext.CurrentContext.Test.Name), TimeSpan.FromSeconds(600)); */

            MainTestClass maintestclass = new MainTestClass(UserConstantData.signupLink, driver);

            string email = DateTime.Now.ToString("h_mm_ss") + UserConstantData.email;

            AllureLifecycle.Instance.RunStep("Signup test", () => {
                maintestclass.signupTesting(email, UserConstantData.accountPassword,
                    UserConstantData.name, UserConstantData.lastname, wait);
            });

            AllureLifecycle.Instance.RunStep("Login test", () => {
                maintestclass.loginTesting(email, UserConstantData.accountPassword,
                    UserConstantData.name, UserConstantData.lastname, wait);
            });

            AllureLifecycle.Instance.RunStep("Auto-created wishlist test", () =>
            {
                maintestclass.autoCreatedWishlist(UserConstantData.myWishlist, wait);
            });

            AllureLifecycle.Instance.RunStep("Manually created wishlist test", () =>
            {
                maintestclass.manuallyCreatedWishlist(UserConstantData.myWishlist, wait);
            });

            AllureLifecycle.Instance.RunStep("Add products to cart test", () =>
            {
                maintestclass.addProductsToCart(UserConstantData.myWishlist, wait);
            });
        }
    }
}
