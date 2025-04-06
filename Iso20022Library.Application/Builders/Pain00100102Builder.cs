using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Pain00100102.Generated;

namespace Iso20022Library.Application.Builders;

/// <summary>
/// Builder responsável por serializar a mensagem pain.001.001.02.
/// </summary>
public class Pain00100102Builder : IMessageBuilder
{
    public string BuildXml(object message)
    {
        if (message is not Document doc)
            throw new InvalidCastException("Invalid message type.");
        return XmlSerializationService.Serialize(doc);
    }
}
