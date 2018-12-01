using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Harvey.TestBase
{
    [TestClass]
    public abstract class UnitTestsBase
    {
        [TestInitialize]
        public void Initialize()
        {
            OnTestInitialize();
        }
        [TestCleanup]
        public void CleanUp()
        {
            OnTestCleanUp();
        }

        public virtual void OnTestInitialize()
        {

        }
        public virtual void OnTestCleanUp()
        {

        }

        public void WaitForSeconds(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }
    }
}
