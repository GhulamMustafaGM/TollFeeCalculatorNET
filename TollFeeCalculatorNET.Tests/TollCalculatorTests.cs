using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TollFeeCalculatorNET.Tests
{
    [TestClass]
    public class TollCalculatorTests
    {
        [TestMethod]
        public void GetTollFeeForNoFreeVehicle()
        {
            DateTime[] date = { new DateTime(2021, 08, 17, 6, 25, 0), new DateTime(2021, 08, 17, 7, 40, 0), new DateTime(2021, 08, 17, 15, 40, 0), new DateTime(2021, 08, 17, 15, 55, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 53);
        }

        [TestMethod]
        public void GetZeroTollFeeForFreeVehicle()
        {
            DateTime[] date = { new DateTime(2021, 08, 17, 6, 25, 0), new DateTime(2021, 08, 17, 7, 40, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Motorbike(), date);
            Assert.AreEqual(tollFee, 0);
        }

        [TestMethod]
        public void GetMaxTollFeeVehicle()
        {
            DateTime[] date = { new DateTime(2021, 08, 17, 6, 25, 0), new DateTime(2021, 08, 17, 7, 40, 0), new DateTime(2021, 08, 17, 8, 55, 0), new DateTime(2021, 08, 17, 15, 40, 0), new DateTime(2021, 08, 17, 17, 40, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 60);
        }

        [TestMethod]
        public void GetZeroTollFeeForFreeDateTime()
        {
            DateTime[] date = { new DateTime(2021, 08, 17, 19, 25, 0), new DateTime(2021, 08, 17, 20, 40, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 0);
        }

        [TestMethod]
        public void GetZeroTollFeeForFreeDate()
        {
            DateTime[] date = { new DateTime(2021, 11, 1, 19, 25, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 0);
        }

        [TestMethod]
        public void GetZeroTollFeeForHoliday()
        {
            DateTime[] date = { new DateTime(2021, 08, 18, 19, 25, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 0);
        }
    }
}
