using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700110;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.007.001.10 messages (Customer Payment Reversal).
    /// </summary>
    /// <remarks>
    /// The pain.007.001.10 message is used to reverse a previously sent payment instruction.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00700110Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00700110Builder"/> class.
        /// </summary>
        public Pain00700110Builder()
        {
            _document = new Document
            {
                CstmrPmtRvsl = new CustomerPaymentReversalV10()
            };
        }

        /// <summary>
        /// Sets the group header for the payment reversal message.
        /// </summary>
        /// <param name="groupHeader">The group header information (GroupHeader88).</param>
        public Pain00700110Builder WithGroupHeader(GroupHeader88 groupHeader)
        {
            _document.CstmrPmtRvsl.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Sets the original group header information for the payment reversal message.
        /// </summary>
        /// <param name="originalGroupHeader">The original group header information (OriginalGroupHeader16).</param>
        public Pain00700110Builder WithOriginalGroupHeader(OriginalGroupHeader16 originalGroupHeader)
        {
            _document.CstmrPmtRvsl.OrgnlGrpInf = originalGroupHeader;
            return this;
        }

        /// <summary>
        /// Adds an original payment instruction reversal entry to the message.
        /// </summary>
        /// <param name="originalPaymentInstruction">The original payment instruction reversal (OriginalPaymentInstruction37).</param>
        public Pain00700110Builder AddOriginalPaymentInstruction(OriginalPaymentInstruction37 originalPaymentInstruction)
        {
            _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Add(originalPaymentInstruction);
            return this;
        }

        /// <summary>
        /// Adds multiple original payment instruction reversal entries to the message.
        /// </summary>
        /// <param name="originalPaymentInstructions">A collection of original payment instruction reversals (OriginalPaymentInstruction37).</param>
        public Pain00700110Builder AddOriginalPaymentInstructions(Collection<OriginalPaymentInstruction37> originalPaymentInstructions)
        {
            foreach (var item in originalPaymentInstructions)
                _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Add(item);
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
