using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Application.Xml;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100110;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.10 messages (Customer Credit Transfer Initiation V10).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.10 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards. This version includes enhancements compared to V09, including
    /// improved instruction handling and additional validation features.
    /// </remarks>
    public class Pain00100110Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the Pain00100110Builder class.
        /// </summary>
        public Pain00100110Builder()
        {
            _document = new Document();
        }

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pain00100110;

        /// <summary>
        /// Sets the group header information for the message.
        /// </summary>
        /// <param name="messageId">Unique identifier for the message.</param>
        /// <param name="creationDateTime">Date and time when the message was created.</param>
        /// <param name="numberOfTransactions">Total number of transactions in the message.</param>
        /// <param name="controlSum">Total amount of all transactions in the message.</param>
        /// <param name="initiatingParty">Party initiating the payment instruction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        public Pain00100110Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            PartyIdentification135? initiatingParty = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));

            var groupHeader = new GroupHeader95
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                InitgPty = initiatingParty
            };

            if (controlSum.HasValue)
            {
                groupHeader.CtrlSum = controlSum.Value;
                groupHeader.CtrlSumSpecified = true;
            }

            _document.CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV10
            {
                GrpHdr = groupHeader
            };

            return this;
        }

        /// <summary>
        /// Adds a payment instruction to the message.
        /// Each payment instruction contains common information for a group of transactions.
        /// </summary>
        /// <param name="paymentInformationId">Unique identifier for the payment instruction.</param>
        /// <param name="paymentMethod">Payment method used for the instruction.</param>
        /// <param name="batchBooking">Indicates whether transactions should be processed individually or in batch.</param>
        /// <param name="numberOfTransactions">Number of transactions within this payment instruction.</param>
        /// <param name="controlSum">Total amount of all transactions within this payment instruction.</param>
        /// <param name="paymentTypeInformation">Additional payment type information.</param>
        /// <param name="requestedExecutionDate">Date on which the payment should be executed.</param>
        /// <param name="debtor">Party making the payment.</param>
        /// <param name="debtorAccount">Account to be debited.</param>
        /// <param name="debtorAgent">Financial institution servicing the debtor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when group header is not set.</exception>
        public Pain00100110Builder AddPaymentInstruction(
            string paymentInformationId,
            PaymentMethod3Code paymentMethod,
            bool? batchBooking,
            string numberOfTransactions,
            decimal? controlSum,
            PaymentTypeInformation26? paymentTypeInformation,
            DateTime requestedExecutionDate,
            PartyIdentification135 debtor,
            CashAccount38 debtorAccount,
            BranchAndFinancialInstitutionIdentification6 debtorAgent)
        {
            ValidateParameter(paymentInformationId, nameof(paymentInformationId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));
            ValidateParameter(debtor, nameof(debtor));
            ValidateParameter(debtorAccount, nameof(debtorAccount));
            ValidateParameter(debtorAgent, nameof(debtorAgent));

            if (_document.CstmrCdtTrfInitn == null)
                throw new InvalidOperationException("Group header must be set before adding payment instructions.");

            var paymentInstruction = new PaymentInstruction34
            {
                PmtInfId = paymentInformationId,
                PmtMtd = paymentMethod,
                NbOfTxs = numberOfTransactions,
                PmtTpInf = paymentTypeInformation,
                ReqdExctnDt = new DateAndDateTime2Choice { Dt = requestedExecutionDate, DtSpecified = true },
                Dbtr = debtor,
                DbtrAcct = debtorAccount,
                DbtrAgt = debtorAgent
            };

            if (batchBooking.HasValue)
            {
                paymentInstruction.BtchBookg = batchBooking.Value;
                paymentInstruction.BtchBookgSpecified = true;
            }

            if (controlSum.HasValue)
            {
                paymentInstruction.CtrlSum = controlSum.Value;
                paymentInstruction.CtrlSumSpecified = true;
            }

            _document.CstmrCdtTrfInitn.PmtInf.Add(paymentInstruction);
            return this;
        }

        /// <summary>
        /// Adds a credit transfer transaction to the last payment instruction.
        /// Each credit transfer transaction represents an individual payment within the instruction.
        /// </summary>
        /// <param name="creditTransfer">The credit transfer transaction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when creditTransfer is null.</exception>
        public Pain00100110Builder AddCreditTransferTransaction(CreditTransferTransaction40 creditTransfer)
        {
            ValidateParameter(creditTransfer, nameof(creditTransfer));

            if (_document.CstmrCdtTrfInitn?.PmtInf == null || !_document.CstmrCdtTrfInitn.PmtInf.Any())
                throw new InvalidOperationException("Payment instruction must be added before adding credit transfer transactions.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf.Last();
            lastPaymentInstruction.CdtTrfTxInf.Add(creditTransfer);

            return this;
        }

        /// <summary>
        /// Creates and adds a credit transfer transaction to the last payment instruction.
        /// This is a convenience method for creating simple credit transfer transactions.
        /// </summary>
        /// <param name="endToEndId">Unique identifier for the transaction that flows through the entire payment chain.</param>
        /// <param name="amount">Amount of the transfer including currency information.</param>
        /// <param name="creditor">Party receiving the payment.</param>
        /// <param name="creditorAccount">Account to be credited.</param>
        /// <param name="instructionId">Optional instruction identifier assigned by the debtor.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        public Pain00100110Builder AddCreditTransferTransaction(
            string endToEndId,
            AmountType4Choice amount,
            PartyIdentification135 creditor,
            CashAccount38 creditorAccount,
            string? instructionId = null)
        {
            ValidateParameter(endToEndId, nameof(endToEndId));
            ValidateParameter(amount, nameof(amount));
            ValidateParameter(creditor, nameof(creditor));
            ValidateParameter(creditorAccount, nameof(creditorAccount));

            var paymentId = new PaymentIdentification6
            {
                EndToEndId = endToEndId
            };

            if (!string.IsNullOrEmpty(instructionId))
            {
                paymentId.InstrId = instructionId;
            }

            var creditTransfer = new CreditTransferTransaction40
            {
                PmtId = paymentId,
                Amt = amount,
                Cdtr = creditor,
                CdtrAcct = creditorAccount
            };

            return AddCreditTransferTransaction(creditTransfer);
        }

        /// <summary>
        /// Sets the creditor agent (receiving financial institution) for the last credit transfer transaction.
        /// </summary>
        /// <param name="creditorAgent">Financial institution servicing the creditor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when creditorAgent is null.</exception>
        public Pain00100110Builder SetCreditorAgent(BranchAndFinancialInstitutionIdentification6 creditorAgent)
        {
            ValidateParameter(creditorAgent, nameof(creditorAgent));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.CdtrAgt = creditorAgent;

            return this;
        }

        /// <summary>
        /// Sets the creditor agent account for the last credit transfer transaction.
        /// </summary>
        /// <param name="creditorAgentAccount">Account of the creditor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when creditorAgentAccount is null.</exception>
        public Pain00100110Builder SetCreditorAgentAccount(CashAccount38 creditorAgentAccount)
        {
            ValidateParameter(creditorAgentAccount, nameof(creditorAgentAccount));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.CdtrAgtAcct = creditorAgentAccount;

            return this;
        }

        /// <summary>
        /// Sets the ultimate creditor for the last credit transfer transaction.
        /// </summary>
        /// <param name="ultimateCreditor">Ultimate party to receive the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when ultimateCreditor is null.</exception>
        public Pain00100110Builder SetUltimateCreditor(PartyIdentification135 ultimateCreditor)
        {
            ValidateParameter(ultimateCreditor, nameof(ultimateCreditor));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.UltmtCdtr = ultimateCreditor;

            return this;
        }

        /// <summary>
        /// Sets the ultimate debtor for the last credit transfer transaction.
        /// </summary>
        /// <param name="ultimateDebtor">Ultimate party that owes money to the creditor.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when ultimateDebtor is null.</exception>
        public Pain00100110Builder SetUltimateDebtor(PartyIdentification135 ultimateDebtor)
        {
            ValidateParameter(ultimateDebtor, nameof(ultimateDebtor));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.UltmtDbtr = ultimateDebtor;

            return this;
        }

        /// <summary>
        /// Adds an instruction for the creditor agent to the last credit transfer transaction.
        /// </summary>
        /// <param name="instruction">Instruction for the creditor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when instruction is null.</exception>
        public Pain00100110Builder AddInstructionForCreditorAgent(InstructionForCreditorAgent3 instruction)
        {
            ValidateParameter(instruction, nameof(instruction));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.InstrForCdtrAgt.Add(instruction);

            return this;
        }

        /// <summary>
        /// Sets the payment type information for the last credit transfer transaction.
        /// </summary>
        /// <param name="paymentTypeInformation">Payment type information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when paymentTypeInformation is null.</exception>
        public Pain00100110Builder SetPaymentTypeInformation(PaymentTypeInformation26 paymentTypeInformation)
        {
            ValidateParameter(paymentTypeInformation, nameof(paymentTypeInformation));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.PmtTpInf = paymentTypeInformation;

            return this;
        }

        /// <summary>
        /// Sets the remittance information for the last credit transfer transaction.
        /// </summary>
        /// <param name="remittanceInformation">Remittance information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when remittanceInformation is null.</exception>
        public Pain00100110Builder SetRemittanceInformation(RemittanceInformation16 remittanceInformation)
        {
            ValidateParameter(remittanceInformation, nameof(remittanceInformation));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.RmtInf = remittanceInformation;

            return this;
        }

        /// <summary>
        /// Sets the first intermediary agent for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent">First intermediary agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent is null.</exception>
        public Pain00100110Builder SetIntermediaryAgent1(BranchAndFinancialInstitutionIdentification6 intermediaryAgent)
        {
            ValidateParameter(intermediaryAgent, nameof(intermediaryAgent));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.IntrmyAgt1 = intermediaryAgent;

            return this;
        }

        /// <summary>
        /// Sets the first intermediary agent account for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgentAccount">First intermediary agent account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgentAccount is null.</exception>
        public Pain00100110Builder SetIntermediaryAgent1Account(CashAccount38 intermediaryAgentAccount)
        {
            ValidateParameter(intermediaryAgentAccount, nameof(intermediaryAgentAccount));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.IntrmyAgt1Acct = intermediaryAgentAccount;

            return this;
        }

        /// <summary>
        /// Sets the second intermediary agent for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent">Second intermediary agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent is null.</exception>
        public Pain00100110Builder SetIntermediaryAgent2(BranchAndFinancialInstitutionIdentification6 intermediaryAgent)
        {
            ValidateParameter(intermediaryAgent, nameof(intermediaryAgent));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.IntrmyAgt2 = intermediaryAgent;

            return this;
        }

        /// <summary>
        /// Sets the second intermediary agent account for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgentAccount">Second intermediary agent account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgentAccount is null.</exception>
        public Pain00100110Builder SetIntermediaryAgent2Account(CashAccount38 intermediaryAgentAccount)
        {
            ValidateParameter(intermediaryAgentAccount, nameof(intermediaryAgentAccount));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.IntrmyAgt2Acct = intermediaryAgentAccount;

            return this;
        }

        /// <summary>
        /// Sets the third intermediary agent for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent">Third intermediary agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent is null.</exception>
        public Pain00100110Builder SetIntermediaryAgent3(BranchAndFinancialInstitutionIdentification6 intermediaryAgent)
        {
            ValidateParameter(intermediaryAgent, nameof(intermediaryAgent));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.IntrmyAgt3 = intermediaryAgent;

            return this;
        }

        /// <summary>
        /// Sets the third intermediary agent account for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgentAccount">Third intermediary agent account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgentAccount is null.</exception>
        public Pain00100110Builder SetIntermediaryAgent3Account(CashAccount38 intermediaryAgentAccount)
        {
            ValidateParameter(intermediaryAgentAccount, nameof(intermediaryAgentAccount));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.IntrmyAgt3Acct = intermediaryAgentAccount;

            return this;
        }

        /// <summary>
        /// Sets the instruction for debtor agent for the last credit transfer transaction.
        /// This is a V10-specific feature that allows structured instructions to the debtor agent.
        /// </summary>
        /// <param name="instructionForDebtorAgent">Instruction for the debtor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no credit transfer transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when instructionForDebtorAgent is null.</exception>
        public Pain00100110Builder SetInstructionForDebtorAgent(InstructionForDebtorAgent1 instructionForDebtorAgent)
        {
            ValidateParameter(instructionForDebtorAgent, nameof(instructionForDebtorAgent));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.InstrForDbtrAgt = instructionForDebtorAgent;

            return this;
        }

        /// <summary>
        /// Builds the complete Pain.001.001.10 document.
        /// </summary>
        /// <returns>The constructed document object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the document is incomplete.</exception>
        public Document Build()
        {
            ValidateDocument();
            return _document;
        }

        /// <summary>
        /// Builds and serializes the message to XML format.
        /// </summary>
        /// <returns>XML representation of the pain.001.001.10 message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the document is incomplete.</exception>
        public string BuildXml()
        {
            ValidateDocument();
            return XmlSerializationService.Serialize(_document);
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
        /// Serializes the message to XML format.
        /// </summary>
        /// <returns>The XML representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the document is incomplete.</exception>
        public string ToXml()
        {
            ValidateDocument();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Gets the current document being built.
        /// </summary>
        /// <returns>The current document object.</returns>
        public object GetDocument()
        {
            return _document;
        }

        /// <summary>
        /// Validates that required parameters are not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null.</exception>
        private static void ValidateParameter(object? parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Gets the last credit transfer transaction from the last payment instruction.
        /// </summary>
        /// <returns>The last credit transfer transaction.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        private CreditTransferTransaction40 GetLastCreditTransferTransaction()
        {
            if (_document.CstmrCdtTrfInitn?.PmtInf == null || !_document.CstmrCdtTrfInitn.PmtInf.Any())
                throw new InvalidOperationException("Payment instruction must be added before modifying credit transfer transactions.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf.Last();
            if (lastPaymentInstruction.CdtTrfTxInf == null || !lastPaymentInstruction.CdtTrfTxInf.Any())
                throw new InvalidOperationException("Credit transfer transaction must be added before modifying it.");

            return lastPaymentInstruction.CdtTrfTxInf.Last();
        }

        /// <summary>
        /// Validates the document structure before building or serializing.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the document is incomplete.</exception>
        private void ValidateDocument()
        {
            if (_document.CstmrCdtTrfInitn == null)
                throw new InvalidOperationException("Group header must be set.");

            if (_document.CstmrCdtTrfInitn.PmtInf == null || !_document.CstmrCdtTrfInitn.PmtInf.Any())
                throw new InvalidOperationException("At least one payment instruction must be added.");

            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                if (paymentInstruction.CdtTrfTxInf == null || !paymentInstruction.CdtTrfTxInf.Any())
                    throw new InvalidOperationException($"Payment instruction '{paymentInstruction.PmtInfId}' must contain at least one credit transfer transaction.");
            }
        }
    }
}
