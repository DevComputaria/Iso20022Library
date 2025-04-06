namespace Iso20022Library.Domain.Common.Interfaces;
public interface IMessageBuilder
{
    string BuildXml(object message);
}
