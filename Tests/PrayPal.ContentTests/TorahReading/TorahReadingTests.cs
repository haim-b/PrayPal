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
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2020, 12, 14), true), new DummyLogger()).ToList();

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
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2020, 12, 31), true), new DummyLogger()).ToList();

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
            // Get Tazria:
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2021, 4, 15), true), new DummyLogger()).ToList();

            Assert.AreEqual(3, p.Count);

            Assert.AreEqual("וַיְדַבֵּ֥ר", p[0].Content.First().Text.Split(' ')[0], "Wrong Cohen first word.");
            Assert.AreEqual("טָֽהֳרָֽהּ׃", p[0].Content.Last().Text.Split(' ').Last(), "Wrong Cohen last word.");
            Assert.AreEqual("וְאִם־נְקֵבָ֣ה", p[1].Content.First().Text.Split(' ')[0], "Wrong Levi first word.");
            Assert.AreEqual("וְטָהֵֽרָה׃", p[1].Content.Last().Text.Split(' ').Last(), "Wrong Levi last word.");
            Assert.AreEqual("וַיְדַבֵּ֣ר", p[2].Content.First().Text.Split(' ')[0], "Wrong Israel first word.");
            Assert.AreEqual("שֵׁנִֽית׃", p[2].Content.Last().Text.Split(' ').Last(), "Wrong Israel last word.");
        }

        [TestMethod]
        public void TestChangingChapterInTheThirdReader()
        {
            // Get Toldot:
            var p = Parashot.GetParashaReadingForShacharit(new JewishCalendar(new DateTime(2020, 11, 19), true), new DummyLogger()).ToList();

            Assert.AreEqual(3, p.Count);

            Assert.AreEqual("וְאֵ֛לֶּה", p[0].Content.First().Text.Split(' ')[0], "Wrong Cohen first word.");
            Assert.AreEqual("אֶת־יְהוָֽה׃", p[0].Content.Last().Text.Split(' ').Last(), "Wrong Cohen last word.");
            Assert.AreEqual("וַיֹּ֨אמֶר", p[1].Content.First().Text.Split(' ')[0], "Wrong Levi first word.");
            Assert.AreEqual("אֹתָֽם׃", p[1].Content.Last().Text.Split(' ').Last(), "Wrong Levi last word.");
            string expectedThird = "וַֽיִּגְדְּלוּ֙ הַנְּעָרִ֔ים וַיְהִ֣י עֵשָׂ֗ו אִ֛ישׁ יֹדֵ֥עַ צַ֖יִד אִ֣ישׁ שָׂדֶ֑ה וְיַֽעֲקֹב֙ אִ֣ישׁ תָּ֔ם יֹשֵׁ֖ב אֹֽהָלִֽים׃ וַיֶּֽאֱהַ֥ב יִצְחָ֛ק אֶת־עֵשָׂ֖ו כִּי־צַ֣יִד בְּפִ֑יו וְרִבְקָ֖ה אֹהֶ֥בֶת אֶֽת־יַעֲקֹֽב׃ וַיָּ֥זֶד יַֽעֲקֹ֖ב נָזִ֑יד וַיָּבֹ֥א עֵשָׂ֛ו מִן־הַשָּׂדֶ֖ה וְה֥וּא עָיֵֽף׃ וַיֹּ֨אמֶר עֵשָׂ֜ו אֶֽל־יַעֲקֹ֗ב הַלְעִיטֵ֤נִי נָא֙ מִן־הָֽאָדֹ֤ם הָֽאָדֹם֙ הַזֶּ֔ה כִּ֥י עָיֵ֖ף אָנֹ֑כִי עַל־כֵּ֥ן קָרָֽא־שְׁמ֖וֹ אֱדֽוֹם׃ וַיֹּ֖אמֶר יַֽעֲקֹ֑ב מִכְרָ֥ה כַיּ֛וֹם אֶת־בְּכֹֽרָתְךָ֖ לִֽי׃ וַיֹּ֣אמֶר עֵשָׂ֔ו הִנֵּ֛ה אָֽנֹכִ֥י הוֹלֵ֖ךְ לָמ֑וּת וְלָמָּה־זֶּ֥ה לִ֖י בְּכֹרָֽה׃ וַיֹּ֣אמֶר יַֽעֲקֹ֗ב הִשָּׁ֤בְעָה לִּי֙ כַּיּ֔וֹם וַיִּשָּׁבַ֖ע ל֑וֹ וַיִּמְכֹּ֥ר אֶת־בְּכֹֽרָת֖וֹ לְיַֽעֲקֹֽב׃ וְיַֽעֲקֹ֞ב נָתַ֣ן לְעֵשָׂ֗ו לֶ֚חֶם וּנְזִ֣יד עֲדָשִׁ֔ים וַיֹּ֣אכַל וַיֵּ֔שְׁתְּ וַיָּ֖קָם וַיֵּלַ֑ךְ וַיִּ֥בֶז עֵשָׂ֖ו אֶת־הַבְּכֹרָֽה׃ וַיְהִ֤י רָעָב֙ בָּאָ֔רֶץ מִלְּבַד֙ הָֽרָעָ֣ב הָֽרִאשׁ֔וֹן אֲשֶׁ֥ר הָיָ֖ה בִּימֵ֣י אַבְרָהָ֑ם וַיֵּ֧לֶךְ יִצְחָ֛ק אֶל־אֲבִימֶ֥לֶךְ מֶֽלֶךְ־פְּלִשְׁתִּ֖ים גְּרָֽרָה׃ וַיֵּרָ֤א אֵלָיו֙ יְהוָ֔ה וַיֹּ֖אמֶר אַל־תֵּרֵ֣ד מִצְרָ֑יְמָה שְׁכֹ֣ן בָּאָ֔רֶץ אֲשֶׁ֖ר אֹמַ֥ר אֵלֶֽיךָ׃ גּ֚וּר בָּאָ֣רֶץ הַזֹּ֔את וְאֶֽהְיֶ֥ה עִמְּךָ֖ וַאֲבָֽרְכֶ֑ךָּ כִּֽי־לְךָ֣ וּֽלְזַרְעֲךָ֗ אֶתֵּן֙ אֶת־כָּל־הָֽאֲרָצֹ֣ת הָאֵ֔ל וַהֲקִֽמֹתִי֙ אֶת־הַשְּׁבֻעָ֔ה אֲשֶׁ֥ר נִשְׁבַּ֖עְתִּי לְאַבְרָהָ֥ם אָבִֽיךָ׃ וְהִרְבֵּיתִ֤י אֶֽת־זַרְעֲךָ֙ כְּכֽוֹכְבֵ֣י הַשָּׁמַ֔יִם וְנָֽתַתִּ֣י לְזַרְעֲךָ֔ אֵ֥ת כָּל־הָֽאֲרָצֹ֖ת הָאֵ֑ל וְהִתְבָּֽרְכ֣וּ בְזַרְעֲךָ֔ כֹּ֖ל גּוֹיֵ֥י הָאָֽרֶץ׃ עֵ֕קֶב אֲשֶׁר־שָׁמַ֥ע אַבְרָהָ֖ם בְּקֹלִ֑י וַיִּשְׁמֹר֙ מִשְׁמַרְתִּ֔י מִצְו͏ֹתַ֖י חֻקּוֹתַ֥י וְתֽוֹרֹתָֽי׃";
            string actualThird = string.Join(" ", p[2].Content.Select(r => r.Text));

            string[] expected = expectedThird.Split(" ");
            string[] actual = actualThird.Split(" ");

            Assert.AreEqual(expected.Length, actual.Length, "Third is not in the expected length.");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "Wrong word at index {0} for the third reader.", i);
            }
        }
    }
}
