using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iso20022Library.Domain.Common.Interfaces
{
    public interface IXmlSerializableMessage
    {
        /// <summary>
        /// Serializa o objeto da mensagem ISO 20022 em XML válido.
        /// </summary>
        /// <returns>XML da mensagem como string.</returns>
        string ToXml();
    }
}
