using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Application.Xml;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700103;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.007.001.03 messages (Customer Payment Reversal V03).
    /// </summary>
    /// <remarks>
    /// The pain.007.001.03 message is used to request the reversal of one or more payment instructions previously sent.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards. The message contains information about the original payment instructions
    /// to be reversed and the reasons for the reversal.
    /// </remarks>
    public class Pain00700103Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the Pain00700103Builder class.
        /// </summary>
        public Pain00700103Builder()
        {
            _document = new Document();
        }

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pain00700103;

        /// <summary>
        /// Sets the group header information for the reversal message.
        /// </summary>
        /// <param name="messageId">Unique identifier for the reversal message.</param>
        /// <param name="creationDateTime">Date and time when the reversal message was created.</param>
        /// <param name="numberOfTransactions">Total number of transactions to be reversed.</param>
        /// <param name="controlSum">Total amount of all transactions to be reversed.</param>
        /// <param name="groupReversal">Indicates if the entire group should be reversed.</param>
        /// <param name="initiatingParty">Party initiating the reversal instruction.</param>
        /// <param name="forwardingAgent">Financial institution that will forward the reversal.</param>
        /// <param name="debtorAgent">Financial institution servicing the debtor account.</param>
        /// <param name="creditorAgent">Financial institution servicing the creditor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        public Pain00700103Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            bool? groupReversal = null,
            PartyIdentification43? initiatingParty = null,
            BranchAndFinancialInstitutionIdentification5? forwardingAgent = null,
            BranchAndFinancialInstitutionIdentification5? debtorAgent = null,
            BranchAndFinancialInstitutionIdentification5? creditorAgent = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));

            var groupHeader = new GroupHeader56
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                InitgPty = initiatingParty,
                FwdgAgt = forwardingAgent,
                DbtrAgt = debtorAgent,
                CdtrAgt = creditorAgent
            };

            if (controlSum.HasValue)
            {
                groupHeader.CtrlSum = controlSum.Value;
                groupHeader.CtrlSumSpecified = true;
            }

            if (groupReversal.HasValue)
            {
                groupHeader.GrpRvsl = groupReversal.Value;
                groupHeader.GrpRvslSpecified = true;
            }

            _document.CstmrPmtRvsl = new CustomerPaymentReversalV03
            {
                GrpHdr = groupHeader
            };

            return this;
        }

        /// <summary>
        /// Sets the original group information that is being reversed.
        /// </summary>
        /// <param name="originalMessageId">Unique identifier of the original message to be reversed.</param>
        /// <param name="originalMessageNameId">Name identifier of the original message type.</param>
        /// <param name="originalCreationDateTime">Original creation date and time of the message being reversed.</param>
        /// <param name="reversalReasonInformation">Information about the reason for the reversal.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when group header is not set.</exception>
        public Pain00700103Builder SetOriginalGroupInformation(
            string originalMessageId,
            string originalMessageNameId,
            DateTime? originalCreationDateTime = null,
            IEnumerable<PaymentReversalReason7>? reversalReasonInformation = null)
        {
            ValidateParameter(originalMessageId, nameof(originalMessageId));
            ValidateParameter(originalMessageNameId, nameof(originalMessageNameId));

            if (_document.CstmrPmtRvsl == null)
                throw new InvalidOperationException("Group header must be set before setting original group information.");

            var originalGroupInfo = new OriginalGroupHeader3
            {
                OrgnlMsgId = originalMessageId,
                OrgnlMsgNmId = originalMessageNameId
            };

            if (originalCreationDateTime.HasValue)
            {
                originalGroupInfo.OrgnlCreDtTm = originalCreationDateTime.Value;
                originalGroupInfo.OrgnlCreDtTmSpecified = true;
            }

            if (reversalReasonInformation != null)
            {
                foreach (var reason in reversalReasonInformation)
                {
                    originalGroupInfo.RvslRsnInf.Add(reason);
                }
            }

            _document.CstmrPmtRvsl.OrgnlGrpInf = originalGroupInfo;
            return this;
        }

        /// <summary>
        /// Adds an original payment instruction to be reversed.
        /// </summary>
        /// <param name="reversalPaymentInformationId">Unique identifier for this reversal payment instruction.</param>
        /// <param name="originalPaymentInformationId">Unique identifier of the original payment instruction to be reversed.</param>
        /// <param name="originalNumberOfTransactions">Number of transactions in the original payment instruction.</param>
        /// <param name="originalControlSum">Total amount of the original payment instruction.</param>
        /// <param name="batchBooking">Indicates whether transactions should be processed individually or in batch.</param>
        /// <param name="paymentInformationReversal">Indicates if the entire payment instruction should be reversed.</param>
        /// <param name="reversalReasonInformation">Information about the reason for the reversal.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when original group information is not set.</exception>
        public Pain00700103Builder AddOriginalPaymentInstruction(
            string? reversalPaymentInformationId,
            string originalPaymentInformationId,
            string? originalNumberOfTransactions = null,
            decimal? originalControlSum = null,
            bool? batchBooking = null,
            bool? paymentInformationReversal = null,
            IEnumerable<PaymentReversalReason7>? reversalReasonInformation = null)
        {
            ValidateParameter(originalPaymentInformationId, nameof(originalPaymentInformationId));

            if (_document.CstmrPmtRvsl?.OrgnlGrpInf == null)
                throw new InvalidOperationException("Original group information must be set before adding payment instructions.");

            var originalPaymentInstruction = new OriginalPaymentInstruction2
            {
                OrgnlPmtInfId = originalPaymentInformationId,
                RvslPmtInfId = reversalPaymentInformationId,
                OrgnlNbOfTxs = originalNumberOfTransactions
            };

            if (originalControlSum.HasValue)
            {
                originalPaymentInstruction.OrgnlCtrlSum = originalControlSum.Value;
                originalPaymentInstruction.OrgnlCtrlSumSpecified = true;
            }

            if (batchBooking.HasValue)
            {
                originalPaymentInstruction.BtchBookg = batchBooking.Value;
                originalPaymentInstruction.BtchBookgSpecified = true;
            }

            if (paymentInformationReversal.HasValue)
            {
                originalPaymentInstruction.PmtInfRvsl = paymentInformationReversal.Value;
                originalPaymentInstruction.PmtInfRvslSpecified = true;
            }

            if (reversalReasonInformation != null)
            {
                foreach (var reason in reversalReasonInformation)
                {
                    originalPaymentInstruction.RvslRsnInf.Add(reason);
                }
            }

            _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Add(originalPaymentInstruction);
            return this;
        }

        /// <summary>
        /// Adds a payment transaction to be reversed to the last payment instruction.
        /// </summary>
        /// <param name="reversalTransaction">The payment transaction reversal information to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when reversalTransaction is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no payment instructions exist.</exception>
        public Pain00700103Builder AddPaymentTransactionReversal(PaymentTransaction35 reversalTransaction)
        {
            ValidateParameter(reversalTransaction, nameof(reversalTransaction));

            if (_document.CstmrPmtRvsl?.OrgnlPmtInfAndRvsl == null || _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count == 0)
                throw new InvalidOperationException("At least one payment instruction must be added before adding transaction reversals.");

            var lastPaymentInstruction = _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Last();
            lastPaymentInstruction.TxInf.Add(reversalTransaction);

            return this;
        }

        /// <summary>
        /// Creates a payment transaction reversal with the specified parameters.
        /// </summary>
        /// <param name="reversalId">Unique identifier for the reversal transaction.</param>
        /// <param name="originalInstructionId">Original instruction identification from the transaction to be reversed.</param>
        /// <param name="originalEndToEndId">Original end-to-end identification from the transaction to be reversed.</param>
        /// <param name="originalInstructedAmount">Original instructed amount to be reversed.</param>
        /// <param name="reversedInstructedAmount">Amount actually being reversed (may be partial).</param>
        /// <param name="chargeBearer">Charge bearer specification.</param>
        /// <param name="reversalReasonInformation">Information about the reason for the reversal.</param>
        /// <param name="originalTransactionReference">Reference information from the original transaction.</param>
        /// <returns>A configured PaymentTransaction35 instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        public static PaymentTransaction35 CreatePaymentTransactionReversal(
            string? reversalId,
            string? originalInstructionId,
            string? originalEndToEndId,
            ActiveOrHistoricCurrencyAndAmount? originalInstructedAmount,
            ActiveOrHistoricCurrencyAndAmount? reversedInstructedAmount,
            ChargeBearerType1Code? chargeBearer = null,
            IEnumerable<PaymentReversalReason7>? reversalReasonInformation = null,
            OriginalTransactionReference16? originalTransactionReference = null)
        {
            var transaction = new PaymentTransaction35
            {
                RvslId = reversalId,
                OrgnlInstrId = originalInstructionId,
                OrgnlEndToEndId = originalEndToEndId,
                OrgnlInstdAmt = originalInstructedAmount,
                RvsdInstdAmt = reversedInstructedAmount,
                OrgnlTxRef = originalTransactionReference
            };

            if (chargeBearer.HasValue)
            {
                transaction.ChrgBr = chargeBearer.Value;
                transaction.ChrgBrSpecified = true;
            }

            if (reversalReasonInformation != null)
            {
                foreach (var reason in reversalReasonInformation)
                {
                    transaction.RvslRsnInf.Add(reason);
                }
            }

            return transaction;
        }

        /// <summary>
        /// Creates a payment reversal reason with the specified parameters.
        /// </summary>
        /// <param name="originator">Party that originated the reversal reason.</param>
        /// <param name="reason">The reversal reason code or proprietary information.</param>
        /// <param name="additionalInformation">Additional textual information about the reversal.</param>
        /// <returns>A configured PaymentReversalReason7 instance.</returns>
        public static PaymentReversalReason7 CreatePaymentReversalReason(
            PartyIdentification43? originator = null,
            ReversalReason4Choice? reason = null,
            IEnumerable<string>? additionalInformation = null)
        {
            var reversalReason = new PaymentReversalReason7
            {
                Orgtr = originator,
                Rsn = reason
            };

            if (additionalInformation != null)
            {
                foreach (var info in additionalInformation)
                {
                    reversalReason.AddtlInf.Add(info);
                }
            }

            return reversalReason;
        }

        /// <summary>
        /// Creates a reversal reason choice with a code.
        /// </summary>
        /// <param name="code">The reversal reason code.</param>
        /// <returns>A configured ReversalReason4Choice instance.</returns>
        public static ReversalReason4Choice CreateReversalReasonWithCode(string code)
        {
            ValidateParameter(code, nameof(code));

            return new ReversalReason4Choice
            {
                Cd = code
            };
        }

        /// <summary>
        /// Creates a reversal reason choice with proprietary information.
        /// </summary>
        /// <param name="proprietary">The proprietary reversal reason.</param>
        /// <returns>A configured ReversalReason4Choice instance.</returns>
        public static ReversalReason4Choice CreateReversalReasonWithProprietary(string proprietary)
        {
            ValidateParameter(proprietary, nameof(proprietary));

            return new ReversalReason4Choice
            {
                Prtry = proprietary
            };
        }

        /// <summary>
        /// Creates a currency and amount instance.
        /// </summary>
        /// <param name="amount">The amount value.</param>
        /// <param name="currency">The currency code (ISO 4217 three-letter code).</param>
        /// <returns>A configured ActiveOrHistoricCurrencyAndAmount instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when currency is null.</exception>
        public static ActiveOrHistoricCurrencyAndAmount CreateCurrencyAndAmount(decimal amount, string currency)
        {
            ValidateParameter(currency, nameof(currency));

            return new ActiveOrHistoricCurrencyAndAmount
            {
                Value = amount,
                Ccy = currency
            };
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="supplementaryData">The supplementary data to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the message is not initialized.</exception>
        public Pain00700103Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            ValidateParameter(supplementaryData, nameof(supplementaryData));

            if (_document.CstmrPmtRvsl == null)
                throw new InvalidOperationException("Message must be initialized before adding supplementary data.");

            _document.CstmrPmtRvsl.SplmtryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Builds and returns the completed Document.
        /// </summary>
        /// <returns>The constructed Document instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required elements are missing.</exception>
        public Document Build()
        {
            ValidateDocument();
            return _document;
        }

        /// <summary>
        /// Builds the document and serializes it to XML.
        /// </summary>
        /// <returns>The XML representation of the pain.007.001.03 message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required elements are missing.</exception>
        public string BuildXml()
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Builds and serializes the provided message to XML format.
        /// </summary>
        /// <param name="message">The message to serialize. Must be an instance of Document.</param>
        /// <returns>XML representation of the message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="InvalidCastException">Thrown when message is not a valid Document.</exception>
        public string BuildXml(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message is not Document doc)
                throw new InvalidCastException($"Expected message of type {typeof(Document).Name}, but received {message.GetType().Name}");

            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Validates the current state of the document.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required elements are missing.</exception>
        private void ValidateDocument()
        {
            if (_document.CstmrPmtRvsl == null)
                throw new InvalidOperationException("Customer payment reversal must be set.");

            if (_document.CstmrPmtRvsl.GrpHdr == null)
                throw new InvalidOperationException("Group header must be set.");

            if (_document.CstmrPmtRvsl.OrgnlGrpInf == null)
                throw new InvalidOperationException("Original group information must be set.");

            if (_document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count == 0)
                throw new InvalidOperationException("At least one original payment instruction must be added.");
        }

        /// <summary>
        /// Validates that a parameter is not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        private static void ValidateParameter(object? parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}
