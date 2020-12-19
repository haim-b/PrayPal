using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrayPal.Common;
using PrayPal.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Zmanim.HebrewCalendar;
using System.Linq;

namespace Tests.PrayPal.Content.TorahReading
{
    [TestClass]
    public class TorahReadingTests
    {
        [TestMethod]
        public void TestHanukkahReading()
        {
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2020, 12, 14), true), Prayer.Shacharit, new DummyLogger()).ToList();

            Assert.AreEqual(3, p.Count);

            Assert.AreEqual("בַּיּוֹם֙", p[0].Content.First().Text.Split(' ')[0], "Wrong Cohen first word.");
            Assert.AreEqual("קְטֹֽרֶת׃", p[0].Content.Last().Text.Split(' ').Last(), "Wrong Cohen last word.");
            Assert.AreEqual("פַּ֣ר", p[1].Content.First().Text.Split(' ')[0], "Wrong Levi first word.");
            Assert.AreEqual("בֶּן־שְׁדֵיאֽוּר׃", p[1].Content.Last().Text.Split(' ').Last(), "Wrong Levi last word.");
            Assert.AreEqual("בַּיּוֹם֙", p[2].Content.First().Text.Split(' ')[0], "Wrong Israel first word.");
            Assert.AreEqual("בֶּן־שְׁדֵיאֽוּר׃", p[2].Content.Last().Text.Split(' ').Last(), "Wrong Israel last word.");
        }

        [TestMethod]
        public void TestChangingChapterBetweenFirstAndSecondReaders()
        {
            // Get Vayechi:
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2020, 12, 31), true), Prayer.Shacharit, new DummyLogger()).ToList();

            Assert.AreEqual(3, p.Count);

            Assert.AreEqual("וַיְחִ֤י", p[0].Content.First().Text.Split(' ')[0], "Wrong Cohen first word.");
            Assert.AreEqual("הַמִּטָּֽה׃", p[0].Content.Last().Text.Split(' ').Last(), "Wrong Cohen last word.");
            Assert.AreEqual("וַיְהִ֗י", p[1].Content.First().Text.Split(' ')[0], "Wrong Levi first word.");
            Assert.AreEqual("אֹתִֽי׃", p[1].Content.Last().Text.Split(' ').Last(), "Wrong Levi last word.");
            Assert.AreEqual("וַיֹּ֣אמֶר", p[2].Content.First().Text.Split(' ')[0], "Wrong Israel first word.");
            Assert.AreEqual("וַאֲבָֽרְכֵֽם׃", p[2].Content.Last().Text.Split(' ').Last(), "Wrong Israel last word.");
        }

        [TestMethod]
        public void TestChangingChapterBetweenSecondAndThirdReaders()
        {
            // Get Taria:
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2021, 4, 15), true), Prayer.Shacharit, new DummyLogger()).ToList();

            Assert.AreEqual(3, p.Count);

            Assert.AreEqual("וַיְדַבֵּ֥ר", p[0].Content.First().Text.Split(' ')[0], "Wrong Cohen first word.");
            Assert.AreEqual("טָֽהֳרָֽהּ׃", p[0].Content.Last().Text.Split(' ').Last(), "Wrong Cohen last word.");
            Assert.AreEqual("וְאִם־נְקֵבָ֣ה", p[1].Content.First().Text.Split(' ')[0], "Wrong Levi first word.");
            Assert.AreEqual("וְטָהֵֽרָה׃", p[1].Content.Last().Text.Split(' ').Last(), "Wrong Levi last word.");
            Assert.AreEqual("וַיְדַבֵּ֣ר", p[2].Content.First().Text.Split(' ')[0], "Wrong Israel first word.");
            Assert.AreEqual("שֵׁנִֽית׃", p[2].Content.Last().Text.Split(' ').Last(), "Wrong Israel last word.");
        }
    }
}
