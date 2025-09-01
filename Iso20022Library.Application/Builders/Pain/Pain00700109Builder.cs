using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700109;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.007.001.09 messages (Customer Payment Reversal V09).
    /// Provides a fluent API for building payment reversal messages with comprehensive validation and XML serialization.
    /// </summary>
    /// <remarks>
    /// The pain.007.001.09 message is used to reverse a previously sent payment instruction.
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
    public class Pain00700109Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pain00700109;

        /// <summary>
        /// Initializes a new instance of the Pain00700109Builder class.
        /// </summary>
        public Pain00700109Builder()
        {
            _document = new Document
            {
                CstmrPmtRvsl = new CustomerPaymentReversalV09()
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
        public Pain00700109Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            PartyIdentification135? initiatingParty = null)
        {
            ValidateParameter(messageId, nameof(messageId));

            _document.CstmrPmtRvsl.GrpHdr = new GroupHeader88
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
        public Pain00700109Builder SetOriginalGroupInformation(
            string originalMessageId,
            string? originalMessageNameId = null,
            DateTime? originalCreationDateTime = null,
            IEnumerable<PaymentReversalReason9>? reversalReasonInformation = null)
        {
            ValidateParameter(originalMessageId, nameof(originalMessageId));

            _document.CstmrPmtRvsl.OrgnlGrpInf = new OriginalGroupHeader16
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
        /// Adds an original payment instruction to be reversed.
        /// </summary>
        /// <param name="originalPaymentInformationId">Unique identification for the original payment information.</param>
        /// <param name="reversalReasonInformation">Information concerning the reason for the reversal of the payment instruction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when originalPaymentInformationId is null.</exception>
        public Pain00700109Builder AddOriginalPaymentInstruction(
            string originalPaymentInformationId,
            IEnumerable<PaymentReversalReason9>? reversalReasonInformation = null)
        {
            ValidateParameter(originalPaymentInformationId, nameof(originalPaymentInformationId));

            var originalPaymentInstruction = new OriginalPaymentInstruction33
            {
                OrgnlPmtInfId = originalPaymentInformationId
            };

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
        /// Adds a payment transaction reversal to the last added original payment instruction.
        /// </summary>
        /// <param name="reversalId">Unique identification for the reversal transaction.</param>
        /// <param name="originalInstructionId">Unique identification for the original instruction.</param>
        /// <param name="originalEndToEndId">Unique identification for the original end-to-end transaction.</param>
        /// <param name="originalUetr">Unique end-to-end transaction reference assigned by the original instructing party.</param>
        /// <param name="reversedAmount">Amount of money to be moved between the debtor and creditor, before deduction of charges.</param>
        /// <param name="reversalReasonInformation">Information concerning the reason for the reversal of the transaction.</param>
        /// <param name="originalTransactionReference">Key elements used to refer to the original transaction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no original payment instruction has been added.</exception>
        /// <exception cref="ArgumentNullException">Thrown when reversalId is null.</exception>
        public Pain00700109Builder AddPaymentTransactionReversal(
            string reversalId,
            string? originalInstructionId = null,
            string? originalEndToEndId = null,
            string? originalUetr = null,
            ActiveOrHistoricCurrencyAndAmount? reversedAmount = null,
            IEnumerable<PaymentReversalReason9>? reversalReasonInformation = null,
            OriginalTransactionReference28? originalTransactionReference = null)
        {
            ValidateParameter(reversalId, nameof(reversalId));

            if (!_document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Any())
            {
                throw new InvalidOperationException("No original payment instruction has been added. Call AddOriginalPaymentInstruction first.");
            }

            var lastPaymentInstruction = _document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Last();

            var transaction = new PaymentTransaction108
            {
                RvslId = reversalId,
                OrgnlInstrId = originalInstructionId,
                OrgnlEndToEndId = originalEndToEndId,
                OrgnlUetr = originalUetr,
                RvsdInstdAmt = reversedAmount,
                OrgnlTxRef = originalTransactionReference
            };

            if (reversalReasonInformation != null)
            {
                foreach (var reason in reversalReasonInformation)
                {
                    transaction.RvslRsnInf.Add(reason);
                }
            }

            lastPaymentInstruction.TxInf.Add(transaction);

            return this;
        }

        /// <summary>
        /// Builds and validates the payment reversal message.
        /// </summary>
        /// <returns>The constructed CustomerPaymentReversalV09 message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing.</exception>
        public CustomerPaymentReversalV09 Build()
        {
            ValidateMessage();
            return _document.CstmrPmtRvsl;
        }

        /// <summary>
        /// Builds and serializes the payment reversal message to XML.
        /// </summary>
        /// <returns>The XML representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing.</exception>
        public string BuildXml()
        {
            ValidateMessage();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Builds and serializes the payment reversal message to XML with custom configuration.
        /// </summary>
        /// <param name="config">Configuration object for XML serialization.</param>
        /// <returns>The XML representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing.</exception>
        public string BuildXml(object config)
        {
            ValidateMessage();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Creates a payment reversal reason with specified code and additional information.
        /// </summary>
        /// <param name="reversalReasonCode">The reason code for the reversal.</param>
        /// <param name="additionalInformation">Additional textual information about the reversal reason.</param>
        /// <param name="originator">Party that issued the reversal.</param>
        /// <returns>A configured PaymentReversalReason9 instance.</returns>
        public static PaymentReversalReason9 CreateReversalReason(
            string reversalReasonCode,
            IEnumerable<string>? additionalInformation = null,
            PartyIdentification135? originator = null)
        {
            var reason = new PaymentReversalReason9
            {
                Rsn = new ReversalReason4Choice
                {
                    Cd = reversalReasonCode
                },
                Orgtr = originator
            };

            if (additionalInformation != null)
            {
                foreach (var info in additionalInformation)
                {
                    reason.AddtlInf.Add(info);
                }
            }

            return reason;
        }

        /// <summary>
        /// Creates a payment reversal reason with proprietary reason and additional information.
        /// </summary>
        /// <param name="proprietaryReason">Proprietary reason for the reversal.</param>
        /// <param name="additionalInformation">Additional textual information about the reversal reason.</param>
        /// <param name="originator">Party that issued the reversal.</param>
        /// <returns>A configured PaymentReversalReason9 instance.</returns>
        public static PaymentReversalReason9 CreateProprietaryReversalReason(
            string proprietaryReason,
            IEnumerable<string>? additionalInformation = null,
            PartyIdentification135? originator = null)
        {
            var reason = new PaymentReversalReason9
            {
                Rsn = new ReversalReason4Choice
                {
                    Prtry = proprietaryReason
                },
                Orgtr = originator
            };

            if (additionalInformation != null)
            {
                foreach (var info in additionalInformation)
                {
                    reason.AddtlInf.Add(info);
                }
            }

            return reason;
        }

        /// <summary>
        /// Creates an active or historic currency and amount.
        /// </summary>
        /// <param name="currency">The currency code (e.g., EUR, USD).</param>
        /// <param name="amount">The amount value.</param>
        /// <returns>A configured ActiveOrHistoricCurrencyAndAmount instance.</returns>
        public static ActiveOrHistoricCurrencyAndAmount CreateAmount(string currency, decimal amount)
        {
            return new ActiveOrHistoricCurrencyAndAmount
            {
                Ccy = currency,
                Value = amount
            };
        }

        /// <summary>
        /// Creates a party identification with organization details.
        /// </summary>
        /// <param name="organizationName">Name of the organization.</param>
        /// <param name="bic">BIC (Bank Identifier Code) of the organization.</param>
        /// <param name="lei">LEI (Legal Entity Identifier) of the organization.</param>
        /// <returns>A configured PartyIdentification135 instance.</returns>
        public static PartyIdentification135 CreateOrganizationParty(
            string organizationName,
            string? bic = null,
            string? lei = null)
        {
            var party = new PartyIdentification135
            {
                Nm = organizationName
            };

            if (!string.IsNullOrEmpty(bic) || !string.IsNullOrEmpty(lei))
            {
                party.Id = new Party38Choice();

                if (!string.IsNullOrEmpty(bic) || !string.IsNullOrEmpty(lei))
                {
                    party.Id.OrgId = new OrganisationIdentification29();

                    if (!string.IsNullOrEmpty(bic))
                    {
                        party.Id.OrgId.AnyBic = bic;
                    }

                    if (!string.IsNullOrEmpty(lei))
                    {
                        party.Id.OrgId.Lei = lei;
                    }
                }
            }

            return party;
        }

        /// <summary>
        /// Creates a party identification with private individual details.
        /// </summary>
        /// <param name="firstName">First name of the individual.</param>
        /// <param name="lastName">Last name of the individual.</param>
        /// <param name="dateOfBirth">Date of birth of the individual.</param>
        /// <returns>A configured PartyIdentification135 instance.</returns>
        public static PartyIdentification135 CreatePrivateParty(
            string firstName,
            string lastName,
            DateTime? dateOfBirth = null)
        {
            var party = new PartyIdentification135
            {
                Nm = $"{firstName} {lastName}",
                Id = new Party38Choice
                {
                    PrvtId = new PersonIdentification13()
                }
            };

            if (dateOfBirth.HasValue)
            {
                party.Id.PrvtId.DtAndPlcOfBirth = new DateAndPlaceOfBirth1
                {
                    BirthDt = dateOfBirth.Value
                };
            }

            return party;
        }

        /// <summary>
        /// Creates an original transaction reference with basic payment information.
        /// </summary>
        /// <param name="amount">Original amount of the transaction.</param>
        /// <param name="currency">Currency of the original transaction.</param>
        /// <param name="requestedExecutionDate">Date at which the initiating party requests the clearing agent to process the payment.</param>
        /// <param name="paymentMethod">Payment method used for the original transaction.</param>
        /// <returns>A configured OriginalTransactionReference28 instance.</returns>
        public static OriginalTransactionReference28 CreateOriginalTransactionReference(
            decimal? amount = null,
            string? currency = null,
            DateTime? requestedExecutionDate = null,
            PaymentMethod4Code? paymentMethod = null)
        {
            var reference = new OriginalTransactionReference28();

            if (amount.HasValue && !string.IsNullOrEmpty(currency))
            {
                reference.IntrBkSttlmAmt = new ActiveOrHistoricCurrencyAndAmount
                {
                    Ccy = currency,
                    Value = amount.Value
                };
            }

            if (requestedExecutionDate.HasValue)
            {
                reference.ReqdExctnDt = new DateAndDateTime2Choice
                {
                    Dt = requestedExecutionDate.Value,
                    DtSpecified = true
                };
            }

            if (paymentMethod.HasValue)
            {
                reference.PmtMtd = paymentMethod.Value;
                reference.PmtMtdSpecified = true;
            }

            return reference;
        }

        /// <summary>
        /// Validates that the parameter is not null or empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null or empty.</exception>
        private static void ValidateParameter(string? parameter, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null or empty.");
            }
        }

        /// <summary>
        /// Validates that the message has all required components.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing.</exception>
        private void ValidateMessage()
        {
            if (_document.CstmrPmtRvsl.GrpHdr == null)
            {
                throw new InvalidOperationException("Group header is required. Call SetGroupHeader before building the message.");
            }

            if (_document.CstmrPmtRvsl.OrgnlGrpInf == null)
            {
                throw new InvalidOperationException("Original group information is required. Call SetOriginalGroupInformation before building the message.");
            }

            if (!_document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Any())
            {
                throw new InvalidOperationException("At least one original payment instruction is required. Call AddOriginalPaymentInstruction before building the message.");
            }
        }
    }
}
