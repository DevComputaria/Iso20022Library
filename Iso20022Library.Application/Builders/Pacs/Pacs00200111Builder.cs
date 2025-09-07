using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00200111;
using Iso20022Library.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iso20022Library.Application.Builders.Pacs
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pacs.002.001.11 messages (FI To FI Payment Status Report V11).
    /// </summary>
    /// <remarks>
    /// The pacs.002.001.11 message is used by financial institutions to send payment status reports to other financial institutions.
    /// This message provides status information about payments that have been processed, including acceptance, rejection, 
    /// or other status updates of payment instructions between financial institutions.
    /// 
    /// Key features of this message:
    /// - Provides status updates for interbank payments
    /// - Supports both group-level and transaction-level status reporting
    /// - Includes original payment information for reference
    /// - Supports supplementary data for additional information
    /// 
    /// This builder handles the construction of the complete message structure and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pacs00200111Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pacs00200111;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pacs00200111Builder"/> class.
        /// </summary>
        public Pacs00200111Builder()
        {
            _document = new Document
            {
                FIToFIPmtStsRpt = new FIToFIPaymentStatusReportV11()
            };
        }

        /// <summary>
        /// Sets the group header for the FI to FI payment status report message.
        /// </summary>
        /// <param name="messageId">Unique identification assigned by the instructing party to unambiguously identify the message.</param>
        /// <param name="creationDateTime">Date and time at which the message was created.</param>
        /// <param name="instructingAgent">Agent that instructs the next party in the chain to carry out the (set of) instruction(s).</param>
        /// <param name="instructedAgent">Agent that is instructed by the previous party in the chain to carry out the (set of) instruction(s).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        /// <exception cref="ArgumentException">Thrown when messageId is null or empty.</exception>
        public Pacs00200111Builder WithGroupHeader(
            string messageId,
            DateTime creationDateTime,
            BranchAndFinancialInstitutionIdentification6? instructingAgent = null,
            BranchAndFinancialInstitutionIdentification6? instructedAgent = null)
        {
            if (string.IsNullOrEmpty(messageId))
                throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));

            _document.FIToFIPmtStsRpt.GrpHdr = new GroupHeader91
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                InstgAgt = instructingAgent,
                InstdAgt = instructedAgent
            };

            return this;
        }

        /// <summary>
        /// Sets the group header using a pre-configured GroupHeader91 object.
        /// </summary>
        /// <param name="groupHeader">The group header information (GroupHeader91).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when groupHeader is null.</exception>
        public Pacs00200111Builder WithGroupHeader(GroupHeader91 groupHeader)
        {
            _document.FIToFIPmtStsRpt.GrpHdr = groupHeader ?? throw new ArgumentNullException(nameof(groupHeader));
            return this;
        }

        /// <summary>
        /// Adds original group information and status to the payment status report.
        /// </summary>
        /// <param name="originalGroupInformation">The original group header information and status (OriginalGroupHeader17).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalGroupInformation is null.</exception>
        public Pacs00200111Builder AddOriginalGroupInformationAndStatus(OriginalGroupHeader17 originalGroupInformation)
        {
            if (originalGroupInformation == null)
                throw new ArgumentNullException(nameof(originalGroupInformation));

            var currentList = _document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts?.ToList() ?? new List<OriginalGroupHeader17>();
            currentList.Add(originalGroupInformation);
            _document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts = currentList.ToArray();

            return this;
        }

        /// <summary>
        /// Creates and adds original group information and status with the specified parameters.
        /// </summary>
        /// <param name="originalMessageId">Original message identification.</param>
        /// <param name="originalMessageNameId">Original message name identification.</param>
        /// <param name="originalCreationDateTime">Original creation date time.</param>
        /// <param name="groupStatus">Status of the group.</param>
        /// <param name="statusReasonInformation">Additional status reason information.</param>
        /// <param name="numberOfTransactionsPerStatus">Number of transactions per status.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when originalMessageId is null or empty.</exception>
        public Pacs00200111Builder AddOriginalGroupInformationAndStatus(
            string originalMessageId,
            string originalMessageNameId,
            DateTime? originalCreationDateTime = null,
            string? groupStatus = null,
            StatusReasonInformation12[]? statusReasonInformation = null,
            NumberOfTransactionsPerStatus5[]? numberOfTransactionsPerStatus = null)
        {
            if (string.IsNullOrEmpty(originalMessageId))
                throw new ArgumentException("Original message ID cannot be null or empty.", nameof(originalMessageId));

            var originalGroupInfo = new OriginalGroupHeader17
            {
                OrgnlMsgId = originalMessageId,
                OrgnlMsgNmId = originalMessageNameId,
                GrpSts = groupStatus,
                StsRsnInf = statusReasonInformation,
                NbOfTxsPerSts = numberOfTransactionsPerStatus
            };

            if (originalCreationDateTime.HasValue)
            {
                originalGroupInfo.OrgnlCreDtTm = originalCreationDateTime.Value;
                originalGroupInfo.OrgnlCreDtTmSpecified = true;
            }

            return AddOriginalGroupInformationAndStatus(originalGroupInfo);
        }

        /// <summary>
        /// Adds transaction information and status to the payment status report.
        /// </summary>
        /// <param name="transactionInformation">The transaction information and status (PaymentTransaction123).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when transactionInformation is null.</exception>
        public Pacs00200111Builder AddTransactionInformationAndStatus(PaymentTransaction123 transactionInformation)
        {
            if (transactionInformation == null)
                throw new ArgumentNullException(nameof(transactionInformation));

            var currentList = _document.FIToFIPmtStsRpt.TxInfAndSts?.ToList() ?? new List<PaymentTransaction123>();
            currentList.Add(transactionInformation);
            _document.FIToFIPmtStsRpt.TxInfAndSts = currentList.ToArray();

            return this;
        }

        /// <summary>
        /// Creates and adds transaction information and status with the specified parameters.
        /// </summary>
        /// <param name="statusId">Status identification.</param>
        /// <param name="originalInstructionId">Original instruction identification.</param>
        /// <param name="originalEndToEndId">Original end to end identification.</param>
        /// <param name="originalTransactionId">Original transaction identification.</param>
        /// <param name="transactionStatus">Transaction status.</param>
        /// <param name="statusReasonInformation">Status reason information.</param>
        /// <param name="chargesInformation">Charges information.</param>
        /// <param name="acceptanceDateTime">Acceptance date time.</param>
        /// <param name="accountServicerReference">Account servicer reference.</param>
        /// <param name="clearingSystemReference">Clearing system reference.</param>
        /// <param name="instructingAgent">Instructing agent.</param>
        /// <param name="instructedAgent">Instructed agent.</param>
        /// <param name="originalTransactionReference">Original transaction reference.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pacs00200111Builder AddTransactionInformationAndStatus(
            string? statusId = null,
            string? originalInstructionId = null,
            string? originalEndToEndId = null,
            string? originalTransactionId = null,
            string? transactionStatus = null,
            StatusReasonInformation12[]? statusReasonInformation = null,
            Charges7[]? chargesInformation = null,
            DateTime? acceptanceDateTime = null,
            string? accountServicerReference = null,
            string? clearingSystemReference = null,
            BranchAndFinancialInstitutionIdentification6? instructingAgent = null,
            BranchAndFinancialInstitutionIdentification6? instructedAgent = null,
            OriginalTransactionReference31? originalTransactionReference = null)
        {
            var transactionInfo = new PaymentTransaction123
            {
                StsId = statusId,
                OrgnlInstrId = originalInstructionId,
                OrgnlEndToEndId = originalEndToEndId,
                OrgnlTxId = originalTransactionId,
                TxSts = transactionStatus,
                StsRsnInf = statusReasonInformation,
                ChrgsInf = chargesInformation,
                AcctSvcrRef = accountServicerReference,
                ClrSysRef = clearingSystemReference,
                InstgAgt = instructingAgent,
                InstdAgt = instructedAgent,
                OrgnlTxRef = originalTransactionReference
            };

            if (acceptanceDateTime.HasValue)
            {
                transactionInfo.AccptncDtTm = acceptanceDateTime.Value;
                transactionInfo.AccptncDtTmSpecified = true;
            }

            return AddTransactionInformationAndStatus(transactionInfo);
        }

        /// <summary>
        /// Adds supplementary data to the payment status report.
        /// </summary>
        /// <param name="supplementaryData">The supplementary data to add (SupplementaryData1).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        public Pacs00200111Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            if (supplementaryData == null)
                throw new ArgumentNullException(nameof(supplementaryData));

            var currentList = _document.FIToFIPmtStsRpt.SplmtryData?.ToList() ?? new List<SupplementaryData1>();
            currentList.Add(supplementaryData);
            _document.FIToFIPmtStsRpt.SplmtryData = currentList.ToArray();

            return this;
        }

        /// <summary>
        /// Creates and adds supplementary data with the specified parameters.
        /// </summary>
        /// <param name="envelope">The envelope containing the supplementary data.</param>
        /// <param name="placementAndDate">Placement and date information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when envelope is null.</exception>
        public Pacs00200111Builder AddSupplementaryData(
            System.Xml.XmlElement envelope,
            string? placementAndDate = null)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = placementAndDate,
                Envlp = envelope
            };

            return AddSupplementaryData(supplementaryData);
        }

        /// <summary>
        /// Builds the XML string representation of the pacs.002.001.11 message.
        /// </summary>
        /// <param name="message">The message object to serialize (not used in this implementation).</param>
        /// <returns>The XML string representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
        public string BuildXml(object message)
        {
            ValidateMessage();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Builds the XML string representation of the pacs.002.001.11 message.
        /// </summary>
        /// <returns>The XML string representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
        public string BuildXml()
        {
            ValidateMessage();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Gets the current document being built.
        /// </summary>
        /// <returns>The current document instance.</returns>
        public Document GetDocument()
        {
            return _document;
        }

        /// <summary>
        /// Gets the current FI to FI payment status report being built.
        /// </summary>
        /// <returns>The current FIToFIPaymentStatusReportV11 instance.</returns>
        public FIToFIPaymentStatusReportV11 GetReport()
        {
            return _document.FIToFIPmtStsRpt;
        }

        /// <summary>
        /// Resets the builder to its initial state, clearing all data.
        /// </summary>
        /// <returns>A new builder instance with cleared data.</returns>
        public Pacs00200111Builder Reset()
        {
            return new Pacs00200111Builder();
        }

        /// <summary>
        /// Creates a copy of the current builder with the same data.
        /// </summary>
        /// <returns>A new builder instance with the same data as the current builder.</returns>
        public Pacs00200111Builder Clone()
        {
            var clonedBuilder = new Pacs00200111Builder();

            // Copy group header
            if (_document.FIToFIPmtStsRpt.GrpHdr != null)
                clonedBuilder.WithGroupHeader(_document.FIToFIPmtStsRpt.GrpHdr);

            // Copy original group information and status
            if (_document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts != null)
            {
                foreach (var originalGroupInfo in _document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts)
                {
                    clonedBuilder.AddOriginalGroupInformationAndStatus(originalGroupInfo);
                }
            }

            // Copy transaction information and status
            if (_document.FIToFIPmtStsRpt.TxInfAndSts != null)
            {
                foreach (var transactionInfo in _document.FIToFIPmtStsRpt.TxInfAndSts)
                {
                    clonedBuilder.AddTransactionInformationAndStatus(transactionInfo);
                }
            }

            // Copy supplementary data
            if (_document.FIToFIPmtStsRpt.SplmtryData != null)
            {
                foreach (var data in _document.FIToFIPmtStsRpt.SplmtryData)
                {
                    clonedBuilder.AddSupplementaryData(data);
                }
            }

            return clonedBuilder;
        }

        /// <summary>
        /// Gets the current number of original group information entries in the message.
        /// </summary>
        /// <returns>The count of original group information entries.</returns>
        public int GetOriginalGroupInformationCount()
        {
            return _document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts?.Length ?? 0;
        }

        /// <summary>
        /// Gets the current number of transaction information entries in the message.
        /// </summary>
        /// <returns>The count of transaction information entries.</returns>
        public int GetTransactionInformationCount()
        {
            return _document.FIToFIPmtStsRpt.TxInfAndSts?.Length ?? 0;
        }

        /// <summary>
        /// Gets the current number of supplementary data entries in the message.
        /// </summary>
        /// <returns>The count of supplementary data entries.</returns>
        public int GetSupplementaryDataCount()
        {
            return _document.FIToFIPmtStsRpt.SplmtryData?.Length ?? 0;
        }

        /// <summary>
        /// Creates a new BranchAndFinancialInstitutionIdentification6 with the specified parameters.
        /// </summary>
        /// <param name="bic">Bank Identifier Code (BIC).</param>
        /// <param name="clearingSystemMemberIdentification">Clearing system member identification.</param>
        /// <param name="lei">Legal Entity Identifier (LEI).</param>
        /// <param name="name">Name of the financial institution.</param>
        /// <param name="postalAddress">Postal address of the financial institution.</param>
        /// <param name="otherIdentification">Other identification.</param>
        /// <returns>A configured BranchAndFinancialInstitutionIdentification6 instance.</returns>
        public static BranchAndFinancialInstitutionIdentification6 CreateFinancialInstitutionIdentification(
            string? bic = null,
            ClearingSystemMemberIdentification2? clearingSystemMemberIdentification = null,
            string? lei = null,
            string? name = null,
            PostalAddress24? postalAddress = null,
            GenericFinancialIdentification1? otherIdentification = null)
        {
            var finInstnId = new FinancialInstitutionIdentification18
            {
                BICFI = bic,
                ClrSysMmbId = clearingSystemMemberIdentification,
                LEI = lei,
                Nm = name,
                PstlAdr = postalAddress,
                Othr = otherIdentification
            };

            return new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = finInstnId
            };
        }

        /// <summary>
        /// Creates a new StatusReasonInformation12 with the specified parameters.
        /// </summary>
        /// <param name="originator">Originator of the status reason.</param>
        /// <param name="reason">Reason for the status.</param>
        /// <param name="additionalInformation">Additional information about the status.</param>
        /// <returns>A configured StatusReasonInformation12 instance.</returns>
        public static StatusReasonInformation12 CreateStatusReasonInformation(
            PartyIdentification135? originator = null,
            StatusReason6Choice? reason = null,
            string[]? additionalInformation = null)
        {
            return new StatusReasonInformation12
            {
                Orgtr = originator,
                Rsn = reason,
                AddtlInf = additionalInformation
            };
        }

        /// <summary>
        /// Creates a new NumberOfTransactionsPerStatus5 with the specified parameters.
        /// </summary>
        /// <param name="detailedNumberOfTransactions">Detailed number of transactions.</param>
        /// <param name="detailedStatus">Detailed status.</param>
        /// <param name="detailedControlSum">Detailed control sum.</param>
        /// <returns>A configured NumberOfTransactionsPerStatus5 instance.</returns>
        public static NumberOfTransactionsPerStatus5 CreateNumberOfTransactionsPerStatus(
            string detailedNumberOfTransactions,
            string detailedStatus,
            decimal? detailedControlSum = null)
        {
            var numberOfTransactions = new NumberOfTransactionsPerStatus5
            {
                DtldNbOfTxs = detailedNumberOfTransactions,
                DtldSts = detailedStatus
            };

            if (detailedControlSum.HasValue)
            {
                numberOfTransactions.DtldCtrlSum = detailedControlSum.Value;
                numberOfTransactions.DtldCtrlSumSpecified = true;
            }

            return numberOfTransactions;
        }

        /// <summary>
        /// Validates the current message structure to ensure it meets the required ISO 20022 standards.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
        private void ValidateMessage()
        {
            if (_document.FIToFIPmtStsRpt == null)
                throw new InvalidOperationException("FI to FI payment status report is not initialized.");

            if (_document.FIToFIPmtStsRpt.GrpHdr == null)
                throw new InvalidOperationException("Group header is required for the payment status report.");

            if (string.IsNullOrEmpty(_document.FIToFIPmtStsRpt.GrpHdr.MsgId))
                throw new InvalidOperationException("Message ID is required in the group header.");

            // Validate that at least one of the following is present:
            // - Original group information and status
            // - Transaction information and status
            bool hasOriginalGroupInfo = _document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts?.Any() == true;
            bool hasTransactionInfo = _document.FIToFIPmtStsRpt.TxInfAndSts?.Any() == true;

            if (!hasOriginalGroupInfo && !hasTransactionInfo)
                throw new InvalidOperationException("At least one original group information or transaction information entry is required.");
        }
    }
}
