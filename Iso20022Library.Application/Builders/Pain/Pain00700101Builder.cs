using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700101;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.007.001.01 messages (Customer Payment Reversal).
    /// </summary>
    /// <remarks>
    /// The pain.007.001.01 message is used to reverse a previously sent payment instruction.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00700101Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00700101Builder"/> class.
        /// </summary>
        public Pain00700101Builder()
        {
            _document = new Document
            {
                Pain00700101 = new Pain00700101()
            };
        }

        /// <summary>
        /// Sets the group header for the payment reversal message.
        /// </summary>
        public Pain00700101Builder WithGroupHeader(GroupHeader8 groupHeader)
        {
            _document.Pain00700101.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Sets the original group information for the payment reversal message.
        /// </summary>
        public Pain00700101Builder WithOriginalGroupInfo(OriginalGroupInformation5 originalGroupInfo)
        {
            _document.Pain00700101.OrgnlGrpInf = originalGroupInfo;
            return this;
        }

        /// <summary>
        /// Adds a payment transaction information entry to the message.
        /// </summary>
        public Pain00700101Builder AddTransaction(PaymentTransactionInformation4 transaction)
        {
            _document.Pain00700101.TxInf.Add(transaction);
            return this;
        }

        /// <summary>
        /// Adds multiple payment transaction information entries to the message.
        /// </summary>
        public Pain00700101Builder AddTransactions(Collection<PaymentTransactionInformation4> transactions)
        {
            foreach (var tx in transactions)
                _document.Pain00700101.TxInf.Add(tx);
            return this;
        }

        /// <summary>
        /// Builds the document object.
        /// </summary>
        public Document Build() => _document;

        /// <summary>
        /// Serializes the document to XML.
        /// </summary>
        public string BuildXml() => XmlSerializationService.Serialize(_document);

        /// <summary>
        /// Serializes the provided document to XML.
        /// </summary>
        public string BuildXml(object message)
        {
            if (message is not Document doc)
                throw new InvalidCastException("Invalid message type.");
            return XmlSerializationService.Serialize(doc);
        }
    }
}
