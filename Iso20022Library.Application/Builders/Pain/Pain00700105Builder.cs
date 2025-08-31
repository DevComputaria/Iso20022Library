using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700105;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.007.001.05 messages (Customer Payment Reversal V05).
    /// Provides a fluent API for building payment reversal messages with comprehensive validation and XML serialization.
    /// </summary>
    /// <remarks>
    /// The pain.007.001.05 message is used to reverse a previously sent payment instruction.
    /// This builder handles the construction of reversal messages with group headers, original group information,
    /// original payment instructions, and payment transaction reversals according to ISO 20022 standards.
    /// 
    /// Key features:
    /// - Fluent API design for intuitive message construction
    /// - Comprehensive validation for all required fields
    /// - Support for both group-level and transaction-level reversals
    /// - Helper methods for creating reversal reasons and statuses
    /// - XML serialization capabilities
    /// </remarks>
    public class Pain00700105Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pain00700105;

        /// <summary>
        /// Initializes a new instance of the Pain00700105Builder class.
        /// </summary>
        public Pain00700105Builder()
        {
            _document = new Document
            {
                CstmrPmtRvsl = new CustomerPaymentReversalV05()
            };
        }

        /// <summary>
        /// Sets the group header for the payment reversal message.
        /// </summary>
        /// <param name="messageId">Unique identification for the message assigned by the instructing party.</param>
        /// <param name="creationDateTime">Date and time at which the message was created.</param>
        /// <param name="numberOfTransactions">Number of individual transaction information blocks contained in the message.</param>
        /// <param name="controlSum">Total amount for all individual payment reversals included in the message.</param>
        /// <param name="groupReversal">Indicates whether the reversal applies to the complete original message or to individual transactions.</param>
        /// <param name="initiatingParty">Party that initiates the payment reversal.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when messageId or numberOfTransactions is null.</exception>
        public Pain00700105Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            bool? groupReversal = null,
            PartyIdentification43? initiatingParty = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));

            _document.CstmrPmtRvsl.GrpHdr = new GroupHeader56
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions
            };

            if (controlSum.HasValue)
            {
                _document.CstmrPmtRvsl.GrpHdr.CtrlSum = controlSum.Value;
                _document.CstmrPmtRvsl.GrpHdr.CtrlSumSpecified = true;
            }

            if (groupReversal.HasValue)
            {
                _document.CstmrPmtRvsl.GrpHdr.GrpRvsl = groupReversal.Value;
                _document.CstmrPmtRvsl.GrpHdr.GrpRvslSpecified = true;
            }

            if (initiatingParty != null)
            {
                _document.CstmrPmtRvsl.GrpHdr.InitgPty = initiatingParty;
            }

            return this;
        }

        /// <summary>
        /// Sets the original group information and reversal reasons for the payment reversal message.
        /// </summary>
        /// <param name="originalMessageId">Point to point reference assigned by the instructing party to identify the original message.</param>
        /// <param name="originalMessageNameId">Specifies the original message name identifier to which the message refers.</param>
        /// <param name="originalCreationDateTime">Date and time at which the original message was created.</param>
        /// <param name="reversalReasonInformation">Set of elements used to provide information on the reversal reason.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalMessageId or originalMessageNameId is null.</exception>
        public Pain00700105Builder SetOriginalGroupInformation(
            string originalMessageId,
            string originalMessageNameId,
            DateTime? originalCreationDateTime = null,
            IEnumerable<PaymentReversalReason7>? reversalReasonInformation = null)
        {
            ValidateParameter(originalMessageId, nameof(originalMessageId));
            ValidateParameter(originalMessageNameId, nameof(originalMessageNameId));

            _document.CstmrPmtRvsl.OrgnlGrpInf = new OriginalGroupHeader3
            {
                OrgnlMsgId = originalMessageId,
                OrgnlMsgNmId = originalMessageNameId
            };

            if (originalCreationDateTime.HasValue)
            {
                _document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlCreDtTm = originalCreationDateTime.Value;
                _document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlCreDtTmSpecified = true;
            }

            if (reversalReasonInformation != null)
            {
                foreach (var reason in reversalReasonInformation)
                {
                    _document.CstmrPmtRvsl.OrgnlGrpInf.RvslRsnInf.Add(reason);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds an original payment instruction to be reversed to the payment reversal message.
        /// </summary>
        /// <param name="reversalPaymentInformationId">Unique identification for the reversal payment information block.</param>
        /// <param name="originalPaymentInformationId">Unique identification for the original payment information block.</param>
        /// <param name="originalNumberOfTransactions">Number of individual transactions contained in the original payment information group.</param>
        /// <param name="originalControlSum">Total amount for all individual payment transactions included in the original payment information group.</param>
        /// <param name="batchBooking">Identifies whether a single entry per individual transaction or a batch entry for the sum of the amounts of all transactions within the group of a message is requested.</param>
        /// <param name="paymentInformationReversal">Indicates whether the reversal applies to the complete original payment information block or to individual transactions.</param>
        /// <param name="reversalReasonInformation">Set of elements used to provide information on the reversal reason.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInformationId is null.</exception>
        public Pain00700105Builder AddOriginalPaymentInstruction(
            string? reversalPaymentInformationId,
            string originalPaymentInformationId,
            string? originalNumberOfTransactions = null,
            decimal? originalControlSum = null,
            bool? batchBooking = null,
            bool? paymentInformationReversal = null,
            IEnumerable<PaymentReversalReason7>? reversalReasonInformation = null)
        {
            ValidateParameter(originalPaymentInformationId, nameof(originalPaymentInformationId));

            var originalPaymentInstruction = new OriginalPaymentInstruction11
            {
                OrgnlPmtInfId = originalPaymentInformationId
            };

            if (!string.IsNullOrEmpty(reversalPaymentInformationId))
            {
                originalPaymentInstruction.RvslPmtInfId = reversalPaymentInformationId;
            }

            if (!string.IsNullOrEmpty(originalNumberOfTransactions))
            {
                originalPaymentInstruction.OrgnlNbOfTxs = originalNumberOfTransactions;
            }

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
        public Pain00700105Builder AddPaymentTransactionReversal(PaymentTransaction56 reversalTransaction)
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
        /// <param name="originalTransactionReference">Reference information about the original transaction.</param>
        /// <returns>A configured PaymentTransaction56 instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalInstructionId or originalEndToEndId is null.</exception>
        public static PaymentTransaction56 CreatePaymentTransactionReversal(
            string? reversalId,
            string originalInstructionId,
            string originalEndToEndId,
            ActiveOrHistoricCurrencyAndAmount? originalInstructedAmount = null,
            ActiveOrHistoricCurrencyAndAmount? reversedInstructedAmount = null,
            ChargeBearerType1Code? chargeBearer = null,
            IEnumerable<PaymentReversalReason7>? reversalReasonInformation = null,
            OriginalTransactionReference20? originalTransactionReference = null)
        {
            ValidateParameter(originalInstructionId, nameof(originalInstructionId));
            ValidateParameter(originalEndToEndId, nameof(originalEndToEndId));

            var transaction = new PaymentTransaction56
            {
                OrgnlInstrId = originalInstructionId,
                OrgnlEndToEndId = originalEndToEndId
            };

            if (!string.IsNullOrEmpty(reversalId))
            {
                transaction.RvslId = reversalId;
            }

            if (originalInstructedAmount != null)
            {
                transaction.OrgnlInstdAmt = originalInstructedAmount;
            }

            if (reversedInstructedAmount != null)
            {
                transaction.RvsdInstdAmt = reversedInstructedAmount;
            }

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

            if (originalTransactionReference != null)
            {
                transaction.OrgnlTxRef = originalTransactionReference;
            }

            return transaction;
        }

        /// <summary>
        /// Creates a payment reversal reason with the specified parameters.
        /// </summary>
        /// <param name="originator">Party that issues the reversal.</param>
        /// <param name="reason">Specifies the reason for the reversal.</param>
        /// <param name="additionalInformation">Additional information about the reversal reason.</param>
        /// <returns>A configured PaymentReversalReason7 instance.</returns>
        public static PaymentReversalReason7 CreatePaymentReversalReason(
            PartyIdentification43? originator = null,
            ReversalReason4Choice? reason = null,
            IEnumerable<string>? additionalInformation = null)
        {
            var reversalReason = new PaymentReversalReason7();

            if (originator != null)
            {
                reversalReason.Orgtr = originator;
            }

            if (reason != null)
            {
                reversalReason.Rsn = reason;
            }

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
        /// Creates a reversal reason with a specific reason code.
        /// </summary>
        /// <param name="reasonCode">The reversal reason code.</param>
        /// <returns>A configured ReversalReason4Choice instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when reasonCode is null.</exception>
        public static ReversalReason4Choice CreateReversalReasonWithCode(string reasonCode)
        {
            ValidateParameter(reasonCode, nameof(reasonCode));

            return new ReversalReason4Choice
            {
                Cd = reasonCode
            };
        }

        /// <summary>
        /// Creates a reversal reason with a proprietary reason.
        /// </summary>
        /// <param name="proprietaryReason">The proprietary reversal reason.</param>
        /// <returns>A configured ReversalReason4Choice instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when proprietaryReason is null.</exception>
        public static ReversalReason4Choice CreateReversalReasonWithProprietary(string proprietaryReason)
        {
            ValidateParameter(proprietaryReason, nameof(proprietaryReason));

            return new ReversalReason4Choice
            {
                Prtry = proprietaryReason
            };
        }

        /// <summary>
        /// Creates an active or historic currency and amount.
        /// </summary>
        /// <param name="amount">The amount value.</param>
        /// <param name="currency">The currency code.</param>
        /// <returns>A configured ActiveOrHistoricCurrencyAndAmount instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when currency is null.</exception>
        /// <exception cref="ArgumentException">Thrown when amount is negative.</exception>
        public static ActiveOrHistoricCurrencyAndAmount CreateCurrencyAndAmount(decimal amount, string currency)
        {
            ValidateParameter(currency, nameof(currency));

            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            return new ActiveOrHistoricCurrencyAndAmount
            {
                Ccy = currency,
                Value = amount
            };
        }

        /// <summary>
        /// Creates a party identification with the specified name.
        /// </summary>
        /// <param name="name">The party name.</param>
        /// <param name="postalAddress">The party's postal address.</param>
        /// <param name="identification">The party's identification.</param>
        /// <param name="countryOfResidence">The party's country of residence.</param>
        /// <param name="contactDetails">The party's contact details.</param>
        /// <returns>A configured PartyIdentification43 instance.</returns>
        public static PartyIdentification43 CreatePartyIdentification(
            string? name = null,
            PostalAddress6? postalAddress = null,
            Party11Choice? identification = null,
            string? countryOfResidence = null,
            ContactDetails2? contactDetails = null)
        {
            var party = new PartyIdentification43();

            if (!string.IsNullOrEmpty(name))
            {
                party.Nm = name;
            }

            if (postalAddress != null)
            {
                party.PstlAdr = postalAddress;
            }

            if (identification != null)
            {
                party.Id = identification;
            }

            if (!string.IsNullOrEmpty(countryOfResidence))
            {
                party.CtryOfRes = countryOfResidence;
            }

            if (contactDetails != null)
            {
                party.CtctDtls = contactDetails;
            }

            return party;
        }

        /// <summary>
        /// Adds supplementary data to the payment reversal message.
        /// </summary>
        /// <param name="supplementaryData">The supplementary data to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        public Pain00700105Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            ValidateParameter(supplementaryData, nameof(supplementaryData));

            _document.CstmrPmtRvsl.SplmtryData.Add(supplementaryData);

            return this;
        }

        /// <summary>
        /// Builds and returns the complete payment reversal document.
        /// </summary>
        /// <returns>The constructed Document instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required components are missing.</exception>
        public Document Build()
        {
            ValidateDocument();
            return _document;
        }

        /// <summary>
        /// Builds the message and returns its XML representation.
        /// </summary>
        /// <returns>The XML representation of the payment reversal message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required components are missing.</exception>
        public string BuildXml()
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Builds the message from an object and returns its XML representation.
        /// This method is required by the IMessageBuilder interface.
        /// </summary>
        /// <param name="message">The message object to serialize. Must be a Document instance.</param>
        /// <returns>The XML representation of the message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="ArgumentException">Thrown when message is not a Document instance.</exception>
        public string BuildXml(object message)
        {
            ValidateParameter(message, nameof(message));

            if (message is not Document document)
                throw new ArgumentException("Message must be a Document instance.", nameof(message));

            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Validates the document structure to ensure all required components are present.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required components are missing.</exception>
        private void ValidateDocument()
        {
            if (_document.CstmrPmtRvsl?.GrpHdr == null)
                throw new InvalidOperationException("Group header must be set.");

            if (_document.CstmrPmtRvsl?.OrgnlGrpInf == null)
                throw new InvalidOperationException("Original group information must be set.");

            if (_document.CstmrPmtRvsl?.OrgnlPmtInfAndRvsl == null || _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count == 0)
                throw new InvalidOperationException("At least one original payment instruction must be added.");
        }

        /// <summary>
        /// Validates a parameter for null reference.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
        private static void ValidateParameter(object? value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}
