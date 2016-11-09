
namespace Popsicle.Tests.Forms.Pipelines
{
    using System;
    using System.Web;
    using KKings.Foundation.Popsicle.Forms;
    using KKings.Foundation.Popsicle.Forms.Items;
    using KKings.Foundation.Popsicle.Pipelines.ExpandShortCodes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sitecore.Abstractions;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb;
    using Match = System.Text.RegularExpressions.Match;

    [TestClass]
    public class MarketingFormExpanderUnitTests
    {
        public Mock<MarketingFormExpander> ExpanderMock { get; set; }

        public ID TestSitecoreId1 = ID.NewID;
        public ID TestSitecoreId2 = ID.NewID;

        [TestInitialize]
        public void Setup()
        {
            var generator = new Mock<IMarkupGenerator>();
            generator.Setup(m => m.Generate(It.IsAny<MarketingFormItem>()))
                     .Returns((MarketingFormItem form) => $"Markup for {form.ID}");

            var factoryMock = new Mock<BaseFactory>();
            factoryMock.Setup(m => m.CreateObject("forms/generator", true))
                .Returns(generator.Object);

            var expanderMock = new Mock<MarketingFormExpander>(factoryMock.Object) { CallBase = true };

            expanderMock.Setup(m => m.IsPreviewMode)
                .Returns(false);

            expanderMock.Setup(m => m.Token)
                        .Returns("form");


            this.ExpanderMock = expanderMock;
        }

        [TestMethod]
        public void SimpleHtmlContainsForm_ShouldReturnShortcode()
        {
            this.ExpanderMock.Setup(m => m.ConvertMatch(It.IsAny<Match>()))
                .Returns((Match m) => new RegexMatch(m.Value, m.Value));

            this.ExpanderMock.Setup(m => m.ExpandShortCode(It.IsAny<RegexMatch>()))
                .Returns((RegexMatch m) => new ShortCode
                {
                    Expanded = m.Value,
                    Unexpanded = m.Value
                });


            var expander = this.ExpanderMock.Object;
            var content = $"<span style=\"color:blue;\">[form id=\"{this.TestSitecoreId1}\"]</span>";

            // Act
            var tokens = expander.GetShortCodes(content);

            // Assert
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
        }

        [TestMethod]
        public void ComplexHtmlContainsForm_ShouldReturnShortcode()
        {
            this.ExpanderMock.Setup(m => m.ConvertMatch(It.IsAny<Match>()))
                .Returns((Match m) => new RegexMatch(m.Value, m.Value));
            this.ExpanderMock.Setup(m => m.ExpandShortCode(It.IsAny<RegexMatch>()))
                .Returns((RegexMatch m) => new ShortCode
                {
                    Expanded = m.Value,
                    Unexpanded = m.Value
                });


            var expander = this.ExpanderMock.Object;
            var content = "<h1>Testing Use Case</h1>\n\n" +
                          "<div>" +
                          $"<span style=\"color:blue;\">[form id=\"{this.TestSitecoreId1}\"]</span>" +
                          "<br/></div>";

            // Act
            var tokens = expander.GetShortCodes(content);

            // Assert
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
        }


        [TestMethod]
        public void SimpleHtmlContainsMultipleForms_ShouldReturnShortcode()
        {
            this.ExpanderMock.Setup(m => m.ConvertMatch(It.IsAny<Match>()))
                .Returns((Match m) => new RegexMatch(m.Value, m.Value));

            this.ExpanderMock.Setup(m => m.ExpandShortCode(It.IsAny<RegexMatch>()))
                .Returns((RegexMatch m) => new ShortCode
                {
                    Expanded = m.Value,
                    Unexpanded = m.Value
                });

            var expander = this.ExpanderMock.Object;
            var content = $"<span style=\"color:blue;\">[form id=\"{this.TestSitecoreId1}\"]</span>\n" +
                          $"<span style=\"color:blue;\">[form id=\"{this.TestSitecoreId2}\"]</span>";

            // Act
            var tokens = expander.GetShortCodes(content);

            // Assert
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count);
        }

        [TestMethod]
        public void ComplexHtmlContainsMultipleForms_ShouldReturnShortcode()
        {
            this.ExpanderMock.Setup(m => m.ConvertMatch(It.IsAny<Match>()))
                .Returns((Match m) => new RegexMatch(m.Value, m.Value));
            this.ExpanderMock.Setup(m => m.ExpandShortCode(It.IsAny<RegexMatch>()))
                .Returns((RegexMatch m) => new ShortCode
                {
                    Expanded = m.Value,
                    Unexpanded = m.Value
                });


            var expander = this.ExpanderMock.Object;
            var content = "<div><h1>\nHello</h1>" +
                          "<ul>" +
                          $"<li><span style=\"color:blue;\">[form id=\"{this.TestSitecoreId1}\"]</span>\n</li>" +
                          $"<li><span style=\"color:blue;\">[form id=\"{this.TestSitecoreId2}\"]</span>" +
                          "<a href=\"\"/>" +
                          "</li>" +
                          "</div>";

            // Act
            var tokens = expander.GetShortCodes(content);

            // Assert
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count);
        }

        [TestMethod]
        public void MatchWithForm_ShouldReturnExpandedShortcode()
        {
            // Assert

            using (var db = new Sitecore.FakeDb.Db
            {
                new DbItem("Example", this.TestSitecoreId1, MarketingFormItem.Constants.MarketingFormTemplateId) { { "Title", "Welcome!" } }
            })
            {
                var match = new RegexMatch("<test></test>", this.TestSitecoreId1.ToString());
                var expander = this.ExpanderMock.Object;
                var expected = $"Markup for {this.TestSitecoreId1}";

                // Act
                var shortCode = expander.ExpandShortCode(match);

                // Arrange
                Assert.IsNotNull(shortCode);
                Assert.AreEqual(expected, shortCode.Expanded);
            }
        }


        [TestMethod]
        public void MatchWithInvalidForm_ShouldReturnMessageInPreviewMode()
        {
            // Assert

            using (var db = new Sitecore.FakeDb.Db
            {
                new DbItem("Example", this.TestSitecoreId1, MarketingFormItem.Constants.MarketingFormTemplateId) { { "Title", "Welcome!" } }
            })
            {
                var id = ID.NewID;
                var match = new RegexMatch("<test></test>", id.ToString());
                var expanderMock = this.ExpanderMock;
                expanderMock.Setup(m => m.IsPreviewMode).Returns(true);

                var expander = expanderMock.Object;
                var expected = $"<!-- {id} was not found. -->";

                // Act
                var shortCode = expander.ExpandShortCode(match);

                // Arrange
                Assert.IsNotNull(shortCode);
                Assert.AreEqual(expected, shortCode.Expanded);
            }
        }
    }
}
