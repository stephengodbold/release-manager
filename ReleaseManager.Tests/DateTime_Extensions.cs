using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseManager.Common;
using Shouldly;

namespace ReleaseManager.Tests
{
    [TestClass]
    public class DateTime_Extensions
    {
        [TestClass]
        public class Last_Second_Of_Day
        {
            [TestMethod]
            public void Returns_Last_Second_For_Date()
            {
                var currentDate = new DateTime(2012, 11, 26, 12, 13, 14);
                var expectedDate = new DateTime(2012, 11, 26, 23, 59, 59);
                var lastTick = currentDate.LastSecondOfDay();

                lastTick.ShouldBe(expectedDate);
            }

            [TestMethod]
            public void Returns_Last_Second_On_Feb29()
            {
                var currentDate = new DateTime(2012, 04, 29, 23, 13, 14);
                var expectedDate = new DateTime(2012, 04, 29, 23, 59, 59);
                var lastTick = currentDate.LastSecondOfDay();

                lastTick.ShouldBe(expectedDate);
            }

            [TestMethod]
            public void Doesnt_Jump_To_Next_Day_On_Last_Second_Of_Day_Input()
            {
                var currentDate = new DateTime(2012, 04, 20, 23, 59, 59);
                var expectedDate = new DateTime(2012, 04, 20, 23, 59, 59);
                var lastTick = currentDate.LastSecondOfDay();

                lastTick.ShouldBe(expectedDate);
            }

            [TestMethod]
            public void Returns_Last_Second_In_Correct_Timezone()
            {
                var currentDate = new DateTime(2012, 04, 20, 23, 00, 59, DateTimeKind.Utc);
                var expectedDate = new DateTime(2012, 04, 20, 23, 59, 59, DateTimeKind.Utc);
                var lastTick = currentDate.LastSecondOfDay();

                lastTick.ShouldBe(expectedDate);
            }
        }

        [TestClass]
        public class First_Second_Of_Day
        {
            [TestMethod]
            public void Returns_First_Second_For_Date()
            {
                var currentDate = new DateTime(2012, 11, 26, 12, 13, 14);
                var expectedDate = new DateTime(2012, 11, 26, 00, 00, 00);
                var lastTick = currentDate.FirstSecondOfDay();

                lastTick.ShouldBe(expectedDate);
            }

            [TestMethod]
            public void Doesnt_Jump_To_Previous_Day_On_First_Second_Of_Day_Input()
            {
                var currentDate = new DateTime(2012, 04, 20, 00, 00, 00);
                var expectedDate = new DateTime(2012, 04, 20, 00, 00, 00);
                var lastTick = currentDate.FirstSecondOfDay();

                lastTick.ShouldBe(expectedDate);
            }
        }
    }
}
