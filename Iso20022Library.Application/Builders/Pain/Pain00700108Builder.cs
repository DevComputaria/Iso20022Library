using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700108;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.007.001.08 messages (Customer Payment Reversal V08).
    /// Provides a fluent API for building payment reversal messages with comprehensive validation and XML serialization.
    /// </summary>
    /// <remarks>
    /// The pain.007.001.08 message is used to reverse a previously sent payment instruction.
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
    public class Pain00700108Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pain00700108;

        /// <summary>
        /// Initializes a new instance of the Pain00700108Builder class.
        /// </summary>
        public Pain00700108Builder()
        {
            _document = new Document
            {
                CstmrPmtRvsl = new CustomerPaymentReversalV08()
            };
        }

        /// <summary>
        /// Sets the group header for the payment reversal message.
        /// </summary>
        /// <param name="messageId">Unique identification for the message assigned by the instructing party.</param>
        /// <param name="creationDateTime">Date and time at which the message was created.</param>
        /// <param name="numberOfTransactions">Number of individual transactions contained in the message.</param>
        /// <param name="controlSum">Total of all individual amounts included in the message.</param>
        /// <param name="initiatingParty">Party that initiates the payment reversal message.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when messageId or initiatingParty is null.</exception>
        public Pain00700108Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            PartyIdentification125? initiatingParty = null)
        {
            ValidateParameter(messageId, nameof(messageId));

            _document.CstmrPmtRvsl.GrpHdr = new GroupHeader75
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                InitgPty = initiatingParty
            };

            if (controlSum.HasValue)
            {
                _document.CstmrPmtRvsl.GrpHdr.CtrlSum = controlSum.Value;
                _document.CstmrPmtRvsl.GrpHdr.CtrlSumSpecified = true;
            }

            return this;
        }

        /// <summary>
        /// Sets the original group information for the payment reversal message.
        /// </summary>
        /// <param name="originalMessageId">Unique identification for the original message.</param>
        /// <param name="originalMessageNameId">Specifies the original message name identifier to which the message refers.</param>
        /// <param name="originalCreationDateTime">Date and time at which the original message was created.</param>
        /// <param name="reversalReasonInformation">Information concerning the reason for the reversal of the transaction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalMessageId is null.</exception>
        public Pain00700108Builder SetOriginalGroupInformation(
            string originalMessageId,
            string? originalMessageNameId = null,
            DateTime? originalCreationDateTime = null,
            IEnumerable<PaymentReversalReason8>? reversalReasonInformation = null)
        {
            ValidateParameter(originalMessageId, nameof(originalMessageId));

            _document.CstmrPmtRvsl.OrgnlGrpInf = new OriginalGroupHeader11
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
        /// <param name="batchBooking">Indicates whether the transactions will be individually booked or bulk booked.</param>
        /// <param name="paymentInformationReversal">Indicates whether the reversal applies to the payment information level.</param>
        /// <param name="reversalReasonInformation">Information concerning the reason for the reversal of the transaction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInformationId is null.</exception>
        public Pain00700108Builder AddOriginalPaymentInstruction(
            string? reversalPaymentInformationId,
            string originalPaymentInformationId,
            string? originalNumberOfTransactions = null,
            decimal? originalControlSum = null,
            bool? batchBooking = null,
            bool? paymentInformationReversal = null,
            IEnumerable<PaymentReversalReason8>? reversalReasonInformation = null)
        {
            ValidateParameter(originalPaymentInformationId, nameof(originalPaymentInformationId));

            var originalPaymentInstruction = new OriginalPaymentInstruction28
            {
                RvslPmtInfId = reversalPaymentInformationId,
                OrgnlPmtInfId = originalPaymentInformationId,
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
        public Pain00700108Builder AddPaymentTransactionReversal(PaymentTransaction93 reversalTransaction)
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
        /// <param name="originalTransactionReference">Key elements used to refer to the original transaction.</param>
        /// <returns>A configured PaymentTransaction93 instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when reversalId is null.</exception>
        public static PaymentTransaction93 CreatePaymentTransactionReversal(
            string reversalId,
            string? originalInstructionId = null,
            string? originalEndToEndId = null,
            ActiveOrHistoricCurrencyAndAmount? originalInstructedAmount = null,
            ActiveOrHistoricCurrencyAndAmount? reversedInstructedAmount = null,
            ChargeBearerType1Code? chargeBearer = null,
            IEnumerable<PaymentReversalReason8>? reversalReasonInformation = null,
            OriginalTransactionReference27? originalTransactionReference = null)
        {
            if (string.IsNullOrEmpty(reversalId))
                throw new ArgumentNullException(nameof(reversalId));

            var transaction = new PaymentTransaction93
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
        /// <param name="originator">Party that issues the reversal.</param>
        /// <param name="reason">Specifies the reason for the reversal.</param>
        /// <param name="additionalInformation">Further details on the reversal reason.</param>
        /// <returns>A configured PaymentReversalReason8 instance.</returns>
        public static PaymentReversalReason8 CreatePaymentReversalReason(
            PartyIdentification125? originator = null,
            ReversalReason4Choice? reason = null,
            IEnumerable<string>? additionalInformation = null)
        {
            var reversalReason = new PaymentReversalReason8
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
        /// Creates a reversal reason with a specific code.
        /// </summary>
        /// <param name="code">The reversal reason code.</param>
        /// <returns>A configured ReversalReason4Choice instance.</returns>
        public static ReversalReason4Choice CreateReversalReasonWithCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            return new ReversalReason4Choice { Cd = code };
        }

        /// <summary>
        /// Creates a reversal reason with proprietary information.
        /// </summary>
        /// <param name="proprietary">The proprietary reversal reason.</param>
        /// <returns>A configured ReversalReason4Choice instance.</returns>
        public static ReversalReason4Choice CreateReversalReasonWithProprietary(string proprietary)
        {
            if (string.IsNullOrEmpty(proprietary))
                throw new ArgumentNullException(nameof(proprietary));

            return new ReversalReason4Choice { Prtry = proprietary };
        }

        /// <summary>
        /// Creates a party identification with the specified name.
        /// </summary>
        /// <param name="name">Name by which a party is known and which is usually used to identify that party.</param>
        /// <param name="organisationIdentification">Unique identification of an organisation.</param>
        /// <param name="privateIdentification">Unique identification of a person.</param>
        /// <returns>A configured PartyIdentification125 instance.</returns>
        public static PartyIdentification125 CreatePartyIdentification(
            string? name = null,
            OrganisationIdentification8? organisationIdentification = null,
            PersonIdentification13? privateIdentification = null)
        {
            var party = new PartyIdentification125
            {
                Nm = name
            };

            if (organisationIdentification != null || privateIdentification != null)
            {
                party.Id = new Party34Choice
                {
                    OrgId = organisationIdentification,
                    PrvtId = privateIdentification
                };
            }

            return party;
        }

        /// <summary>
        /// Creates an active or historic currency and amount with the specified value and currency.
        /// </summary>
        /// <param name="value">The monetary amount.</param>
        /// <param name="currency">The currency code (e.g., "EUR", "USD").</param>
        /// <returns>A configured ActiveOrHistoricCurrencyAndAmount instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when currency is null or empty.</exception>
        public static ActiveOrHistoricCurrencyAndAmount CreateAmount(decimal value, string currency)
        {
            if (string.IsNullOrEmpty(currency))
                throw new ArgumentNullException(nameof(currency));

            return new ActiveOrHistoricCurrencyAndAmount
            {
                Value = value,
                Ccy = currency
            };
        }

        /// <summary>
        /// Creates an original transaction reference with the specified parameters.
        /// </summary>
        /// <param name="amount">Amount of money to be moved between the debtor and creditor.</param>
        /// <param name="requestedExecutionDate">Date at which the initiating party requests the clearing agent to process the payment.</param>
        /// <param name="requestedCollectionDate">Date at which the creditor requests the amount to be collected from the debtor.</param>
        /// <param name="creditorSchemeIdentification">Credit party that signed the mandate.</param>
        /// <param name="paymentTypeInformation">Set of elements used to further specify the type of transaction.</param>
        /// <param name="paymentMethod">Specifies the means of payment that will be used to move the amount of money.</param>
        /// <param name="mandateRelatedInformation">Set of elements used to provide further details related to a direct debit mandate signed between the creditor and the debtor.</param>
        /// <param name="remittanceInformation">Information supplied to enable the matching of an entry with the items that the transfer is intended to settle.</param>
        /// <returns>A configured OriginalTransactionReference27 instance.</returns>
        public static OriginalTransactionReference27 CreateOriginalTransactionReference(
            AmountType4Choice? amount = null,
            DateTime? requestedExecutionDate = null,
            DateTime? requestedCollectionDate = null,
            PartyIdentification125? creditorSchemeIdentification = null,
            PaymentTypeInformation25? paymentTypeInformation = null,
            PaymentMethod4Code? paymentMethod = null,
            MandateRelatedInformation12? mandateRelatedInformation = null,
            RemittanceInformation15? remittanceInformation = null)
        {
            var reference = new OriginalTransactionReference27
            {
                Amt = amount,
                CdtrSchmeId = creditorSchemeIdentification,
                PmtTpInf = paymentTypeInformation,
                RmtInf = remittanceInformation,
                MndtRltdInf = mandateRelatedInformation
            };

            if (requestedExecutionDate.HasValue)
            {
                reference.ReqdExctnDt = new DateAndDateTime2Choice
                {
                    DtTm = requestedExecutionDate.Value,
                    DtTmSpecified = true
                };
            }

            if (requestedCollectionDate.HasValue)
            {
                reference.ReqdColltnDt = requestedCollectionDate.Value;
                reference.ReqdColltnDtSpecified = true;
            }

            if (paymentMethod.HasValue)
            {
                reference.PmtMtd = paymentMethod.Value;
                reference.PmtMtdSpecified = true;
            }

            return reference;
        }

        /// <summary>
        /// Builds and returns the constructed Document.
        /// </summary>
        /// <returns>The constructed Document object containing the customer payment reversal.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing.</exception>
        public Document Build()
        {
            if (_document.CstmrPmtRvsl?.GrpHdr == null)
                throw new InvalidOperationException("Group header must be set before building the document.");

            if (_document.CstmrPmtRvsl?.OrgnlGrpInf == null)
                throw new InvalidOperationException("Original group information must be set before building the document.");

            return _document;
        }

        /// <summary>
        /// Generates the XML representation of the constructed message.
        /// </summary>
        /// <returns>A string containing the XML representation of the pain.007.001.08 message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing.</exception>
        public string ToXml()
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Builds XML from a given message object.
        /// </summary>
        /// <param name="message">The message object to serialize to XML.</param>
        /// <returns>A string containing the XML representation of the message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        public string BuildXml(object message)
        {
            ValidateParameter(message, nameof(message));
            return XmlSerializationService.Serialize(message);
        }

        /// <summary>
        /// Validates that a parameter is not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        private static void ValidateParameter(object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}
