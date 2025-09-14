using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Domain.Common.Enums;

namespace Iso20022Library.Tests.Builders;

/// <summary>
/// Unit tests for MessageBuilderFactory with Pacs00700110Builder.
/// Tests the factory pattern implementation for PACS.007.001.10 Payment Reversal messages.
/// </summary>
[TestClass]
public class MessageBuilderFactoryPacs00700110Tests
{
    private MessageBuilderFactory _factory;

    [TestInitialize]
    public void Setup()
    {
        _factory = new MessageBuilderFactory();
    }

    [TestMethod]
    public void GetBuilder_WithPacs00700110_ShouldReturnPacs00700110Builder()
    {
        // Act
        var builder = _factory.GetBuilder(MessageType.Pacs00700110);

        // Assert
        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(Pacs00700110Builder));
    }

    [TestMethod]
    public void GetBuilder_WithPacs00700110_ShouldImplementIMessageBuilder()
    {
        // Act
        var builder = _factory.GetBuilder(MessageType.Pacs00700110);

        // Assert
        Assert.IsNotNull(builder);
        Assert.IsTrue(builder is Iso20022Library.Domain.Common.Interfaces.IMessageBuilder);
    }

    [TestMethod]
    public void GetBuilder_WithPacs00700110_ShouldBeDifferentInstances()
    {
        // Act
        var builder1 = _factory.GetBuilder(MessageType.Pacs00700110);
        var builder2 = _factory.GetBuilder(MessageType.Pacs00700110);

        // Assert
        Assert.IsNotNull(builder1);
        Assert.IsNotNull(builder2);
        Assert.AreNotSame(builder1, builder2);
    }
}
