using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;

namespace Iso20022Library.Application.Builders;

/// <summary>
/// Factory responsible for providing the appropriate builder for each ISO 20022 message type.
/// </summary>
/// <remarks>
/// This factory implements the Factory Method pattern to create message builders
/// without exposing the instantiation logic to clients. The factory maintains a registry
/// of supported message types and their corresponding builder implementations.
/// 
/// To add support for a new message type:
/// 1. Create a new builder class implementing <see cref="IMessageBuilder"/>
/// 2. Register the builder in the <see cref="_builders"/> dictionary
/// </remarks>
public class MessageBuilderFactory
{
    /// <summary>
    /// Internal dictionary of registered builders indexed by message type.
    /// </summary>
    /// <remarks>
    /// This dictionary acts as a registry for all supported message types and their
    /// corresponding builder instances. Each builder must implement the <see cref="IMessageBuilder"/> interface.
    /// The dictionary is initialized with all supported message types at construction time.
    /// </remarks>
    private readonly Dictionary<MessageType, Func<IMessageBuilder>> _builders = new()
    {
        { MessageType.Pain00100102, () => new Pain.Pain00100102Builder() },
        { MessageType.Pain00100103, () => new Pain.Pain00100103Builder() },
        { MessageType.Pain00100104, () => new Pain00100104Builder() },
        { MessageType.Pain00100106, () => new Pain00100106Builder() },
        { MessageType.Pain00100107, () => new Pain00100107Builder() },
        { MessageType.Pain00100108, () => new Pain00100108Builder() },
        { MessageType.Pain00100109, () => new Pain00100109Builder() },
        { MessageType.Pain00100110, () => new Pain00100110Builder() },
        { MessageType.Pain00200104, () => new Pain.Pain00200104Builder() },
        { MessageType.Pain00200106, () => new Pain.Pain00200106Builder() },
        { MessageType.Pain00200107, () => new Pain.Pain00200107Builder() },
        { MessageType.Pain00200108, () => new Pain.Pain00200108Builder() },
        { MessageType.Pain00200109, () => new Pain.Pain00200109Builder() },
        { MessageType.Pain00200110, () => new Pain.Pain00200110Builder() }
    };

    /// <summary>
    /// Returns the builder corresponding to the specified message type.
    /// </summary>
    /// <param name="type">The ISO 20022 message type identifier.</param>
    /// <returns>An instance of <see cref="IMessageBuilder"/> corresponding to the requested type.</returns>
    /// <exception cref="NotSupportedException">Thrown when there is no registered builder for the provided message type.</exception>
    /// <remarks>
    /// This method implements the factory method pattern by returning the appropriate builder
    /// based on the message type. It performs a lookup in the internal registry and throws
    /// an exception when the requested message type is not supported.
    /// 
    /// Usage example:
    /// <code>
    /// var factory = new MessageBuilderFactory();
    /// var builder = factory.GetBuilder(MessageType.Pain00100103);
    /// var message = builder.BuildXml(documentObject);
    /// </code>
    /// </remarks>
    public IMessageBuilder GetBuilder(MessageType type)
    {
        if (_builders.TryGetValue(type, out var builderFactory))
            return builderFactory();

        throw new NotSupportedException($"Message type {type} not supported.");
    }
}
