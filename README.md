## **About:**  

- STAF stands for Simple Test Automation Framework.
- STAF.UI.API nuget for Page Object Model implementation of Selenium Framework.

## **What it has**

- STAF is a test automation framework for c# projects using selenium.
- STAF helps to automate API and UI Tests using REST and Selenium.
- STAF has all base class written so you can inherit and use those.
- STAF has a good HTML reporting as well for test result viewing.
- Parallel Execution using native machine threads.
- Excel Comparison and easy usage.
- STAF framework is maintained by https://github.com/sooraj171


## **How to implement**

From your Visual Studio IDE Navigate to file menu:
- Extension -> Manage Extension -> Online Templates -> Search "STAFTemplate".
- Install the template successfully.
-   Create a new project from Visual Studio
- Search for the template "STAFSample" and select it and create project
- From the Build menu, Click Build Project
- From Test Explorer, select the test and Click run.
- A sample test will get executed.

## **How to Override the driver and driver options**

To override BrowserOptions use SetChromeOptions / SetEdgeOptions
    Usage:
        protected override ChromeOptions SetChromeOptions()
        { return ChromeOptions;}

To override BrowserDriver
    Usage:
        public override IWebDriver GetBrowserDriverObject("Chrome", "", false)
        { return WebDriver;}


## **How to Use Excel Options**
    ExcelDriver excel= new ExcelDriver();
            ExcelCompareStatus res = excel.CompareFiles("\Excel1.xlsx", "\Excel2.xlsx",SheetIndexFile1,SheetIndexFile2);
            if (res.IsMatching)
            {
               return true;
            }
            else
            {
                return false;
            }

## **License:**  
     MIT



THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.