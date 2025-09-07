using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Domain.Common.Enums;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class MessageBuilderFactoryPacs00200111Tests
    {
        private MessageBuilderFactory _factory = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
        }

        [TestMethod]
        public void GetBuilder_WithPacs00200111MessageType_ShouldReturnPacs00200111Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pacs00200111);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pacs00200111Builder));
            Assert.AreEqual(MessageType.Pacs00200111, ((Pacs00200111Builder)builder).MessageType);
        }

        [TestMethod]
        public void GetBuilder_WithPacs00200111MessageType_ShouldReturnNewInstanceEachTime()
        {
            // Act
            var builder1 = _factory.GetBuilder(MessageType.Pacs00200111);
            var builder2 = _factory.GetBuilder(MessageType.Pacs00200111);

            // Assert
            Assert.IsNotNull(builder1);
            Assert.IsNotNull(builder2);
            Assert.AreNotSame(builder1, builder2);
            Assert.IsInstanceOfType(builder1, typeof(Pacs00200111Builder));
            Assert.IsInstanceOfType(builder2, typeof(Pacs00200111Builder));
        }

        [TestMethod]
        public void GetBuilder_WithPacs00200111MessageType_ShouldHaveCorrectMessageType()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pacs00200111) as Pacs00200111Builder;

            // Assert
            Assert.IsNotNull(builder);
            Assert.AreEqual(MessageType.Pacs00200111, builder.MessageType);
        }
    }
}
