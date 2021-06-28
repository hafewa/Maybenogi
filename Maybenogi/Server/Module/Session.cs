using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Maybenogi.Shared.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Maybenogi.Server.Module
{
    public class Session 
    {
        private readonly DriverOptions _options;
        private readonly NexonAccount _account;

        private EBrowserType _browser;

        private RemoteWebDriver _driver;
        private readonly TimeSpan _timeout;

        public const string LOGIN_PAGE = "https://nxlogin.nexon.com/common/login.aspx?redirect=https%3A%2F%2Fwww.nexon.com%2FHome%2FGame";
        public const string MABINOGI_BOARD = "https://mabinogi.nexon.com/page/community/free_list.asp";

        static class Elements
        {
            public static class ClassID
            {
                public const string ID = "txtNexonID";
                public const string PWD = "txtPWD";
            }

            public static class XPATH
            {
                public const string LOGIN = "//*[@id=\"nexonLogin\"]/fieldset/div[4]/button";
            }
        }

        static class JavaScripts
        {
            public const string RUN_MABINOGI = "MabinogiGameStart();";
        }

        private Session(List<string> options, in NexonAccount account, int timeout, EBrowserType browserType)
        {
            switch (browserType)
            {
                case EBrowserType.None:
                    break;
                case EBrowserType.Chrome:
                    this._options = new ChromeOptions();
                    ((ChromeOptions)_options ).AddArguments(options);
                    break;

                case EBrowserType.Firefox:
                    this._options = new FirefoxOptions();
                    ((FirefoxOptions)_options).AddArguments(options);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
            }

            this._account = account;
            this._timeout = TimeSpan.FromSeconds(timeout);
            this._browser = browserType;
        }

        private void CreateWebDriver()
        {
            switch (_browser)
            {
                case EBrowserType.None:
                    break;

                case EBrowserType.Chrome:
                    _driver = new ChromeDriver(_options as ChromeOptions);
                    break;
                case EBrowserType.Firefox:
                    _driver = new FirefoxDriver(_options as FirefoxOptions);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task Delay(int millisecond)
        {
            Console.WriteLine($"Delay : {millisecond}ms");
            await Task.Delay(millisecond);
        }

        public async Task Run()
        {
            Console.WriteLine("Run");
            await Delay(1000);

            Console.WriteLine("CreateWebDriver Begin");
            CreateWebDriver();
            Console.WriteLine("CreateWebDriver End");

            await Delay(1000);

            Console.WriteLine("GoToNexon Begin");
            _driver.Navigate().GoToUrl(LOGIN_PAGE);
            Console.WriteLine("GoToNexon End");

            await Delay(1000);

            Console.WriteLine("Find IWebElements Begin");
            var idField = _driver.FindElementById(Elements.ClassID.ID);
            var pwField = _driver.FindElementById(Elements.ClassID.PWD);

            var loginButton = _driver.FindElementByXPath(Elements.XPATH.LOGIN);
            Console.WriteLine("Find IWebElements End");

            await Delay(1000);

            Console.WriteLine("SendKeys Begin");
            idField.SendKeys(_account.Email);
            await Delay(1000);
            pwField.SendKeys(_account.Password);
            await Delay(1000);

            loginButton.Click();
            await Delay(1000);
            Console.WriteLine("SendKeys End");
            //Wait();

            Console.WriteLine("GoToMabinogi Begin");
            _driver.Navigate().GoToUrl(MABINOGI_BOARD);
            Console.WriteLine("GoToMabinogi End");

            await Delay(1000);

            _driver.ExecuteScript(JavaScripts.RUN_MABINOGI);
        }

        public async Task Dispose()
        {
            await Task.Delay(10);
            _driver.Dispose();
        }

        private void Wait()
        {
            new WebDriverWait(_driver, _timeout).Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(2000);
        }

        public class Builder
        {
            private int _width = 400;
            private int _height = 400;
            private string _user_data = "C:\\users\\public\\";
            private bool _isHeadless = false;
            private int _timeout = 2;
            private EBrowserType _browserType = EBrowserType.Firefox;

            private NexonAccount _account;

            public Builder SetResolution(int width, int height)
            {
                this._width = width;
                this._height = height;

                return this;
            }

            public Builder UseCustomUserDataFolder(string path)
            {
                this._user_data = path;

                return this;
            }

            public Builder SetAccount(NexonAccount account)
            {
                this._account = account;
                return this;
            }

            public Builder SetHeadless(bool isHeadless)
            {
                this._isHeadless = isHeadless;
                
                return this;
            }

            public Builder SetTimeout(int second)
            {
                this._timeout = second;
                return this;
            }

            public Builder SetBrowser(EBrowserType type)
            {
                this._browserType = type;
                return this;
            }

            public Session Build()
            {
                var options = new List<string>();

                switch (_browserType)
                {
                    case EBrowserType.None:
                        break;
                    case EBrowserType.Chrome:
                        options.Add($"--user-data-dir={_user_data}chrome");
                        break;
                    case EBrowserType.Firefox:
                        options.Add($"-profile");
                        options.Add($"{_user_data}firefox");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                options.Add($"--window-size={_width},{_height}");
                
                if (_isHeadless)
                {
                    options.Add("--headless");
                    options.Add("--start-maximized");
                }

                var session = new Session(options, _account, _timeout, _browserType);

                return session;
            }
        }
    }
}