using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700107;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Tests for MessageBuilderFactory Pain.007.001.07 builder registration.
    /// </summary>
    [TestClass]
    public class MessageBuilderFactoryPain00700107Tests
    {
        private MessageBuilderFactory _factory = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
        }

        [TestMethod]
        public void GetBuilder_WithPain00700107_ShouldReturnPain00700107Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700107);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00700107Builder));

            var concreteBuilder = (Pain00700107Builder)builder;
            Assert.AreEqual(MessageType.Pain00700107, concreteBuilder.MessageType);
        }

        [TestMethod]
        public void GetBuilder_WithPain00700107_ShouldReturnIMessageBuilderInterface()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700107);

            // Assert
            Assert.IsInstanceOfType(builder, typeof(IMessageBuilder));
        }

        [TestMethod]
        public void GetBuilder_WithPain00700107_BuilderShouldWorkCorrectly()
        {
            // Arrange
            var builder = _factory.GetBuilder(MessageType.Pain00700107);
            var pain00700107Builder = (Pain00700107Builder)builder;

            // Act
            var document = pain00700107Builder
                .SetGroupHeader("MSG123", DateTime.Now, "1")
                .SetOriginalGroupInformation("ORIG123", "pain.001.001.03")
                .AddOriginalPaymentInstruction(null, "PMT123")
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsInstanceOfType(document, typeof(Document));
            Assert.IsNotNull(document.CstmrPmtRvsl);
            Assert.IsInstanceOfType(document.CstmrPmtRvsl, typeof(CustomerPaymentReversalV07));
        }

        [TestMethod]
        public void GetBuilder_WithPain00700107_ShouldProduceValidXml()
        {
            // Arrange
            var builder = _factory.GetBuilder(MessageType.Pain00700107);
            var pain00700107Builder = (Pain00700107Builder)builder;

            // Act
            var xml = pain00700107Builder
                .SetGroupHeader("MSG123", DateTime.Now, "1")
                .SetOriginalGroupInformation("ORIG123", "pain.001.001.03")
                .AddOriginalPaymentInstruction(null, "PMT123")
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.StartsWith("<?xml"));
            Assert.IsTrue(xml.Contains("Document"));
            Assert.IsTrue(xml.Contains("CstmrPmtRvsl"));
        }

        [TestMethod]
        public void MessageBuilderFactory_ShouldHavePain00700107Registered()
        {
            // Act & Assert - Should not throw exception
            var builder = _factory.GetBuilder(MessageType.Pain00700107);
            Assert.IsNotNull(builder);
        }
    }
}
