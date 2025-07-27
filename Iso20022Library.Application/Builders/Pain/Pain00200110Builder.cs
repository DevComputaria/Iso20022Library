using System;
using System.Collections.ObjectModel;
using System.IO;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200110;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for creating Pain.002.001.10 (Customer Payment Status Report V10) messages.
    /// This message type is used by financial institutions to report the status of 
    /// previously received payment instructions. Version 10 includes the latest features
    /// and enhanced status reporting capabilities with improved data structures.
    /// </summary>
    public class Pain00200110Builder : IMessageBuilder
    {
        private readonly CustomerPaymentStatusReportV10 _report;

        /// <summary>
        /// Initializes a new instance of the Pain00200110Builder class.
        /// Sets up the basic structure with required components.
        /// </summary>
        public Pain00200110Builder()
        {
            _report = new CustomerPaymentStatusReportV10();
            InitializeGroupHeader();
            InitializeOriginalGroupInfo();
        }

        /// <summary>
        /// Sets the message identification and creation timestamp.
        /// This is required information that identifies the status report message.
        /// </summary>
        /// <param name="messageId">Unique identification for the status report message</param>
        /// <param name="creationDateTime">Date and time when the message was created (optional, defaults to current time)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder SetMessageIdentification(string messageId, DateTime? creationDateTime = null)
        {
            ValidateParameter(messageId, nameof(messageId));

            _report.GrpHdr.MsgId = messageId;
            _report.GrpHdr.CreDtTm = creationDateTime ?? DateTime.Now;

            return this;
        }

        /// <summary>
        /// Sets the initiating party information.
        /// This identifies the party that initiated the original payment instruction.
        /// </summary>
        /// <param name="initiatingPartyName">Name of the initiating party</param>
        /// <param name="bicfi">BIC of the initiating party (optional)</param>
        /// <param name="identification">Other identification of the initiating party (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder SetInitiatingParty(string initiatingPartyName, string? bicfi = null, string? identification = null)
        {
            ValidateParameter(initiatingPartyName, nameof(initiatingPartyName));

            _report.GrpHdr.InitgPty = new PartyIdentification135
            {
                Nm = initiatingPartyName
            };

            if (!string.IsNullOrEmpty(bicfi) || !string.IsNullOrEmpty(identification))
            {
                _report.GrpHdr.InitgPty.Id = new Party38Choice();

                if (!string.IsNullOrEmpty(bicfi))
                {
                    _report.GrpHdr.InitgPty.Id.OrgId = new OrganisationIdentification29
                    {
                        AnyBic = bicfi
                    };
                }

                if (!string.IsNullOrEmpty(identification))
                {
                    _report.GrpHdr.InitgPty.Id.OrgId ??= new OrganisationIdentification29();
                    _report.GrpHdr.InitgPty.Id.OrgId.Othr.Add(new GenericOrganisationIdentification1
                    {
                        Id = identification
                    });
                }
            }

            return this;
        }

        /// <summary>
        /// Sets the forwarding agent information.
        /// This identifies the financial institution that forwards the status report.
        /// </summary>
        /// <param name="forwardingAgentName">Name of the forwarding agent</param>
        /// <param name="bicfi">BIC of the forwarding agent</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder SetForwardingAgent(string forwardingAgentName, string bicfi)
        {
            ValidateParameter(forwardingAgentName, nameof(forwardingAgentName));
            ValidateParameter(bicfi, nameof(bicfi));

            _report.GrpHdr.FwdgAgt = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18
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
        /// <param name="originalMessageId">Original message identification</param>
        /// <param name="originalMessageNameId">Original message name identification</param>
        /// <param name="originalCreationDateTime">Original creation date and time (optional)</param>
        /// <param name="groupStatus">Status of the original group (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder SetOriginalGroupInformation(
            string originalMessageId,
            string originalMessageNameId,
            DateTime? originalCreationDateTime = null,
            string? groupStatus = null)
        {
            ValidateParameter(originalMessageId, nameof(originalMessageId));
            ValidateParameter(originalMessageNameId, nameof(originalMessageNameId));

            _report.OrgnlGrpInfAndSts.OrgnlMsgId = originalMessageId;
            _report.OrgnlGrpInfAndSts.OrgnlMsgNmId = originalMessageNameId;

            if (originalCreationDateTime.HasValue)
            {
                _report.OrgnlGrpInfAndSts.OrgnlCreDtTm = originalCreationDateTime.Value;
                _report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified = true;
            }

            if (!string.IsNullOrEmpty(groupStatus))
            {
                _report.OrgnlGrpInfAndSts.GrpSts = groupStatus;
            }

            return this;
        }

        /// <summary>
        /// Adds status reason information to the original group information.
        /// This provides detailed information about why the group has a particular status.
        /// </summary>
        /// <param name="reasonCode">Code indicating the reason for the status</param>
        /// <param name="additionalInformation">Additional textual information about the status (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder AddGroupStatusReason(string reasonCode, string? additionalInformation = null)
        {
            ValidateParameter(reasonCode, nameof(reasonCode));

            var statusReason = new StatusReasonInformation12
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

            _report.OrgnlGrpInfAndSts.StsRsnInf.Add(statusReason);

            return this;
        }

        /// <summary>
        /// Adds an original payment instruction with its status information.
        /// This allows detailed instruction-level status reporting.
        /// </summary>
        /// <param name="originalInstructionId">Original instruction identification</param>
        /// <param name="paymentStatus">Status of the payment instruction</param>
        /// <param name="numberOfTransactions">Number of transactions in the instruction (optional)</param>
        /// <param name="controlSum">Total amount of the instruction (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder AddOriginalPaymentInstruction(
            string originalInstructionId,
            string paymentStatus,
            string? numberOfTransactions = null,
            decimal? controlSum = null)
        {
            ValidateParameter(originalInstructionId, nameof(originalInstructionId));
            ValidateParameter(paymentStatus, nameof(paymentStatus));

            var paymentInstruction = new OriginalPaymentInstruction32
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
        /// Adds status reason information to the last added payment instruction.
        /// This provides detailed information about why the payment instruction has a particular status.
        /// </summary>
        /// <param name="reasonCode">Code indicating the reason for the status</param>
        /// <param name="additionalInformation">Additional textual information about the status (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder AddPaymentInstructionStatusReason(string reasonCode, string? additionalInformation = null)
        {
            ValidateParameter(reasonCode, nameof(reasonCode));

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("No payment instruction has been added. Use AddOriginalPaymentInstruction first.");
            }

            var lastInstruction = _report.OrgnlPmtInfAndSts[_report.OrgnlPmtInfAndSts.Count - 1];

            var statusReason = new StatusReasonInformation12
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

            lastInstruction.StsRsnInf.Add(statusReason);

            return this;
        }

        /// <summary>
        /// Adds a payment transaction with its status to the last added payment instruction.
        /// This method allows detailed transaction-level status reporting.
        /// </summary>
        /// <param name="originalEndToEndId">Original end-to-end identification of the transaction</param>
        /// <param name="transactionStatus">Status of the individual transaction</param>
        /// <param name="statusId">Status identification (optional)</param>
        /// <param name="originalInstructionId">Original instruction identification (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder AddPaymentTransaction(
            string originalEndToEndId,
            string transactionStatus,
            string? statusId = null,
            string? originalInstructionId = null)
        {
            ValidateParameter(originalEndToEndId, nameof(originalEndToEndId));
            ValidateParameter(transactionStatus, nameof(transactionStatus));

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("No payment instruction has been added. Use AddOriginalPaymentInstruction first.");
            }

            var lastInstruction = _report.OrgnlPmtInfAndSts[_report.OrgnlPmtInfAndSts.Count - 1];

            var transaction = new PaymentTransaction105
            {
                StsId = statusId,
                OrgnlEndToEndId = originalEndToEndId,
                TxSts = transactionStatus
            };

            if (!string.IsNullOrEmpty(originalInstructionId))
            {
                transaction.OrgnlInstrId = originalInstructionId;
            }

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
        public Pain00200110Builder AddTransactionStatusReason(string reasonCode, string? additionalInformation = null)
        {
            ValidateParameter(reasonCode, nameof(reasonCode));

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("No payment instruction has been added. Use AddOriginalPaymentInstruction first.");
            }

            var lastInstruction = _report.OrgnlPmtInfAndSts[_report.OrgnlPmtInfAndSts.Count - 1];

            if (lastInstruction.TxInfAndSts == null || lastInstruction.TxInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("No payment transaction has been added. Use AddPaymentTransaction first.");
            }

            var lastTransaction = lastInstruction.TxInfAndSts[lastInstruction.TxInfAndSts.Count - 1];

            var statusReason = new StatusReasonInformation12
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

            lastTransaction.StsRsnInf.Add(statusReason);

            return this;
        }

        /// <summary>
        /// Sets charges information for the last added payment transaction.
        /// This provides details about charges applied to the transaction.
        /// </summary>
        /// <param name="chargeAmount">Amount of the charge</param>
        /// <param name="chargeCurrency">Currency of the charge</param>
        /// <param name="agentName">Name of the charging agent</param>
        /// <param name="agentBic">BIC of the charging agent (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder SetTransactionCharges(decimal chargeAmount, string chargeCurrency, string agentName, string? agentBic = null)
        {
            ValidateParameter(chargeCurrency, nameof(chargeCurrency));
            ValidateParameter(agentName, nameof(agentName));

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("No payment instruction has been added. Use AddOriginalPaymentInstruction first.");
            }

            var lastInstruction = _report.OrgnlPmtInfAndSts[_report.OrgnlPmtInfAndSts.Count - 1];

            if (lastInstruction.TxInfAndSts == null || lastInstruction.TxInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("No payment transaction has been added. Use AddPaymentTransaction first.");
            }

            var lastTransaction = lastInstruction.TxInfAndSts[lastInstruction.TxInfAndSts.Count - 1];

            var charge = new Charges7
            {
                Amt = new ActiveOrHistoricCurrencyAndAmount
                {
                    Ccy = chargeCurrency,
                    Value = chargeAmount
                },
                Agt = new BranchAndFinancialInstitutionIdentification6
                {
                    FinInstnId = new FinancialInstitutionIdentification18
                    {
                        Nm = agentName
                    }
                }
            };

            if (!string.IsNullOrEmpty(agentBic))
            {
                charge.Agt.FinInstnId.Bicfi = agentBic;
            }

            lastTransaction.ChrgsInf.Add(charge);

            return this;
        }

        /// <summary>
        /// Adds supplementary data to the status report.
        /// This allows additional information to be included that is not covered by the standard message structure.
        /// </summary>
        /// <param name="data">Supplementary data to add</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pain00200110Builder AddSupplementaryData(SupplementaryData1 data)
        {
            ValidateParameter(data, nameof(data));

            _report.SplmtryData.Add(data);

            return this;
        }

        /// <summary>
        /// Builds and returns the complete Pain.002.001.10 message.
        /// Performs validation to ensure all required fields are populated.
        /// </summary>
        /// <returns>The complete CustomerPaymentStatusReportV10 message</returns>
        public CustomerPaymentStatusReportV10 Build()
        {
            ValidateMessage();
            return _report;
        }

        /// <summary>
        /// Builds the complete message and serializes it to XML.
        /// </summary>
        /// <returns>XML representation of the Pain.002.001.10 message</returns>
        public string BuildXml()
        {
            var message = Build();
            return XmlSerializationService.Serialize(message);
        }

        /// <summary>
        /// Builds the complete message from a generic object and serializes it to XML.
        /// This method implements the IMessageBuilder interface.
        /// </summary>
        /// <param name="message">The message object (must be CustomerPaymentStatusReportV10)</param>
        /// <returns>XML representation of the Pain.002.001.10 message</returns>
        public string BuildXml(object message)
        {
            if (message is not CustomerPaymentStatusReportV10 report)
            {
                throw new ArgumentException("Message must be of type CustomerPaymentStatusReportV10", nameof(message));
            }

            return XmlSerializationService.Serialize(report);
        }

        /// <summary>
        /// Saves the message as an XML file.
        /// </summary>
        /// <param name="filePath">Path where the XML file should be saved</param>
        public void SaveToFile(string filePath)
        {
            ValidateParameter(filePath, nameof(filePath));

            var xml = BuildXml();
            File.WriteAllText(filePath, xml);
        }

        /// <summary>
        /// Initializes the group header with required basic structure.
        /// </summary>
        private void InitializeGroupHeader()
        {
            _report.GrpHdr = new GroupHeader86
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
            _report.OrgnlGrpInfAndSts = new OriginalGroupHeader17
            {
                OrgnlMsgId = string.Empty,
                OrgnlMsgNmId = string.Empty
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

            if (string.IsNullOrEmpty(_report.OrgnlGrpInfAndSts?.OrgnlMsgNmId))
            {
                throw new InvalidOperationException("Original message name ID is required. Use SetOriginalGroupInformation method.");
            }

            if (_report.OrgnlPmtInfAndSts.Count == 0)
            {
                throw new InvalidOperationException("At least one original payment instruction is required. Use AddOriginalPaymentInstruction method.");
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

            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                throw new ArgumentException("Parameter cannot be null or empty.", parameterName);
            }
        }
    }
}
