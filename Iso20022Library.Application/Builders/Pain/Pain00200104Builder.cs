using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200104;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.002.001.04 messages (Customer Payment Status Report).
    /// </summary>
    /// <remarks>
    /// The pain.002.001.04 message is used by financial institutions to provide payment status reports to customers.
    /// This message informs the customer about the status of previously sent payment instructions,
    /// including acceptance, rejection, or processing status information.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00200104Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00200104Builder"/> class.
        /// </summary>
        public Pain00200104Builder()
        {
            _document = new Document
            {
                CstmrPmtStsRpt = new CustomerPaymentStatusReportV04()
            };
        }

        /// <summary>
        /// Sets the group header for the customer payment status report message.
        /// </summary>
        /// <param name="groupHeader">The group header information (GroupHeader52).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when groupHeader is null.</exception>
        public Pain00200104Builder WithGroupHeader(GroupHeader52 groupHeader)
        {
            _document.CstmrPmtStsRpt.GrpHdr = groupHeader ?? throw new ArgumentNullException(nameof(groupHeader));
            return this;
        }

        /// <summary>
        /// Sets the original group information and status for the payment status report.
        /// </summary>
        /// <param name="originalGroupHeader">The original group header information (OriginalGroupHeader1).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalGroupHeader is null.</exception>
        public Pain00200104Builder WithOriginalGroupInformationAndStatus(OriginalGroupHeader1 originalGroupHeader)
        {
            _document.CstmrPmtStsRpt.OrgnlGrpInfAndSts = originalGroupHeader ?? throw new ArgumentNullException(nameof(originalGroupHeader));
            return this;
        }

        /// <summary>
        /// Adds an original payment instruction status entry to the message.
        /// </summary>
        /// <param name="originalPaymentInstruction">The original payment instruction status information (OriginalPaymentInstruction1).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInstruction is null.</exception>
        public Pain00200104Builder AddOriginalPaymentInstructionAndStatus(OriginalPaymentInstruction1 originalPaymentInstruction)
        {
            if (originalPaymentInstruction == null)
                throw new ArgumentNullException(nameof(originalPaymentInstruction));

            _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Add(originalPaymentInstruction);
            return this;
        }

        /// <summary>
        /// Adds multiple original payment instruction status entries to the message.
        /// </summary>
        /// <param name="originalPaymentInstructions">A collection of original payment instruction status information (OriginalPaymentInstruction1).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInstructions is null.</exception>
        public Pain00200104Builder AddOriginalPaymentInstructionsAndStatus(Collection<OriginalPaymentInstruction1> originalPaymentInstructions)
        {
            if (originalPaymentInstructions == null)
                throw new ArgumentNullException(nameof(originalPaymentInstructions));

            foreach (var instruction in originalPaymentInstructions)
            {
                if (instruction != null)
                    _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Add(instruction);
            }
            return this;
        }

        /// <summary>
        /// Validates the current state of the document to ensure required fields are populated.
        /// </summary>
        /// <returns>True if the document is valid; otherwise, false.</returns>
        public bool IsValid()
        {
            return _document?.CstmrPmtStsRpt?.GrpHdr != null &&
                   _document.CstmrPmtStsRpt.OrgnlGrpInfAndSts != null;
        }

        /// <summary>
        /// Validates the current state of the document and throws an exception if invalid.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the document is in an invalid state.</exception>
        private void ValidateDocument()
        {
            if (_document?.CstmrPmtStsRpt?.GrpHdr == null)
                throw new InvalidOperationException("Group header is required.");

            if (_document.CstmrPmtStsRpt.OrgnlGrpInfAndSts == null)
                throw new InvalidOperationException("Original group information and status is required.");
        }

        /// <summary>
        /// Builds the document object.
        /// </summary>
        /// <returns>The constructed Document object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the document is in an invalid state.</exception>
        public Document Build()
        {
            ValidateDocument();
            return _document;
        }

        /// <summary>
        /// Serializes the document to XML.
        /// </summary>
        /// <returns>The XML representation of the document.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the document is in an invalid state.</exception>
        public string BuildXml()
        {
            ValidateDocument();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Serializes the provided document to XML.
        /// </summary>
        /// <param name="message">The document to serialize.</param>
        /// <returns>The XML representation of the document.</returns>
        /// <exception cref="InvalidCastException">Thrown when the message is not a valid Document type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        public string BuildXml(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message is not Document doc)
                throw new InvalidCastException($"Invalid message type. Expected Document, but received {message.GetType().Name}.");

            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Resets the builder to its initial state, clearing all data.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00200104Builder Reset()
        {
            _document.CstmrPmtStsRpt = new CustomerPaymentStatusReportV04();
            return this;
        }

        /// <summary>
        /// Creates a copy of the current builder with the same data.
        /// </summary>
        /// <returns>A new builder instance with the same data as the current builder.</returns>
        public Pain00200104Builder Clone()
        {
            var clonedBuilder = new Pain00200104Builder();

            if (_document.CstmrPmtStsRpt.GrpHdr != null)
                clonedBuilder.WithGroupHeader(_document.CstmrPmtStsRpt.GrpHdr);

            if (_document.CstmrPmtStsRpt.OrgnlGrpInfAndSts != null)
                clonedBuilder.WithOriginalGroupInformationAndStatus(_document.CstmrPmtStsRpt.OrgnlGrpInfAndSts);

            foreach (var instruction in _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts)
                clonedBuilder.AddOriginalPaymentInstructionAndStatus(instruction);

            return clonedBuilder;
        }
    }
}
