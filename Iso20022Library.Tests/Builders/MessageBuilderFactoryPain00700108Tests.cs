using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for MessageBuilderFactory Pain00700108Builder integration.
    /// </summary>
    [TestClass]
    public class MessageBuilderFactoryPain00700108Tests
    {
        private MessageBuilderFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
        }

        [TestMethod]
        public void GetBuilder_ForPain00700108_ShouldReturnPain00700108Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700108);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00700108Builder));
            var typedBuilder = (Pain00700108Builder)builder;
            Assert.AreEqual(MessageType.Pain00700108, typedBuilder.MessageType);
        }

        [TestMethod]
        public void GetBuilder_ForPain00700108_ShouldReturnNewInstanceEachTime()
        {
            // Act
            var builder1 = _factory.GetBuilder(MessageType.Pain00700108);
            var builder2 = _factory.GetBuilder(MessageType.Pain00700108);

            // Assert
            Assert.IsNotNull(builder1);
            Assert.IsNotNull(builder2);
            Assert.AreNotSame(builder1, builder2);
        }

        [TestMethod]
        public void GetBuilder_ForPain00700108_ShouldHaveCorrectMessageType()
        {
            // Act
            var builder = (Pain00700108Builder)_factory.GetBuilder(MessageType.Pain00700108);

            // Assert
            Assert.AreEqual(MessageType.Pain00700108, builder.MessageType);
        }

        [TestMethod]
        public void GetBuilder_ForPain00700108_ShouldBeUsableToGenerateXml()
        {
            // Arrange
            var builder = (Pain00700108Builder)_factory.GetBuilder(MessageType.Pain00700108);

            // Act
            var xml = builder
                .SetGroupHeader("MSG001", System.DateTime.Now, "1")
                .SetOriginalGroupInformation("ORIG001")
                .AddOriginalPaymentInstruction("RVSL123", "PMT123")
                .ToXml();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            Assert.IsTrue(xml.Contains("pain.007.001.08"));
        }

        [TestMethod]
        public void GetBuilder_ForPain00700108_ShouldImplementIMessageBuilder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700108);

            // Assert
            Assert.IsInstanceOfType(builder, typeof(Domain.Common.Interfaces.IMessageBuilder));
        }
    }
}
