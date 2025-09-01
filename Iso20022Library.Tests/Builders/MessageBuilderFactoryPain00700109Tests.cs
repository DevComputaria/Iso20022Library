using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using System;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class MessageBuilderFactoryPain00700109Tests
    {
        private MessageBuilderFactory _factory = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
        }

        [TestMethod]
        public void GetBuilder_ForPain00700109_ShouldReturnPain00700109Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700109);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00700109Builder));
        }

        [TestMethod]
        public void GetBuilder_Pain00700109Builder_ShouldImplementIMessageBuilder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700109);

            // Assert
            Assert.IsInstanceOfType(builder, typeof(IMessageBuilder));
        }

        [TestMethod]
        public void Pain00700109Builder_ShouldSupportXmlGeneration()
        {
            // Arrange
            var builder = (Pain00700109Builder)_factory.GetBuilder(MessageType.Pain00700109);

            // Act
            builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                   .SetOriginalGroupInformation("ORIG001")
                   .AddOriginalPaymentInstruction("PMT001")
                   .AddPaymentTransactionReversal("REV001");

            var xml = builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("pain.007.001.09"));
        }

        [TestMethod]
        public void GetBuilder_Pain00700109_ShouldHaveCorrectMessageType()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00700109);
            var pain00700109Builder = builder as Pain00700109Builder;

            // Assert
            Assert.IsNotNull(pain00700109Builder);
            Assert.AreEqual(MessageType.Pain00700109, pain00700109Builder.MessageType);
        }

        [TestMethod]
        public void GetBuilder_Pain00700109_ShouldReturnNewInstanceEachTime()
        {
            // Act
            var builder1 = _factory.GetBuilder(MessageType.Pain00700109);
            var builder2 = _factory.GetBuilder(MessageType.Pain00700109);

            // Assert
            Assert.AreNotSame(builder1, builder2);
            Assert.AreEqual(builder1.GetType(), builder2.GetType());
        }
    }
}
