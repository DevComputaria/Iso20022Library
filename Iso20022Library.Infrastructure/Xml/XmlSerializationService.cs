using System.Xml.Serialization;

namespace Iso20022Library.Infrastructure.Xml;

public static class XmlSerializationService
{
    public static string Serialize<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var writer = new Utf8StringWriter();
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }
}
