﻿using System.Threading;
using MetX.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests
{
    [TestClass]
    public class StreamBuilderTests
    {
        [TestMethod]
        public void TargetStreamShouldNotBeNullWhenSetTest()
        {
            var henry = new StreamBuilder();
            Assert.IsTrue(TryIt());
        }

        public bool TryIt()
        {
            var gregory = new StreamBuilder("gregory1.txt");
            if (gregory.TargetStream == null || !gregory.TargetStream.CanWrite)
                return false;

            int count = 0;
            while(++count < 5 && gregory.TargetStream != null && gregory.TargetStream.CanWrite)
                Thread.Sleep(1000);

            if (gregory.TargetStream == null || !gregory.TargetStream.CanWrite)
                return false;
            
            return true;
        }


    }
}