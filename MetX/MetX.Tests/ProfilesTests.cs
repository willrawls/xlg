using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using MetX.G13ProfileEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MetX.Tests
{
    [TestClass]
    public class ProfilesTests
    {
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private MemoryTarget _target;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialze()
        {
            _target = new MemoryTarget {Layout = "${message}"};
            SimpleConfigurator.ConfigureForTargetLogging(_target, LogLevel.Debug);
        }

        [TestCleanup]
        public void DumpLogs()
        {
            if (_target?.Logs == null)
                return;

            for (var index = 0; index < _target.Logs.Count; index++)
            {
                var s = _target.Logs[index];
                TestContext.WriteLine($"{index + 1}: {s}");
            }
        }


        [TestMethod]
        public void ToXmlTest()
        {
            var data = new Profiles
            {
                Profile =
                    new Profile(Guid.NewGuid(), @lock: 0, gkeysdk: 0, lastpayeddate: DateTime.Now,
                        uriHostNameType: "Programming", launchable: 1, gpasupported: 0, gameid: "")
            };
            data.Profile.Assignments = new List<Assignments>
            {
                new Assignments
                {
                    devicecategory = "fred",
                    Assignment = new List<Assignment>
                    {
                        new Assignment
                        {
                            backup =true,
                            contextid = "george",
                            original = true,
                            shiftstate = 12,
                        }
                    }
                }
            };

            var macro = new Macro(Guid.NewGuid(), 123, true, "456", true)
            {
                TextBlock = new TextBlock(
                    new Text(true, 234, "fred", "a;slkdjf\n"))
            };

            data.Profile.Macros = new List<Macro>
            {
                macro,
            };
            var actual = data.ToXml();
            Log.Debug(actual);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("<profiles"));
        }
    }

}