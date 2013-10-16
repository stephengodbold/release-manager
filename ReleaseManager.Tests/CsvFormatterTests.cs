using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseManager.Common;
using ReleaseManager.Models;
using Shouldly;

namespace ReleaseManager.Tests
{
    [TestClass]
    public class CsvFormatterTests
    {
        [TestMethod]
        public void Only_Writes_Release_Notes()
        {
            var formatter = new CsvFormatter();

            formatter.CanWriteType(typeof(ReleaseNotes)).ShouldBe(true);
            formatter.CanWriteType(typeof(WorkItem)).ShouldBe(false);
            formatter.CanWriteType(typeof(Project)).ShouldBe(false);
            formatter.CanWriteType(typeof(Build)).ShouldBe(false);
            formatter.CanWriteType(typeof(Models.Environment)).ShouldBe(false);
        }

        [TestMethod]
        public void Does_Not_Read_Types()
        {
            var formatter = new CsvFormatter();

            formatter.CanReadType(typeof(ReleaseNotes)).ShouldBe(false);
            formatter.CanReadType(typeof(WorkItem)).ShouldBe(false);
            formatter.CanReadType(typeof(Project)).ShouldBe(false);
            formatter.CanReadType(typeof(Build)).ShouldBe(false);
            formatter.CanReadType(typeof(Models.Environment)).ShouldBe(false);
        }
    }
}
