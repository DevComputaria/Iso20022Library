using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00400110;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;

namespace Iso20022Library.Application.Builders.Pacs
{
    /// <summary>
    /// Builder for creating PACS.004.001.10 (Payment Return V10) messages.
    /// This message is used by financial institutions to return a payment instruction.
    /// </summary>
    public class Pacs00400110Builder : IMessageBuilder
    {
        private readonly Document _document;
        private readonly List<PaymentTransaction118> _paymentTransactions;
        private readonly List<SupplementaryData1> _supplementaryData;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pacs00400110Builder"/> class.
        /// </summary>
        public Pacs00400110Builder()
        {
            _document = new Document
            {
                PmtRtr = new PaymentReturnV10()
            };
            _paymentTransactions = new List<PaymentTransaction118>();
            _supplementaryData = new List<SupplementaryData1>();
        }

        /// <summary>
        /// Sets the group header for the payment return.
        /// </summary>
        /// <param name="messageId">Unique identification for the message.</param>
        /// <param name="creationDateTime">Date and time when the message was created.</param>
        /// <param name="numberOfTransactions">Number of individual transactions in the message.</param>
        /// <param name="controlSum">Total amount of all individual transactions (optional).</param>
        /// <param name="groupReturn">Indicates if the entire group is being returned (optional).</param>
        /// <param name="totalReturnedInterBankSettlementAmount">Total returned interbank settlement amount (optional).</param>
        /// <param name="interBankSettlementDate">Interbank settlement date (optional).</param>
        /// <param name="batchBooking">Batch booking indicator (optional).</param>
        /// <param name="instructingAgent">Instructing agent financial institution (optional).</param>
        /// <param name="instructedAgent">Instructed agent financial institution (optional).</param>
        /// <param name="settlementInformation">Settlement method information (optional).</param>
        /// <param name="authorization">Authorization information (optional).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when required parameters are null or empty.</exception>
        public Pacs00400110Builder WithGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            bool? groupReturn = null,
            ActiveCurrencyAndAmount? totalReturnedInterBankSettlementAmount = null,
            DateTime? interBankSettlementDate = null,
            bool? batchBooking = null,
            BranchAndFinancialInstitutionIdentification6? instructingAgent = null,
            BranchAndFinancialInstitutionIdentification6? instructedAgent = null,
            SettlementInstruction7? settlementInformation = null,
            Authorisation1Choice[]? authorization = null)
        {
            if (string.IsNullOrEmpty(messageId))
                throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));
            if (string.IsNullOrEmpty(numberOfTransactions))
                throw new ArgumentException("Number of transactions cannot be null or empty.", nameof(numberOfTransactions));

            _document.PmtRtr.GrpHdr = new GroupHeader90
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                CtrlSum = controlSum ?? 0,
                CtrlSumSpecified = controlSum.HasValue,
                GrpRtr = groupReturn ?? false,
                GrpRtrSpecified = groupReturn.HasValue,
                TtlRtrdIntrBkSttlmAmt = totalReturnedInterBankSettlementAmount,
                IntrBkSttlmDt = interBankSettlementDate ?? DateTime.MinValue,
                IntrBkSttlmDtSpecified = interBankSettlementDate.HasValue,
                BtchBookg = batchBooking ?? false,
                BtchBookgSpecified = batchBooking.HasValue,
                InstgAgt = instructingAgent,
                InstdAgt = instructedAgent,
                SttlmInf = settlementInformation,
                Authstn = authorization
            };

            return this;
        }

        /// <summary>
        /// Sets the original group information for the payment return.
        /// </summary>
        /// <param name="originalMessageId">Original message identification.</param>
        /// <param name="originalMessageNameId">Original message name identification.</param>
        /// <param name="originalCreationDateTime">Original creation date and time (optional).</param>
        /// <param name="returnReasonInformation">Return reason information (optional).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when required parameters are null or empty.</exception>
        public Pacs00400110Builder WithOriginalGroupInformation(
            string originalMessageId,
            string originalMessageNameId,
            DateTime? originalCreationDateTime = null,
            PaymentReturnReason6[]? returnReasonInformation = null)
        {
            if (string.IsNullOrEmpty(originalMessageId))
                throw new ArgumentException("Original message ID cannot be null or empty.", nameof(originalMessageId));
            if (string.IsNullOrEmpty(originalMessageNameId))
                throw new ArgumentException("Original message name ID cannot be null or empty.", nameof(originalMessageNameId));

            _document.PmtRtr.OrgnlGrpInf = new OriginalGroupHeader18
            {
                OrgnlMsgId = originalMessageId,
                OrgnlMsgNmId = originalMessageNameId,
                OrgnlCreDtTm = originalCreationDateTime ?? DateTime.MinValue,
                OrgnlCreDtTmSpecified = originalCreationDateTime.HasValue,
                RtrRsnInf = returnReasonInformation
            };

            return this;
        }

        /// <summary>
        /// Adds a payment transaction with return information.
        /// </summary>
        /// <param name="returnId">Unique identification for the return transaction.</param>
        /// <param name="originalGroupInformation">Original group information (optional).</param>
        /// <param name="originalInstructionId">Original instruction identification (optional).</param>
        /// <param name="originalEndToEndId">Original end-to-end identification (optional).</param>
        /// <param name="originalTransactionId">Original transaction identification (optional).</param>
        /// <param name="originalUETR">Original Unique End-to-end Transaction Reference (optional).</param>
        /// <param name="originalClearingSystemReference">Original clearing system reference (optional).</param>
        /// <param name="originalInterBankSettlementAmount">Original interbank settlement amount (optional).</param>
        /// <param name="originalInterBankSettlementDate">Original interbank settlement date (optional).</param>
        /// <param name="returnedInterBankSettlementAmount">Returned interbank settlement amount (optional).</param>
        /// <param name="interBankSettlementDate">Interbank settlement date (optional).</param>
        /// <param name="returnedInstructedAmount">Returned instructed amount (optional).</param>
        /// <param name="exchangeRate">Exchange rate (optional).</param>
        /// <param name="compensationAmount">Compensation amount (optional).</param>
        /// <param name="reasonInformation">Return reason information (optional).</param>
        /// <param name="originalTransactionReference">Original transaction reference (optional).</param>
        /// <param name="supplementaryData">Supplementary data (optional).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when required parameters are null or empty.</exception>
        public Pacs00400110Builder AddPaymentTransaction(
            string returnId,
            OriginalGroupInformation29? originalGroupInformation = null,
            string? originalInstructionId = null,
            string? originalEndToEndId = null,
            string? originalTransactionId = null,
            string? originalUETR = null,
            string? originalClearingSystemReference = null,
            ActiveOrHistoricCurrencyAndAmount? originalInterBankSettlementAmount = null,
            DateTime? originalInterBankSettlementDate = null,
            ActiveCurrencyAndAmount? returnedInterBankSettlementAmount = null,
            DateTime? interBankSettlementDate = null,
            ActiveOrHistoricCurrencyAndAmount? returnedInstructedAmount = null,
            decimal? exchangeRate = null,
            ActiveOrHistoricCurrencyAndAmount? compensationAmount = null,
            PaymentReturnReason6[]? reasonInformation = null,
            OriginalTransactionReference32? originalTransactionReference = null,
            SupplementaryData1[]? supplementaryData = null)
        {
            if (string.IsNullOrEmpty(returnId))
                throw new ArgumentException("Return ID cannot be null or empty.", nameof(returnId));

            var transaction = new PaymentTransaction118
            {
                RtrId = returnId,
                OrgnlGrpInf = originalGroupInformation,
                OrgnlInstrId = originalInstructionId,
                OrgnlEndToEndId = originalEndToEndId,
                OrgnlTxId = originalTransactionId,
                OrgnlUETR = originalUETR,
                OrgnlClrSysRef = originalClearingSystemReference,
                OrgnlIntrBkSttlmAmt = originalInterBankSettlementAmount,
                OrgnlIntrBkSttlmDt = originalInterBankSettlementDate ?? DateTime.MinValue,
                OrgnlIntrBkSttlmDtSpecified = originalInterBankSettlementDate.HasValue,
                RtrdIntrBkSttlmAmt = returnedInterBankSettlementAmount,
                IntrBkSttlmDt = interBankSettlementDate ?? DateTime.MinValue,
                IntrBkSttlmDtSpecified = interBankSettlementDate.HasValue,
                RtrdInstdAmt = returnedInstructedAmount,
                XchgRate = exchangeRate ?? 0,
                XchgRateSpecified = exchangeRate.HasValue,
                CompstnAmt = compensationAmount,
                RtrRsnInf = reasonInformation,
                OrgnlTxRef = originalTransactionReference,
                SplmtryData = supplementaryData
            };

            _paymentTransactions.Add(transaction);
            return this;
        }

        /// <summary>
        /// Creates return reason information.
        /// </summary>
        /// <param name="originator">Party that issues the return (optional).</param>
        /// <param name="returnReasonCode">Return reason code (optional).</param>
        /// <param name="additionalReturnReasonInformation">Additional return reason information (optional).</param>
        /// <returns>A new PaymentReturnReason6 instance.</returns>
        public static PaymentReturnReason6 CreateReturnReasonInformation(
            PartyIdentification135? originator = null,
            ReturnReason5Choice? returnReasonCode = null,
            string[]? additionalReturnReasonInformation = null)
        {
            return new PaymentReturnReason6
            {
                Orgtr = originator,
                Rsn = returnReasonCode,
                AddtlInf = additionalReturnReasonInformation
            };
        }

        /// <summary>
        /// Creates a financial institution identification.
        /// </summary>
        /// <param name="bic">Bank Identifier Code (BIC).</param>
        /// <param name="clearingSystemMemberId">Clearing system member identification (optional).</param>
        /// <param name="lei">Legal Entity Identifier (optional).</param>
        /// <param name="name">Name of the financial institution (optional).</param>
        /// <param name="postalAddress">Postal address (optional).</param>
        /// <param name="other">Other identification (optional).</param>
        /// <returns>A new BranchAndFinancialInstitutionIdentification6 instance.</returns>
        /// <exception cref="ArgumentException">Thrown when BIC is null or empty.</exception>
        public static BranchAndFinancialInstitutionIdentification6 CreateFinancialInstitution(
            string bic,
            ClearingSystemMemberIdentification2? clearingSystemMemberId = null,
            string? lei = null,
            string? name = null,
            PostalAddress24? postalAddress = null,
            GenericFinancialIdentification1? other = null)
        {
            if (string.IsNullOrEmpty(bic))
                throw new ArgumentException("BIC cannot be null or empty.", nameof(bic));

            return new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18
                {
                    BICFI = bic,
                    ClrSysMmbId = clearingSystemMemberId,
                    LEI = lei,
                    Nm = name,
                    PstlAdr = postalAddress,
                    Othr = other
                }
            };
        }

        /// <summary>
        /// Creates settlement information.
        /// </summary>
        /// <param name="settlementMethod">Settlement method.</param>
        /// <param name="settlementAccount">Settlement account (optional).</param>
        /// <param name="clearingSystem">Clearing system (optional).</param>
        /// <returns>A new SettlementInstruction7 instance.</returns>
        /// <exception cref="ArgumentException">Thrown when settlement method is null.</exception>
        public static SettlementInstruction7 CreateSettlementInformation(
            SettlementMethod1Code settlementMethod,
            CashAccount38? settlementAccount = null,
            ClearingSystemIdentification3Choice? clearingSystem = null)
        {
            return new SettlementInstruction7
            {
                SttlmMtd = settlementMethod,
                SttlmAcct = settlementAccount,
                ClrSys = clearingSystem
            };
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="envelopeContent">The supplementary data content.</param>
        /// <param name="placedAs">Where the supplementary data is placed (optional).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when envelope content is null.</exception>
        public Pacs00400110Builder AddSupplementaryData(object envelopeContent, string? placedAs = null)
        {
            if (envelopeContent == null)
                throw new ArgumentNullException(nameof(envelopeContent));

            var doc = new XmlDocument();
            var element = doc.CreateElement("Envelope");
            element.InnerText = envelopeContent.ToString() ?? string.Empty;

            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = placedAs,
                Envlp = element
            };

            _supplementaryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Builds the PaymentReturnV10 document.
        /// </summary>
        /// <returns>The constructed PaymentReturnV10 document.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
        public Document Build()
        {
            ValidateMessage();

            // Set the payment transactions
            _document.PmtRtr.TxInf = _paymentTransactions.ToArray();

            // Set supplementary data if any
            if (_supplementaryData.Any())
            {
                _document.PmtRtr.SplmtryData = _supplementaryData.ToArray();
            }

            return _document;
        }

        /// <summary>
        /// Builds the XML representation of the payment return message.
        /// </summary>
        /// <param name="message">The message object (not used, the builder creates its own document).</param>
        /// <returns>The XML string representation of the PACS.004.001.10 message.</returns>
        public string BuildXml(object message)
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Creates a copy of the current builder.
        /// </summary>
        /// <returns>A new Pacs00400110Builder instance with the same configuration.</returns>
        public Pacs00400110Builder Clone()
        {
            var clonedBuilder = new Pacs00400110Builder();

            // Clone the document structure
            if (_document.PmtRtr.GrpHdr != null)
            {
                var originalHeader = _document.PmtRtr.GrpHdr;
                clonedBuilder.WithGroupHeader(
                    originalHeader.MsgId,
                    originalHeader.CreDtTm,
                    originalHeader.NbOfTxs,
                    originalHeader.CtrlSumSpecified ? originalHeader.CtrlSum : null,
                    originalHeader.GrpRtrSpecified ? originalHeader.GrpRtr : null,
                    originalHeader.TtlRtrdIntrBkSttlmAmt,
                    originalHeader.IntrBkSttlmDtSpecified ? originalHeader.IntrBkSttlmDt : null,
                    originalHeader.BtchBookgSpecified ? originalHeader.BtchBookg : null,
                    originalHeader.InstgAgt,
                    originalHeader.InstdAgt,
                    originalHeader.SttlmInf,
                    originalHeader.Authstn);
            }

            // Clone original group information
            if (_document.PmtRtr.OrgnlGrpInf != null)
            {
                var originalGrpInf = _document.PmtRtr.OrgnlGrpInf;
                clonedBuilder.WithOriginalGroupInformation(
                    originalGrpInf.OrgnlMsgId,
                    originalGrpInf.OrgnlMsgNmId,
                    originalGrpInf.OrgnlCreDtTmSpecified ? originalGrpInf.OrgnlCreDtTm : null,
                    originalGrpInf.RtrRsnInf);
            }

            // Clone transactions
            foreach (var transaction in _paymentTransactions)
            {
                clonedBuilder._paymentTransactions.Add(transaction);
            }

            // Clone supplementary data
            foreach (var data in _supplementaryData)
            {
                clonedBuilder._supplementaryData.Add(data);
            }

            return clonedBuilder;
        }

        /// <summary>
        /// Validates the message structure before XML generation.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the message structure is invalid.</exception>
        private void ValidateMessage()
        {
            if (_document.PmtRtr.GrpHdr == null)
                throw new InvalidOperationException("Group header is required for PACS.004.001.10 message.");

            if (string.IsNullOrEmpty(_document.PmtRtr.GrpHdr.MsgId))
                throw new InvalidOperationException("Message ID is required in the group header.");

            if (string.IsNullOrEmpty(_document.PmtRtr.GrpHdr.NbOfTxs))
                throw new InvalidOperationException("Number of transactions is required in the group header.");

            if (!_paymentTransactions.Any())
                throw new InvalidOperationException("At least one payment transaction is required.");

            // Validate that the number of transactions matches the actual count
            if (int.TryParse(_document.PmtRtr.GrpHdr.NbOfTxs, out int declaredCount) &&
                declaredCount != _paymentTransactions.Count)
            {
                throw new InvalidOperationException(
                    $"Declared number of transactions ({declaredCount}) does not match actual count ({_paymentTransactions.Count}).");
            }

            // Validate control sum if specified
            if (_document.PmtRtr.GrpHdr.CtrlSumSpecified)
            {
                var totalAmount = _paymentTransactions
                    .Where(t => t.RtrdIntrBkSttlmAmt != null)
                    .Sum(t => t.RtrdIntrBkSttlmAmt!.Value);

                if (Math.Abs(_document.PmtRtr.GrpHdr.CtrlSum - totalAmount) > 0.01m)
                {
                    throw new InvalidOperationException(
                        $"Control sum ({_document.PmtRtr.GrpHdr.CtrlSum}) does not match total transaction amounts ({totalAmount}).");
                }
            }
        }
    }
}
