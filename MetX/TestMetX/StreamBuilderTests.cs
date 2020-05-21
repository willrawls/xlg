using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MetX.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataService.Tests.Services
{
    [TestClass]
    public class CLASS
    {
        [TestMethod]
        public void TargetStreamShouldNotBeNullWhenSetTest()
        {
            var henry = new Henry();
            henry.TryIt();
        }


        public class Henry
        {
            public Stream LocalStream;

            public void TryIt()
            {
                //var stream1 = new FileStream("garth1.txt", false ? FileMode.Append : FileMode.Create);
                var fileStream = File.Open("garth5.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                
                Assert.IsFalse(fileStream.IsAsync);
                Assert.IsTrue(fileStream.CanWrite);
                LocalStream = fileStream; 
                Assert.IsTrue(fileStream.CanWrite);

                var george = new Henry();
                george.LocalStream = fileStream;
                Assert.IsTrue(fileStream.CanWrite);

                var streamBuilder1 = new StreamBuilder();
                streamBuilder1.TargetStream = george.LocalStream;

                Assert.IsTrue(fileStream.CanWrite);
                Assert.IsNotNull(streamBuilder1.TargetStream);

                while(streamBuilder1.TargetStream != null && fileStream.CanWrite)
                    Thread.Sleep(1000);

                Assert.IsTrue(LocalStream.CanWrite);
                
                var streamBuilder = new StreamBuilder("george2.txt");
                streamBuilder.AppendLine("12");
                streamBuilder.Finish(false);
            }
/*
            public void TryIt2()
            {
                var stream1 = new FileStream("garth1.txt", false ? FileMode.Append : FileMode.Create);
                var stream2 = new FileStream("garth2.txt", false ? FileMode.Append : FileMode.Create);
                var stream3 = new FileStream("garth3.txt", false ? FileMode.Append : FileMode.Create);

                Assert.IsTrue(stream1.CanWrite);
                LocalStream = stream1; // wtaf - fine here
                Assert.IsTrue(stream1.CanWrite);

                var streamBuilder1 = new StreamBuilder();
                var streamBuilder2 = new StreamBuilder();

                Assert.IsTrue(stream1.CanWrite);
                streamBuilder1.TargetStreams.Add(stream1);
                Assert.IsTrue(stream1.CanWrite);
                Assert.IsTrue(streamBuilder1.TargetStream != null);

                Assert.IsTrue(stream2.SafeFileHandle?.IsClosed == false);
                streamBuilder1.TargetStream = stream2;
                Assert.IsTrue(stream2.SafeFileHandle?.IsClosed == false);
                
                
                streamBuilder2.TargetStream = stream3;
                Assert.IsTrue(streamBuilder2.TargetStream != null);

                var streamBuilder = new StreamBuilder("george2.txt");
                streamBuilder.AppendLine("12");
                streamBuilder.Finish(false);
            }
*/
        }
    }
}