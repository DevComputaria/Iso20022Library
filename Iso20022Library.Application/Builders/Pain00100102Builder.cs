using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Pain00100102.Generated;

namespace Iso20022Library.Application.Builders;

/// <summary>
/// Builder for creating and serializing ISO 20022 pain.001.001.02 messages (Customer Credit Transfer Initiation).
/// </summary>
/// <remarks>
/// The pain.001.001.02 message is used to initiate credit transfer instructions from a debtor to a creditor.
/// This builder handles the serialization of the message to XML format according to ISO 20022 standards.
/// </remarks>
public class Pain00100102Builder : IMessageBuilder
{
    /// <summary>
    /// Builds an XML representation of the pain.001.001.02 message.
    /// </summary>
    /// <param name="message">The message object to serialize. Must be an instance of <see cref="Document"/>.</param>
    /// <returns>A string containing the XML representation of the message.</returns>
    /// <exception cref="InvalidCastException">Thrown when the provided message is not of type <see cref="Document"/>.</exception>
    /// <remarks>
    /// This method validates that the input is the correct message type before serializing it to XML.
    /// The resulting XML conforms to the pain.001.001.02 schema specifications.
    /// </remarks>
    public string BuildXml(object message)
    {
        if (message is not Document doc)
            throw new InvalidCastException("Invalid message type.");
        return XmlSerializationService.Serialize(doc);
    }
}
