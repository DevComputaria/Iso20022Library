using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200106;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.002.001.06 messages (Customer Payment Status Report V06).
    /// </summary>
    /// <remarks>
    /// The pain.002.001.06 message is used by financial institutions to provide payment status reports to customers.
    /// This message informs the customer about the status of previously sent payment instructions,
    /// including acceptance, rejection, or processing status information. This version 06 includes
    /// enhanced features and additional fields compared to earlier versions.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00200106Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00200106Builder"/> class.
        /// </summary>
        public Pain00200106Builder()
        {
            _document = new Document
            {
                CstmrPmtStsRpt = new CustomerPaymentStatusReportV06()
            };
        }

        /// <summary>
        /// Sets the group header for the customer payment status report message.
        /// </summary>
        /// <param name="groupHeader">The group header information (GroupHeader52).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when groupHeader is null.</exception>
        public Pain00200106Builder WithGroupHeader(GroupHeader52 groupHeader)
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
        public Pain00200106Builder WithOriginalGroupInformationAndStatus(OriginalGroupHeader1 originalGroupHeader)
        {
            _document.CstmrPmtStsRpt.OrgnlGrpInfAndSts = originalGroupHeader ?? throw new ArgumentNullException(nameof(originalGroupHeader));
            return this;
        }

        /// <summary>
        /// Adds an original payment instruction status entry to the message.
        /// </summary>
        /// <param name="originalPaymentInstruction">The original payment instruction status information (OriginalPaymentInstruction12).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInstruction is null.</exception>
        public Pain00200106Builder AddOriginalPaymentInstructionAndStatus(OriginalPaymentInstruction12 originalPaymentInstruction)
        {
            if (originalPaymentInstruction == null)
                throw new ArgumentNullException(nameof(originalPaymentInstruction));

            _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Add(originalPaymentInstruction);
            return this;
        }

        /// <summary>
        /// Adds multiple original payment instruction status entries to the message.
        /// </summary>
        /// <param name="originalPaymentInstructions">A collection of original payment instruction status information (OriginalPaymentInstruction12).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInstructions is null.</exception>
        public Pain00200106Builder AddOriginalPaymentInstructionsAndStatus(Collection<OriginalPaymentInstruction12> originalPaymentInstructions)
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
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="supplementaryData">The supplementary data (SupplementaryData1).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        public Pain00200106Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            if (supplementaryData == null)
                throw new ArgumentNullException(nameof(supplementaryData));

            _document.CstmrPmtStsRpt.SplmtryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Adds multiple supplementary data entries to the message.
        /// </summary>
        /// <param name="supplementaryDataCollection">A collection of supplementary data (SupplementaryData1).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryDataCollection is null.</exception>
        public Pain00200106Builder AddSupplementaryDataCollection(Collection<SupplementaryData1> supplementaryDataCollection)
        {
            if (supplementaryDataCollection == null)
                throw new ArgumentNullException(nameof(supplementaryDataCollection));

            foreach (var data in supplementaryDataCollection)
            {
                if (data != null)
                    _document.CstmrPmtStsRpt.SplmtryData.Add(data);
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
                throw new InvalidOperationException("Group header is required for pain.002.001.06 message.");

            if (_document.CstmrPmtStsRpt.OrgnlGrpInfAndSts == null)
                throw new InvalidOperationException("Original group information and status is required for pain.002.001.06 message.");
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
        public Pain00200106Builder Reset()
        {
            _document.CstmrPmtStsRpt = new CustomerPaymentStatusReportV06();
            return this;
        }

        /// <summary>
        /// Creates a copy of the current builder with the same data.
        /// </summary>
        /// <returns>A new builder instance with the same data as the current builder.</returns>
        public Pain00200106Builder Clone()
        {
            var clonedBuilder = new Pain00200106Builder();

            if (_document.CstmrPmtStsRpt.GrpHdr != null)
                clonedBuilder.WithGroupHeader(_document.CstmrPmtStsRpt.GrpHdr);

            if (_document.CstmrPmtStsRpt.OrgnlGrpInfAndSts != null)
                clonedBuilder.WithOriginalGroupInformationAndStatus(_document.CstmrPmtStsRpt.OrgnlGrpInfAndSts);

            foreach (var instruction in _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts)
                clonedBuilder.AddOriginalPaymentInstructionAndStatus(instruction);

            foreach (var data in _document.CstmrPmtStsRpt.SplmtryData)
                clonedBuilder.AddSupplementaryData(data);

            return clonedBuilder;
        }

        /// <summary>
        /// Gets the current number of original payment instructions in the message.
        /// </summary>
        /// <returns>The count of original payment instructions.</returns>
        public int GetOriginalPaymentInstructionCount()
        {
            return _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count;
        }

        /// <summary>
        /// Gets the current number of supplementary data entries in the message.
        /// </summary>
        /// <returns>The count of supplementary data entries.</returns>
        public int GetSupplementaryDataCount()
        {
            return _document.CstmrPmtStsRpt.SplmtryData.Count;
        }

        /// <summary>
        /// Clears all original payment instructions from the message.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00200106Builder ClearOriginalPaymentInstructions()
        {
            _document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Clear();
            return this;
        }

        /// <summary>
        /// Clears all supplementary data from the message.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00200106Builder ClearSupplementaryData()
        {
            _document.CstmrPmtStsRpt.SplmtryData.Clear();
            return this;
        }
    }
}
