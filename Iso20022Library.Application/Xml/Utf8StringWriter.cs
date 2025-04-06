using System.Text;

namespace Iso20022Library.Application.Xml;

public class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}
