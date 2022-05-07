using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PudelkoUnitTests
{

    [TestClass]
    public static class InitializeCulture
    {
        [AssemblyInitialize]
        public static void SetEnglishCultureOnAllUnitTest(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }

    // ========================================

    [TestClass]
    public class UnitTestsPudelkoConstructors
    {
        private static readonly double defaultSize = 0.1; // w metrach
        private static readonly double accuracy = 0.001; //dokładność 3 miejsca po przecinku

        private static void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
        {
            Assert.AreEqual(expectedA, p.A, delta: accuracy);
            Assert.AreEqual(expectedB, p.B, delta: accuracy);
            Assert.AreEqual(expectedC, p.C, delta: accuracy);
        }
        private static void AssertObjetosc(Pudelko p, double expectedObjetosc)
        {
            Assert.AreEqual(expectedObjetosc, p.Objetosc, delta: accuracy);
        }
        private static void AssertPole(Pudelko p, double expectedPole)
        {
            Assert.AreEqual(expectedPole, p.Pole, delta: accuracy);
        }

        #region Constructor tests ================================

        [TestMethod, TestCategory("Constructors")]
        public void Constructor_Default()
        {
            Pudelko p = new();

            Assert.AreEqual(defaultSize, p.A, delta: accuracy);
            Assert.AreEqual(defaultSize, p.B, delta: accuracy);
            Assert.AreEqual(defaultSize, p.C, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_DefaultMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new(a, b, c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_InMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new(a, b, c, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100.0, 25.5, 3.1,
                 1.0, 0.255, 0.031)]
        [DataRow(100.0, 25.58, 3.13,
                 1.0, 0.255, 0.031)] // dla centymertów liczy się tylko 1 miejsce po przecinku
        public void Constructor_3params_InCentimeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new(a: a, b: b, c: c, unit: UnitOfMeasure.centimeter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100, 255, 3,
                 0.1, 0.255, 0.003)]
        [DataRow(100.0, 25.58, 3.13,
                 0.1, 0.025, 0.003)] // dla milimetrów nie liczą się miejsca po przecinku
        public void Constructor_3params_InMilimeters(double a, double b, double c,
                                                     double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new(unit: UnitOfMeasure.milimeter, a: a, b: b, c: c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        // ----

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new(a, b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new(a: a, b: b, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 2.5, 0.11, 0.025)]
        [DataRow(100.1, 2.599, 1.001, 0.025)]
        [DataRow(2.0019, 0.25999, 0.02, 0.002)]
        public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new(unit: UnitOfMeasure.centimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 2.0, 0.011, 0.002)]
        [DataRow(100.1, 2599, 0.1, 2.599)]
        [DataRow(200.19, 2.5999, 0.2, 0.002)]
        public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new(unit: UnitOfMeasure.milimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        // -------

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_DefaultMeters(double a)
        {
            Pudelko p = new(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_InMeters(double a)
        {
            Pudelko p = new(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 0.11)]
        [DataRow(100.1, 1.001)]
        [DataRow(2.0019, 0.02)]
        public void Constructor_1param_InCentimeters(double a, double expectedA)
        {
            Pudelko p = new(unit: UnitOfMeasure.centimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 0.011)]
        [DataRow(100.1, 0.1)]
        [DataRow(200.19, 0.2)]
        public void Constructor_1param_InMilimeters(double a, double expectedA)
        {
            Pudelko p = new(unit: UnitOfMeasure.milimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        // ---

        public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5, 3.1},
            new object[] {1.0, -2.5, 3.1},
            new object[] {1.0, 2.5, -3.1},
            new object[] {-1.0, -2.5, 3.1},
            new object[] {-1.0, 2.5, -3.1},
            new object[] {1.0, -2.5, -3.1},
            new object[] {-1.0, -2.5, -3.1},
            new object[] {0, 2.5, 3.1},
            new object[] {1.0, 0, 3.1},
            new object[] {1.0, 2.5, 0},
            new object[] {1.0, 0, 0},
            new object[] {0, 2.5, 0},
            new object[] {0, 0, 3.1},
            new object[] {0, 0, 0},
            new object[] {10.1, 2.5, 3.1},
            new object[] {10, 10.1, 3.1},
            new object[] {10, 10, 10.1},
            new object[] {10.1, 10.1, 3.1},
            new object[] {10.1, 10, 10.1},
            new object[] {10, 10.1, 10.1},
            new object[] {10.1, 10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new(a, b, c);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new(a, b, c, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.01, 0.1, 1)]
        [DataRow(0.1, 0.01, 1)]
        [DataRow(0.1, 0.1, 0.01)]
        [DataRow(1001, 1, 1)]
        [DataRow(1, 1001, 1)]
        [DataRow(1, 1, 1001)]
        [DataRow(1001, 1, 1001)]
        [DataRow(1, 1001, 1001)]
        [DataRow(1001, 1001, 1)]
        [DataRow(1001, 1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new(a, b, c, unit: UnitOfMeasure.centimeter);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.1, 1, 1)]
        [DataRow(1, 0.1, 1)]
        [DataRow(1, 1, 0.1)]
        [DataRow(10001, 1, 1)]
        [DataRow(1, 10001, 1)]
        [DataRow(1, 1, 10001)]
        [DataRow(10001, 10001, 1)]
        [DataRow(10001, 1, 10001)]
        [DataRow(1, 10001, 10001)]
        [DataRow(10001, 10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new(a, b, c, unit: UnitOfMeasure.milimeter);
        }


        public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5},
            new object[] {1.0, -2.5},
            new object[] {-1.0, -2.5},
            new object[] {0, 2.5},
            new object[] {1.0, 0},
            new object[] {0, 0},
            new object[] {10.1, 10},
            new object[] {10, 10.1},
            new object[] {10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new(a, b);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new(a, b, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.01, 1)]
        [DataRow(1, 0.01)]
        [DataRow(0.01, 0.01)]
        [DataRow(1001, 1)]
        [DataRow(1, 1001)]
        [DataRow(1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new(a, b, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.1, 1)]
        [DataRow(1, 0.1)]
        [DataRow(0.1, 0.1)]
        [DataRow(10001, 1)]
        [DataRow(1, 10001)]
        [DataRow(10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new(a, b, unit: UnitOfMeasure.milimeter);
        }




        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new(a);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new(a, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(0.01)]
        [DataRow(1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new(a, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(0.1)]
        [DataRow(10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new(a, unit: UnitOfMeasure.milimeter);
        }

        #endregion


        #region ToString tests ===================================

        [TestMethod, TestCategory("String representation")]
        public void ToString_Default_Culture_EN()
        {
            var p = new Pudelko(2.5, 9.321);
            string expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

            Assert.AreEqual(expectedStringEN, p.ToString());
        }

        [DataTestMethod, TestCategory("String representation")]
        [DataRow(null, 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
        [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
        public void ToString_Formattable_Culture_EN(string format, double a, double b, double c, string expectedStringRepresentation)
        {
            var p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
            Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
        }

        [TestMethod, TestCategory("String representation")]
        [ExpectedException(typeof(FormatException))]
        public void ToString_Formattable_WrongFormat_FormatException()
        {
            var p = new Pudelko(1);
            var stringformatedrepreentation = p.ToString("wrong code");
        }

        #endregion


        #region Pole, Objętość ===================================

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1, 0.01)]
        [DataRow(10, 0.1)]
        [DataRow(0.001, 0.00001)]
        public void Objetosc_1param_DefaultMeters(double a, double expectedObjetosc)
        {
            Pudelko p = new(a);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1, 0.01)]
        [DataRow(10, 0.1)]
        [DataRow(0.001, 0.00001)]
        public void Objetosc_1param_Meters(double a, double expectedObjetosc)
        {
            Pudelko p = new(a, UnitOfMeasure.meter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(100, 0.01)]
        [DataRow(1000, 0.1)]
        [DataRow(0.1, 0.00001)]
        public void Objetosc_1param_Centimeters(double a, double expectedObjetosc)
        {
            Pudelko p = new(a, UnitOfMeasure.centimeter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1000, 0.01)]
        [DataRow(10000, 0.1)]
        [DataRow(1, 0.00001)]
        public void Objetosc_1param_Milimeters(double a, double expectedObjetosc)
        {
            Pudelko p = new(a, UnitOfMeasure.milimeter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1, 1, 0.1)]
        [DataRow(10, 10, 10)]
        [DataRow(0.001, 0.001, 0.0000001)]
        [DataRow(2, 2, 0.4)]
        public void Objetosc_2params_DefaultMeters(double a, double b, double expectedObjetosc)
        {
            Pudelko p = new(a, b);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1, 1, 0.1)]
        [DataRow(10, 10, 10)]
        [DataRow(0.001, 0.001, 0.0000001)]
        [DataRow(2, 2, 0.4)]
        public void Objetosc_2params_Meters(double a, double b, double expectedObjetosc)
        {
            Pudelko p = new(a, b, UnitOfMeasure.meter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(100, 100, 0.1)]
        [DataRow(1000, 1000, 10)]
        [DataRow(0.1, 0.1, 0.0000001)]
        [DataRow(200, 200, 0.4)]
        public void Objetosc_2params_Centimeters(double a, double b, double expectedObjetosc)
        {
            Pudelko p = new(a, b, UnitOfMeasure.centimeter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1000, 1000, 0.1)]
        [DataRow(10000, 10000, 10)]
        [DataRow(1, 1, 0.0000001)]
        [DataRow(2000, 2000, 0.4)]
        public void Objetosc_2params_Milimeters(double a, double b, double expectedObjetosc)
        {
            Pudelko p = new(a, b, UnitOfMeasure.milimeter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1, 1, 1, 1)]
        [DataRow(10, 10, 10, 1000)]
        [DataRow(0.001, 0.001, 0.001, 0.000000001)]
        [DataRow(2, 2, 2, 8)]
        public void Objetosc_3params_DefaultMeters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new(a, b, c);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1, 1, 1, 1)]
        [DataRow(10, 10, 10, 1000)]
        [DataRow(0.001, 0.001, 0.001, 0.000000001)]
        [DataRow(2, 2, 2, 8)]
        public void Objetosc_3params_Meters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new(a, b, c, UnitOfMeasure.meter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(100, 100, 100, 1)]
        [DataRow(1000, 1000, 1000, 1000)]
        [DataRow(0.1, 0.1, 0.1, 0.000000001)]
        [DataRow(200, 200, 200, 8)]
        public void Objetosc_3params_Centimeters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new(a, b, c, UnitOfMeasure.centimeter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(1000, 1000, 1000, 1)]
        [DataRow(10000, 10000, 10000, 1000)]
        [DataRow(1, 1, 1, 0.000000001)]
        [DataRow(2000, 2000, 2000, 8)]
        public void Objetosc_3params_Milimeters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new(a, b, c, UnitOfMeasure.milimeter);
            AssertObjetosc(p, expectedObjetosc);
        }

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(1, 1, 1, 6)]
        [DataRow(1, 2, 3, 22)]
        [DataRow(10, 10, 10, 600)]
        [DataRow(0.001, 0.001, 0.001, 0.000006)]
        [DataRow(0.001, 0.002, 0.003, 0.000022)]
        public void Pole_Meters(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new(a, b, c, UnitOfMeasure.meter);
            AssertPole(p, expectedPole);
        }

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(100, 100, 100, 6)]
        [DataRow(100, 200, 300, 22)]
        [DataRow(1000, 1000, 1000, 600)]
        [DataRow(0.1, 0.1, 0.1, 0.000006)]
        [DataRow(0.1, 0.2, 0.3, 0.000022)]
        public void Pole_Centimeters(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new(a, b, c, UnitOfMeasure.centimeter);
            AssertPole(p, expectedPole);
        }

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(1000, 1000, 1000, 6)]
        [DataRow(1000, 2000, 3000, 22)]
        [DataRow(10000, 10000, 10000, 600)]
        [DataRow(1, 1, 1, 0.000006)]
        [DataRow(1, 2, 3, 0.000022)]
        public void Pole_Milimeters(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new(a, b, c, UnitOfMeasure.milimeter);
            AssertPole(p, expectedPole);
        }

        #endregion

        #region Equals ===========================================
        
        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, 1, 1, 1, UnitOfMeasure.meter)]
        [DataRow(1, 2, 1, 2, 1, 1, UnitOfMeasure.meter)]
        [DataRow(1, 2, 3, 3, 2, 1, UnitOfMeasure.meter)]
        [DataRow(10, 5, 10, 5, 10, 10, UnitOfMeasure.meter)]
        [DataRow(0.1, 0.1, 0.1, 0.1, 0.1, 0.1, UnitOfMeasure.meter)]
        [DataRow(0.2, 0.3, 0.4, 0.3, 0.4, 0.2, UnitOfMeasure.meter)]
        [DataRow(0.002, 0.003, 0.004, 0.003, 0.004, 0.002, UnitOfMeasure.meter)]
        [DataRow(0.001, 0.001, 0.001, 0.001, 0.001, 0.001, UnitOfMeasure.meter)]
        public void Equals_SameUnits(double a1, double b1, double c1, double a2, double b2, double c2, UnitOfMeasure unit)
        {
            Pudelko p1 = new(a1, b1, c1, unit);
            Pudelko p2 = new(a2, b2, c2, unit);
            Assert.AreEqual(p1, p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 100, 100, 100, UnitOfMeasure.centimeter)]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 1000, 1000, 1000, UnitOfMeasure.milimeter)]
        [DataRow(100, 100, 100, UnitOfMeasure.centimeter, 1000, 1000, 1000, UnitOfMeasure.milimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 1000, 1000, 1000, UnitOfMeasure.centimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 10000, 10000, 10000, UnitOfMeasure.milimeter)]
        [DataRow(1000, 1000, 1000, UnitOfMeasure.centimeter, 10000, 10000, 10000, UnitOfMeasure.milimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 0.1, 0.1, 0.1, UnitOfMeasure.centimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 1, 1, 1, UnitOfMeasure.milimeter)]
        [DataRow(0.1, 0.1, 0.1, UnitOfMeasure.centimeter, 1, 1, 1, UnitOfMeasure.milimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 300, 400, 200, UnitOfMeasure.centimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 3000, 4000, 2000, UnitOfMeasure.milimeter)]
        [DataRow(200, 300, 400, UnitOfMeasure.centimeter, 3000, 4000, 2000, UnitOfMeasure.milimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 0.3, 0.4, 0.2, UnitOfMeasure.centimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 3, 4, 2, UnitOfMeasure.milimeter)]
        [DataRow(0.2, 0.3, 0.4, UnitOfMeasure.centimeter, 3, 4, 2, UnitOfMeasure.milimeter)]
        public void Equals_DifferentUnits(double a1, double b1, double c1, UnitOfMeasure unit1, double a2, double b2, double c2, UnitOfMeasure unit2)
        {
            Pudelko p1 = new(a1, b1, c1, unit1);
            Pudelko p2 = new(a2, b2, c2, unit2);
            Assert.AreEqual(p1, p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, 1, 1, 2, UnitOfMeasure.meter)]
        [DataRow(1, 2, 1, 2, 1, 2, UnitOfMeasure.meter)]
        [DataRow(1, 2, 3, 3, 2, 2, UnitOfMeasure.meter)]
        [DataRow(10, 5, 10, 5, 10, 9, UnitOfMeasure.meter)]
        [DataRow(0.1, 0.1, 0.1, 0.1, 0.1, 0.2, UnitOfMeasure.meter)]
        [DataRow(0.2, 0.3, 0.4, 0.3, 0.4, 0.3, UnitOfMeasure.meter)]
        [DataRow(0.002, 0.003, 0.004, 0.003, 0.004, 0.042, UnitOfMeasure.meter)]
        [DataRow(0.001, 0.001, 0.001, 0.001, 0.001, 0.601, UnitOfMeasure.meter)]
        public void NotEquals_SameUnits(double a1, double b1, double c1, double a2, double b2, double c2, UnitOfMeasure unit)
        {
            Pudelko p1 = new(a1, b1, c1, unit);
            Pudelko p2 = new(a2, b2, c2, unit);
            Assert.AreNotEqual(p1, p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 100, 100, 120, UnitOfMeasure.centimeter)]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 1000, 1000, 3000, UnitOfMeasure.milimeter)]
        [DataRow(100, 100, 100, UnitOfMeasure.centimeter, 1000, 4000, 1000, UnitOfMeasure.milimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 1000, 1000, 500, UnitOfMeasure.centimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 10000, 1, 10000, UnitOfMeasure.milimeter)]
        [DataRow(1000, 1000, 1000, UnitOfMeasure.centimeter, 10000, 2, 10000, UnitOfMeasure.milimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 0.1, 0.2, 0.1, UnitOfMeasure.centimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 1, 10, 1, UnitOfMeasure.milimeter)]
        [DataRow(0.1, 0.1, 0.1, UnitOfMeasure.centimeter, 1, 100, 1, UnitOfMeasure.milimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 300, 400, 222, UnitOfMeasure.centimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 3000, 4000, 4000, UnitOfMeasure.milimeter)]
        [DataRow(200, 300, 400, UnitOfMeasure.centimeter, 3000, 4000, 4000, UnitOfMeasure.milimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 0.3, 0.4, 0.4, UnitOfMeasure.centimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 3, 4, 4, UnitOfMeasure.milimeter)]
        [DataRow(0.2, 0.3, 0.4, UnitOfMeasure.centimeter, 3, 4, 4, UnitOfMeasure.milimeter)]
        public void NotEquals_DifferentUnits(double a1, double b1, double c1, UnitOfMeasure unit1, double a2, double b2, double c2, UnitOfMeasure unit2)
        {
            Pudelko p1 = new(a1, b1, c1, unit1);
            Pudelko p2 = new(a2, b2, c2, unit2);
            Assert.AreNotEqual(p1, p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, 1, 1, 1, UnitOfMeasure.meter)]
        [DataRow(1, 2, 1, 2, 1, 1, UnitOfMeasure.meter)]
        [DataRow(1, 2, 3, 3, 2, 1, UnitOfMeasure.meter)]
        [DataRow(10, 5, 10, 5, 10, 10, UnitOfMeasure.meter)]
        [DataRow(0.1, 0.1, 0.1, 0.1, 0.1, 0.1, UnitOfMeasure.meter)]
        [DataRow(0.2, 0.3, 0.4, 0.3, 0.4, 0.2, UnitOfMeasure.meter)]
        [DataRow(0.002, 0.003, 0.004, 0.003, 0.004, 0.002, UnitOfMeasure.meter)]
        [DataRow(0.001, 0.001, 0.001, 0.001, 0.001, 0.001, UnitOfMeasure.meter)]
        public void Equals_Operator_SameUnits(double a1, double b1, double c1, double a2, double b2, double c2, UnitOfMeasure unit)
        {
            Pudelko p1 = new(a1, b1, c1, unit);
            Pudelko p2 = new(a2, b2, c2, unit);
            Assert.IsTrue(p1 == p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 100, 100, 100, UnitOfMeasure.centimeter)]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 1000, 1000, 1000, UnitOfMeasure.milimeter)]
        [DataRow(100, 100, 100, UnitOfMeasure.centimeter, 1000, 1000, 1000, UnitOfMeasure.milimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 1000, 1000, 1000, UnitOfMeasure.centimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 10000, 10000, 10000, UnitOfMeasure.milimeter)]
        [DataRow(1000, 1000, 1000, UnitOfMeasure.centimeter, 10000, 10000, 10000, UnitOfMeasure.milimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 0.1, 0.1, 0.1, UnitOfMeasure.centimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 1, 1, 1, UnitOfMeasure.milimeter)]
        [DataRow(0.1, 0.1, 0.1, UnitOfMeasure.centimeter, 1, 1, 1, UnitOfMeasure.milimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 300, 400, 200, UnitOfMeasure.centimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 3000, 4000, 2000, UnitOfMeasure.milimeter)]
        [DataRow(200, 300, 400, UnitOfMeasure.centimeter, 3000, 4000, 2000, UnitOfMeasure.milimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 0.3, 0.4, 0.2, UnitOfMeasure.centimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 3, 4, 2, UnitOfMeasure.milimeter)]
        [DataRow(0.2, 0.3, 0.4, UnitOfMeasure.centimeter, 3, 4, 2, UnitOfMeasure.milimeter)]
        public void Equals_Operator_DifferentUnits(double a1, double b1, double c1, UnitOfMeasure unit1, double a2, double b2, double c2, UnitOfMeasure unit2)
        {
            Pudelko p1 = new(a1, b1, c1, unit1);
            Pudelko p2 = new(a2, b2, c2, unit2);
            Assert.IsTrue(p1 == p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, 1, 1, 2, UnitOfMeasure.meter)]
        [DataRow(1, 2, 1, 2, 1, 2, UnitOfMeasure.meter)]
        [DataRow(1, 2, 3, 3, 2, 2, UnitOfMeasure.meter)]
        [DataRow(10, 5, 10, 5, 10, 9, UnitOfMeasure.meter)]
        [DataRow(0.1, 0.1, 0.1, 0.1, 0.1, 0.2, UnitOfMeasure.meter)]
        [DataRow(0.2, 0.3, 0.4, 0.3, 0.4, 0.3, UnitOfMeasure.meter)]
        [DataRow(0.002, 0.003, 0.004, 0.003, 0.004, 0.042, UnitOfMeasure.meter)]
        [DataRow(0.001, 0.001, 0.001, 0.001, 0.001, 0.601, UnitOfMeasure.meter)]
        public void NotEquals_Operator_SameUnits(double a1, double b1, double c1, double a2, double b2, double c2, UnitOfMeasure unit)
        {
            Pudelko p1 = new(a1, b1, c1, unit);
            Pudelko p2 = new(a2, b2, c2, unit);
            Assert.IsTrue(p1 != p2);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 100, 100, 120, UnitOfMeasure.centimeter)]
        [DataRow(1, 1, 1, UnitOfMeasure.meter, 1000, 1000, 3000, UnitOfMeasure.milimeter)]
        [DataRow(100, 100, 100, UnitOfMeasure.centimeter, 1000, 4000, 1000, UnitOfMeasure.milimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 1000, 1000, 500, UnitOfMeasure.centimeter)]
        [DataRow(10, 10, 10, UnitOfMeasure.meter, 10000, 1, 10000, UnitOfMeasure.milimeter)]
        [DataRow(1000, 1000, 1000, UnitOfMeasure.centimeter, 10000, 2, 10000, UnitOfMeasure.milimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 0.1, 0.2, 0.1, UnitOfMeasure.centimeter)]
        [DataRow(0.001, 0.001, 0.001, UnitOfMeasure.meter, 1, 10, 1, UnitOfMeasure.milimeter)]
        [DataRow(0.1, 0.1, 0.1, UnitOfMeasure.centimeter, 1, 100, 1, UnitOfMeasure.milimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 300, 400, 222, UnitOfMeasure.centimeter)]
        [DataRow(2, 3, 4, UnitOfMeasure.meter, 3000, 4000, 4000, UnitOfMeasure.milimeter)]
        [DataRow(200, 300, 400, UnitOfMeasure.centimeter, 3000, 4000, 4000, UnitOfMeasure.milimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 0.3, 0.4, 0.4, UnitOfMeasure.centimeter)]
        [DataRow(0.002, 0.003, 0.004, UnitOfMeasure.meter, 3, 4, 4, UnitOfMeasure.milimeter)]
        [DataRow(0.2, 0.3, 0.4, UnitOfMeasure.centimeter, 3, 4, 4, UnitOfMeasure.milimeter)]
        public void NotEquals_Operator_DifferentUnits(double a1, double b1, double c1, UnitOfMeasure unit1, double a2, double b2, double c2, UnitOfMeasure unit2)
        {
            Pudelko p1 = new(a1, b1, c1, unit1);
            Pudelko p2 = new(a2, b2, c2, unit2);
            Assert.IsTrue(p1 != p2);
        }

        #endregion

        #region Operators overloading ===========================

        [DataTestMethod, TestCategory("Adding")]
        [DataRow(1, 1, 1, 1, 1, 1, 2)]
        [DataRow(1, 2, 1, 1, 2, 1, 4)]
        [DataRow(3, 3, 3, 1, 1, 1, 36)]
        [DataRow(5, 5, 1, 4, 4, 1, 45)]
        [DataRow(5, 5, 1, 5, 1, 1, 30)]
        [DataRow(0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.000000002)]
        [DataRow(0.001, 0.002, 0.001, 0.001, 0.002, 0.001, 0.000000004)]
        [DataRow(0.003, 0.003, 0.003, 0.001, 0.001, 0.001, 0.000000036)]
        [DataRow(0.005, 0.005, 0.001, 0.004, 0.004, 0.001, 0.000000045)]
        [DataRow(0.005, 0.005, 0.001, 0.005, 0.001, 0.001, 0.000000030)]
        public void Adding_Operator(double a1, double b1, double c1, double a2, double b2, double c2, double expectedObjetosc)
        {
            Pudelko p1 = new(a1, b1, c1);
            Pudelko p2 = new(a2, b2, c2);
            Assert.IsTrue((p1 + p2).Objetosc == expectedObjetosc, (p1+p2).Objetosc.ToString());
        }

        #endregion

        #region Conversions =====================================
        [TestMethod]
        public void ExplicitConversion_ToDoubleArray_AsMeters()
        {
            Pudelko pudelko = new(1, 2.1, 3.231);
            double[] p = (double[])pudelko;
            double[] tab = p;
            Assert.AreEqual(3, tab.Length);
            Assert.AreEqual(pudelko.A, tab[0]);
            Assert.AreEqual(pudelko.B, tab[1]);
            Assert.AreEqual(pudelko.C, tab[2]);
        }

        [TestMethod]
        public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
        {
            var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
            Pudelko p = (a, b, c);
            Assert.AreEqual((int)(p.A * 1000), a);
            Assert.AreEqual((int)(p.B * 1000), b);
            Assert.AreEqual((int)(p.C * 1000), c);
        }

        #endregion

        #region Indexer, enumeration ============================
        [TestMethod]
        public void Indexer_ReadFrom()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            Assert.AreEqual(p.A, p[0]);
            Assert.AreEqual(p.B, p[1]);
            Assert.AreEqual(p.C, p[2]);
        }

        [TestMethod]
        public void ForEach_Test()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            var tab = new[] { p.A, p.B, p.C };
            int i = 0;
            foreach (double x in p)
            {
                Assert.AreEqual(x, tab[i]);
                i++;
            }
        }

        #endregion

        #region Parsing =========================================

        #endregion

    }
}