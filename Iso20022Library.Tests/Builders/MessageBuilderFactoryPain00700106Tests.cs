using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using System;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Tests for MessageBuilderFactory to ensure Pain00700106Builder is properly registered.
    /// </summary>
    [TestClass]
    public class MessageBuilderFactoryPain00700106Tests
    {
        [TestMethod]
        public void GetBuilder_WithPain00700106_ShouldReturnPain00700106Builder()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act
            var builder = factory.GetBuilder(MessageType.Pain00700106);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00700106Builder));
            Assert.AreEqual(MessageType.Pain00700106, ((Pain00700106Builder)builder).MessageType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetBuilder_WithUnsupportedMessageType_ShouldThrowNotSupportedException()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act - This should throw since we're using an invalid enum value
            factory.GetBuilder((MessageType)99999);
        }

        [TestMethod]
        public void GetBuilder_CreateMultipleInstances_ShouldReturnDifferentInstances()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act
            var builder1 = factory.GetBuilder(MessageType.Pain00700106);
            var builder2 = factory.GetBuilder(MessageType.Pain00700106);

            // Assert
            Assert.IsNotNull(builder1);
            Assert.IsNotNull(builder2);
            Assert.AreNotSame(builder1, builder2);
            Assert.IsInstanceOfType(builder1, typeof(Pain00700106Builder));
            Assert.IsInstanceOfType(builder2, typeof(Pain00700106Builder));
        }
    }
}
