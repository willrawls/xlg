using System.Threading;
using MetX.Standard.Library.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests
{
    [TestClass]
    public class StreamBuilderTests
    {
        [TestMethod]
        public void TargetStreamShouldNotBeNullWhenSetTest()
        {
            Assert.IsTrue(TryIt());
        }

        public bool TryIt()
        {
            var gregory = new StreamBuilder("gregory1.txt");
            if (gregory.TargetStream == null || !gregory.TargetStream.CanWrite)
                return false;

            var count = 0;
            while(++count < 5 && gregory.TargetStream != null && gregory.TargetStream.CanWrite)
                Thread.Sleep(1000);

            if (gregory.TargetStream == null || !gregory.TargetStream.CanWrite)
                return false;
            
            return true;
        }


    }
}