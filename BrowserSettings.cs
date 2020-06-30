using System;
using OpenQA.Selenium.Remote;

namespace Automationpractice
{
    static class BrowserSettings
    {
        public static OpenQA.Selenium.ICapabilities webDriverSetting(string name) {
            var sauceUserName =
                Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            var sauceAccessKey =
                Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability("username", sauceUserName);
            capabilities.SetCapability("accessKey", sauceAccessKey);
            capabilities.SetCapability("name", name);

            // 1) Windows 10, Microsoft Edge (latest version)
            /* capabilities.SetCapability("platform", "Windows 10");
            capabilities.SetCapability("browserName", "MicrosoftEdge");
            capabilities.SetCapability("version", "latest"); */

            // 2) Windows 8.1, Mozilla Firefox 39.0
            /* capabilities.SetCapability("platform", "Windows 8.1");
            capabilities.SetCapability("browserName", "Firefox");
            capabilities.SetCapability("version", "39.0"); */

            // 3) Linux, Google Chrome 40
            capabilities.SetCapability("platform", "Linux");
            capabilities.SetCapability("browserName", "Chrome");
            capabilities.SetCapability("version", "40.0");

            return capabilities;
        }
    }
}
