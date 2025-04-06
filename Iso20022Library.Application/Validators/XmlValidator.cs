using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml;

namespace Iso20022Library.Application.Validators
{
    public class XmlValidator
    {
        /// <summary>
        /// Valida um XML string contra um schema XSD.
        /// </summary>
        /// <param name="xml">Conteúdo XML em string.</param>
        /// <param name="xsdPath">Caminho do arquivo XSD.</param>
        /// <param name="validationErrors">Saída com mensagens de erro, se houver.</param>
        /// <returns>True se o XML for válido; caso contrário, false.</returns>
        public static bool Validate(string xml, string xsdPath, out string validationErrors)
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, xsdPath);

            var sb = new StringBuilder();
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemaSet
            };

            settings.ValidationEventHandler += (sender, args) =>
            {
                sb.AppendLine(args.Message);
            };

            try
            {
                using var reader = XmlReader.Create(new StringReader(xml), settings);
                while (reader.Read()) { }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Exception: {ex.Message}");
            }

            validationErrors = sb.ToString();
            return string.IsNullOrEmpty(validationErrors);
        }
    }
}
