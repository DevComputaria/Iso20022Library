using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200108;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for creating Pain.002.001.08 (Customer Payment Status Report V08) messages.
    /// This message type is used by financial institutions to report the status of 
    /// previously received payment instructions.
    /// </summary>
    public class Pain00200108Builder : IMessageBuilder
    {
        private readonly CustomerPaymentStatusReportV08 _report;

        /// <summary>
        /// Initializes a new instance of the Pain00200108Builder class.
        /// Sets up the basic structure with required components.
        /// </summary>
        public Pain00200108Builder()
        {
            _report = new CustomerPaymentStatusReportV08();
            InitializeGroupHeader();
            InitializeOriginalGroupInfo();
        }

        /// <summary>
        /// Sets the message identification and creation date time for the group header.
        /// </summary>
        /// <param name="messageId">Unique identification for the message</param>
        /// <param name="creationDateTime">Date and time when the message was created</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder SetMessageIdentification(string messageId, DateTime creationDateTime)
        {
            ValidateParameter(messageId, "messageId");

            _report.GrpHdr.MsgId = messageId;
            _report.GrpHdr.CreDtTm = creationDateTime;

            return this;
        }

        /// <summary>
        /// Sets the initiating party information for the message.
        /// This identifies the party that initiated the original payment instruction.
        /// </summary>
        /// <param name="initiatingPartyName">Name of the initiating party</param>
        /// <param name="bicfi">BIC (Bank Identifier Code) of the initiating party's financial institution</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder SetInitiatingParty(string initiatingPartyName, string? bicfi = null)
        {
            ValidateParameter(initiatingPartyName, "initiatingPartyName");

            _report.GrpHdr.InitgPty = new PartyIdentification43
            {
                Nm = initiatingPartyName
            };

            if (!string.IsNullOrEmpty(bicfi))
            {
                _report.GrpHdr.InitgPty.Id = new Party11Choice
                {
                    OrgId = new OrganisationIdentification8
                    {
                        AnyBic = bicfi
                    }
                };
            }

            return this;
        }

        /// <summary>
        /// Sets the forwarding agent information for the message.
        /// This identifies the financial institution forwarding the status report.
        /// </summary>
        /// <param name="forwardingAgentName">Name of the forwarding agent</param>
        /// <param name="bicfi">BIC of the forwarding agent</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder SetForwardingAgent(string forwardingAgentName, string bicfi)
        {
            ValidateParameter(forwardingAgentName, "forwardingAgentName");
            ValidateParameter(bicfi, "bicfi");

            _report.GrpHdr.FwdgAgt = new BranchAndFinancialInstitutionIdentification5
            {
                FinInstnId = new FinancialInstitutionIdentification8
                {
                    Bicfi = bicfi,
                    Nm = forwardingAgentName
                }
            };

            return this;
        }

        /// <summary>
        /// Sets the original group information and status for the status report.
        /// This references the original payment instruction group that this status report relates to.
        /// </summary>
        /// <param name="originalMessageId">Message ID of the original payment instruction</param>
        /// <param name="originalCreationDateTime">Creation date/time of the original payment instruction</param>
        /// <param name="numberOfTransactions">Number of transactions in the original group</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder SetOriginalGroupInformation(
            string originalMessageId,
            DateTime originalCreationDateTime,
            string? numberOfTransactions = null)
        {
            ValidateParameter(originalMessageId, "originalMessageId");

            _report.OrgnlGrpInfAndSts.OrgnlMsgId = originalMessageId;
            _report.OrgnlGrpInfAndSts.OrgnlCreDtTm = originalCreationDateTime;
            _report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified = true;

            if (!string.IsNullOrEmpty(numberOfTransactions))
            {
                _report.OrgnlGrpInfAndSts.OrgnlNbOfTxs = numberOfTransactions;
            }

            return this;
        }

        /// <summary>
        /// Sets the original group status using a string value.
        /// In Pain.002.001.08, status is represented as a string rather than an enum.
        /// </summary>
        /// <param name="groupStatus">Status of the original payment group (e.g., "ACTC", "RJCT", "PDNG")</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder SetOriginalGroupStatus(string groupStatus)
        {
            ValidateParameter(groupStatus, "groupStatus");

            _report.OrgnlGrpInfAndSts.GrpSts = groupStatus;

            return this;
        }

        /// <summary>
        /// Adds an original payment instruction with its status information.
        /// This method allows building multiple payment instruction statuses within the report.
        /// </summary>
        /// <param name="originalInstructionId">ID of the original payment instruction</param>
        /// <param name="paymentStatus">Status of the payment instruction</param>
        /// <param name="numberOfTransactions">Number of transactions in this payment instruction</param>
        /// <param name="controlSum">Control sum for validation (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder AddOriginalPaymentInstruction(
            string originalInstructionId,
            string paymentStatus,
            string? numberOfTransactions = null,
            decimal? controlSum = null)
        {
            ValidateParameter(originalInstructionId, "originalInstructionId");
            ValidateParameter(paymentStatus, "paymentStatus");

            var paymentInstruction = new OriginalPaymentInstruction23
            {
                OrgnlPmtInfId = originalInstructionId,
                PmtInfSts = paymentStatus
            };

            if (!string.IsNullOrEmpty(numberOfTransactions))
            {
                paymentInstruction.OrgnlNbOfTxs = numberOfTransactions;
            }

            if (controlSum.HasValue)
            {
                paymentInstruction.OrgnlCtrlSum = controlSum.Value;
                paymentInstruction.OrgnlCtrlSumSpecified = true;
            }

            _report.OrgnlPmtInfAndSts.Add(paymentInstruction);

            return this;
        }

        /// <summary>
        /// Adds a payment transaction with its status to the last added payment instruction.
        /// This method allows detailed transaction-level status reporting.
        /// </summary>
        /// <param name="originalEndToEndId">Original end-to-end identification of the transaction</param>
        /// <param name="transactionStatus">Status of the individual transaction</param>
        /// <param name="originalTransactionId">Original transaction identification (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder AddPaymentTransaction(
            string originalEndToEndId,
            string transactionStatus,
            string? originalTransactionId = null)
        {
            ValidateParameter(originalEndToEndId, "originalEndToEndId");
            ValidateParameter(transactionStatus, "transactionStatus");

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("Add an original payment instruction before adding transactions.");
            }

            var transaction = new PaymentTransaction82
            {
                OrgnlEndToEndId = originalEndToEndId,
                TxSts = transactionStatus
            };

            if (!string.IsNullOrEmpty(originalTransactionId))
            {
                transaction.OrgnlInstrId = originalTransactionId;
            }

            var lastInstruction = _report.OrgnlPmtInfAndSts[_report.OrgnlPmtInfAndSts.Count - 1];
            lastInstruction.TxInfAndSts.Add(transaction);

            return this;
        }

        /// <summary>
        /// Adds status reason information to the last added payment transaction.
        /// This provides detailed information about why a transaction has a particular status.
        /// </summary>
        /// <param name="reasonCode">Code indicating the reason for the status</param>
        /// <param name="additionalInformation">Additional textual information about the status (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder AddTransactionStatusReason(string reasonCode, string? additionalInformation = null)
        {
            ValidateParameter(reasonCode, "reasonCode");

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("Add an original payment instruction before adding status reasons.");
            }

            var lastInstruction = _report.OrgnlPmtInfAndSts[_report.OrgnlPmtInfAndSts.Count - 1];
            if (lastInstruction.TxInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("Add a payment transaction before adding status reasons.");
            }

            var statusReason = new StatusReasonInformation9
            {
                Rsn = new StatusReason6Choice
                {
                    Cd = reasonCode
                }
            };

            if (!string.IsNullOrEmpty(additionalInformation))
            {
                statusReason.AddtlInf.Add(additionalInformation);
            }

            var lastTransaction = lastInstruction.TxInfAndSts[lastInstruction.TxInfAndSts.Count - 1];
            lastTransaction.StsRsnInf.Add(statusReason);

            return this;
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// This allows inclusion of additional information not covered by the standard message structure.
        /// </summary>
        /// <param name="data">The supplementary data to add</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200108Builder AddSupplementaryData(SupplementaryData1 data)
        {
            ValidateParameter(data, "data");

            _report.SplmtryData.Add(data);

            return this;
        }

        /// <summary>
        /// Builds and returns the complete Pain.002.001.08 message.
        /// Performs validation to ensure all required fields are populated.
        /// </summary>
        /// <returns>The complete CustomerPaymentStatusReportV08 message</returns>
        public CustomerPaymentStatusReportV08 Build()
        {
            ValidateMessage();
            return _report;
        }

        /// <summary>
        /// Builds the complete message and serializes it to XML.
        /// </summary>
        /// <returns>XML representation of the Pain.002.001.08 message</returns>
        public string BuildXml()
        {
            var message = Build();
            var document = new Document { CstmrPmtStsRpt = message };
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Serializes the provided document to XML.
        /// </summary>
        /// <param name="message">The document to serialize. Must be an instance of Document.</param>
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
        /// Saves the message as an XML file.
        /// </summary>
        /// <param name="filePath">Path where the XML file should be saved</param>
        public void SaveToFile(string filePath)
        {
            ValidateParameter(filePath, "filePath");

            var xml = BuildXml();
            File.WriteAllText(filePath, xml);
        }

        /// <summary>
        /// Initializes the group header with required basic structure.
        /// </summary>
        private void InitializeGroupHeader()
        {
            _report.GrpHdr = new GroupHeader52
            {
                MsgId = string.Empty,
                CreDtTm = DateTime.Now
            };
        }

        /// <summary>
        /// Initializes the original group information with required basic structure.
        /// </summary>
        private void InitializeOriginalGroupInfo()
        {
            _report.OrgnlGrpInfAndSts = new OriginalGroupHeader7
            {
                OrgnlMsgId = string.Empty
            };
        }

        /// <summary>
        /// Validates that the message has all required information before building.
        /// </summary>
        private void ValidateMessage()
        {
            if (string.IsNullOrEmpty(_report.GrpHdr?.MsgId))
            {
                throw new InvalidOperationException("Message ID is required. Use SetMessageIdentification method.");
            }

            if (string.IsNullOrEmpty(_report.OrgnlGrpInfAndSts?.OrgnlMsgId))
            {
                throw new InvalidOperationException("Original message ID is required. Use SetOriginalGroupInformation method.");
            }

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("At least one original payment instruction is required. Use AddOriginalPaymentInstruction method.");
            }
        }

        /// <summary>
        /// Validates that a parameter is not null or empty.
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="parameterName">Name of the parameter for error reporting</param>
        private static void ValidateParameter(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
            }
        }

        /// <summary>
        /// Validates that a parameter is not null.
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="parameterName">Name of the parameter for error reporting</param>
        private static void ValidateParameter(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}