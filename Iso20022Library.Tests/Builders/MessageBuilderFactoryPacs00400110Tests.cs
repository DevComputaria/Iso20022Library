using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Domain.Common.Enums;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class MessageBuilderFactoryPacs00400110Tests
    {
        private MessageBuilderFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
        }

        [TestMethod]
        public void GetBuilder_WithPacs00400110_ShouldReturnPacs00400110Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pacs00400110);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pacs00400110Builder));
        }

        [TestMethod]
        public void GetBuilder_WithPacs00400110_ShouldImplementIMessageBuilder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pacs00400110);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsTrue(builder is Iso20022Library.Domain.Common.Interfaces.IMessageBuilder);
        }

        [TestMethod]
        public void GetBuilder_WithPacs00400110_ShouldBeDifferentInstances()
        {
            // Act
            var builder1 = _factory.GetBuilder(MessageType.Pacs00400110);
            var builder2 = _factory.GetBuilder(MessageType.Pacs00400110);

            // Assert
            Assert.IsNotNull(builder1);
            Assert.IsNotNull(builder2);
            Assert.AreNotSame(builder1, builder2);
        }
    }
}
