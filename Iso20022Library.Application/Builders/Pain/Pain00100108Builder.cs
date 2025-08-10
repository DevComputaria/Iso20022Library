using System;
using System.Collections.ObjectModel;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100108;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.08 messages (Customer Credit Transfer Initiation V08).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.08 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00100108Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the Pain00100108Builder class.
        /// </summary>
        public Pain00100108Builder()
        {
            _document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV08()
            };
        }

        /// <summary>
        /// Sets the group header information for the message.
        /// The group header contains information that applies to the entire message.
        /// </summary>
        /// <param name="messageId">Unique identifier for the message.</param>
        /// <param name="creationDateTime">Date and time when the message was created.</param>
        /// <param name="numberOfTransactions">Total number of transactions in the message.</param>
        /// <param name="controlSum">Total amount of all transactions in the message.</param>
        /// <param name="initiatingParty">Party that initiates the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100108Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            PartyIdentification43? initiatingParty = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));

            _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                InitgPty = initiatingParty
            };

            if (controlSum.HasValue)
            {
                _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = controlSum.Value;
                _document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified = true;
            }

            return this;
        }

        /// <summary>
        /// Adds authorization information to the group header.
        /// </summary>
        /// <param name="authorization">Authorization information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when authorization is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when group header is not set.</exception>
        public Pain00100108Builder AddAuthorization(Authorisation1Choice authorization)
        {
            ValidateParameter(authorization, nameof(authorization));

            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                throw new InvalidOperationException("Group header must be set before adding authorization. Call SetGroupHeader first.");

            _document.CstmrCdtTrfInitn.GrpHdr.Authstn.Add(authorization);
            return this;
        }

        /// <summary>
        /// Sets the forwarding agent for the group header.
        /// </summary>
        /// <param name="forwardingAgent">Financial institution that forwards the message.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when forwardingAgent is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when group header is not set.</exception>
        public Pain00100108Builder SetForwardingAgent(BranchAndFinancialInstitutionIdentification5 forwardingAgent)
        {
            ValidateParameter(forwardingAgent, nameof(forwardingAgent));

            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                throw new InvalidOperationException("Group header must be set before setting forwarding agent. Call SetGroupHeader first.");

            _document.CstmrCdtTrfInitn.GrpHdr.FwdgAgt = forwardingAgent;
            return this;
        }

        /// <summary>
        /// Adds a payment instruction to the message.
        /// Each payment instruction contains common information for a group of transactions.
        /// </summary>
        /// <param name="paymentInformationId">Unique identifier for the payment instruction.</param>
        /// <param name="paymentMethod">Payment method used for the instruction.</param>
        /// <param name="batchBooking">Indicates whether individual transactions should be grouped for booking.</param>
        /// <param name="numberOfTransactions">Number of transactions in this instruction.</param>
        /// <param name="controlSum">Total amount of all transactions in this instruction.</param>
        /// <param name="paymentTypeInformation">Payment type information.</param>
        /// <param name="requestedExecutionDate">Date when the payment should be executed.</param>
        /// <param name="debtor">Party that owes money to the creditor.</param>
        /// <param name="debtorAccount">Account to be debited.</param>
        /// <param name="debtorAgent">Financial institution servicing the debtor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100108Builder AddPaymentInstruction(
            string paymentInformationId,
            PaymentMethod3Code paymentMethod,
            bool? batchBooking,
            string numberOfTransactions,
            decimal? controlSum,
            PaymentTypeInformation19? paymentTypeInformation,
            DateTime requestedExecutionDate,
            PartyIdentification43 debtor,
            CashAccount24 debtorAccount,
            BranchAndFinancialInstitutionIdentification5 debtorAgent)
        {
            ValidateParameter(paymentInformationId, nameof(paymentInformationId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));
            ValidateParameter(debtor, nameof(debtor));
            ValidateParameter(debtorAccount, nameof(debtorAccount));
            ValidateParameter(debtorAgent, nameof(debtorAgent));

            var paymentInstruction = new PaymentInstruction22
            {
                PmtInfId = paymentInformationId,
                PmtMtd = paymentMethod,
                NbOfTxs = numberOfTransactions,
                PmtTpInf = paymentTypeInformation,
                ReqdExctnDt = new DateAndDateTimeChoice 
                { 
                    Dt = requestedExecutionDate.Date, 
                    DtSpecified = true 
                },
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
        /// Sets the charge bearer for the last payment instruction.
        /// </summary>
        /// <param name="chargeBearer">Specifies which party will bear the charges.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100108Builder SetChargeBearer(ChargeBearerType1Code chargeBearer)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set charge bearer without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.ChrgBr = chargeBearer;
            lastPaymentInstruction.ChrgBrSpecified = true;
            return this;
        }

        /// <summary>
        /// Sets the debtor agent account for the last payment instruction.
        /// </summary>
        /// <param name="debtorAgentAccount">Account of the debtor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when debtorAgentAccount is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100108Builder SetDebtorAgentAccount(CashAccount24 debtorAgentAccount)
        {
            ValidateParameter(debtorAgentAccount, nameof(debtorAgentAccount));

            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set debtor agent account without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.DbtrAgtAcct = debtorAgentAccount;
            return this;
        }

        /// <summary>
        /// Sets the ultimate debtor for the last payment instruction.
        /// </summary>
        /// <param name="ultimateDebtor">Ultimate party that owes money to the creditor.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when ultimateDebtor is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100108Builder SetUltimateDebtor(PartyIdentification43 ultimateDebtor)
        {
            ValidateParameter(ultimateDebtor, nameof(ultimateDebtor));

            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set ultimate debtor without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.UltmtDbtr = ultimateDebtor;
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
        public Pain00100108Builder AddCreditTransferTransaction(CreditTransferTransaction26 creditTransfer)
        {
            ValidateParameter(creditTransfer, nameof(creditTransfer));

            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
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
        /// <param name="instructionId">Optional instruction identifier.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100108Builder AddCreditTransferTransaction(
            string endToEndId,
            AmountType4Choice amount,
            PartyIdentification43 creditor,
            CashAccount24 creditorAccount,
            string? instructionId = null)
        {
            ValidateParameter(endToEndId, nameof(endToEndId));
            ValidateParameter(amount, nameof(amount));
            ValidateParameter(creditor, nameof(creditor));
            ValidateParameter(creditorAccount, nameof(creditorAccount));

            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction. Add a payment instruction first.");

            var paymentId = new PaymentIdentification1 { EndToEndId = endToEndId };
            if (!string.IsNullOrEmpty(instructionId))
            {
                paymentId.InstrId = instructionId;
            }

            var transaction = new CreditTransferTransaction26
            {
                PmtId = paymentId,
                Amt = amount,
                Cdtr = creditor,
                CdtrAcct = creditorAccount
            };

            return AddCreditTransferTransaction(transaction);
        }

        /// <summary>
        /// Sets the creditor agent (receiving financial institution) for the last credit transfer transaction.
        /// </summary>
        /// <param name="creditorAgent">Financial institution servicing the creditor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when creditorAgent is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetCreditorAgent(BranchAndFinancialInstitutionIdentification5 creditorAgent)
        {
            ValidateParameter(creditorAgent, nameof(creditorAgent));

            var lastTransaction = GetLastTransaction();
            lastTransaction.CdtrAgt = creditorAgent;
            return this;
        }

        /// <summary>
        /// Sets the creditor agent account for the last credit transfer transaction.
        /// </summary>
        /// <param name="creditorAgentAccount">Account of the creditor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when creditorAgentAccount is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetCreditorAgentAccount(CashAccount24 creditorAgentAccount)
        {
            ValidateParameter(creditorAgentAccount, nameof(creditorAgentAccount));

            var lastTransaction = GetLastTransaction();
            lastTransaction.CdtrAgtAcct = creditorAgentAccount;
            return this;
        }

        /// <summary>
        /// Sets the ultimate creditor for the last credit transfer transaction.
        /// </summary>
        /// <param name="ultimateCreditor">Ultimate party that receives the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when ultimateCreditor is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetUltimateCreditor(PartyIdentification43 ultimateCreditor)
        {
            ValidateParameter(ultimateCreditor, nameof(ultimateCreditor));

            var lastTransaction = GetLastTransaction();
            lastTransaction.UltmtCdtr = ultimateCreditor;
            return this;
        }

        /// <summary>
        /// Sets the intermediary agent 1 for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent1">First intermediary financial institution.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent1 is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetIntermediaryAgent1(BranchAndFinancialInstitutionIdentification5 intermediaryAgent1)
        {
            ValidateParameter(intermediaryAgent1, nameof(intermediaryAgent1));

            var lastTransaction = GetLastTransaction();
            lastTransaction.IntrmyAgt1 = intermediaryAgent1;
            return this;
        }

        /// <summary>
        /// Sets the intermediary agent 1 account for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent1Account">Account of the first intermediary agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent1Account is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetIntermediaryAgent1Account(CashAccount24 intermediaryAgent1Account)
        {
            ValidateParameter(intermediaryAgent1Account, nameof(intermediaryAgent1Account));

            var lastTransaction = GetLastTransaction();
            lastTransaction.IntrmyAgt1Acct = intermediaryAgent1Account;
            return this;
        }

        /// <summary>
        /// Sets the intermediary agent 2 for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent2">Second intermediary financial institution.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent2 is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetIntermediaryAgent2(BranchAndFinancialInstitutionIdentification5 intermediaryAgent2)
        {
            ValidateParameter(intermediaryAgent2, nameof(intermediaryAgent2));

            var lastTransaction = GetLastTransaction();
            lastTransaction.IntrmyAgt2 = intermediaryAgent2;
            return this;
        }

        /// <summary>
        /// Sets the intermediary agent 2 account for the last credit transfer transaction.
        /// </summary>
        /// <param name="intermediaryAgent2Account">Account of the second intermediary agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when intermediaryAgent2Account is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetIntermediaryAgent2Account(CashAccount24 intermediaryAgent2Account)
        {
            ValidateParameter(intermediaryAgent2Account, nameof(intermediaryAgent2Account));

            var lastTransaction = GetLastTransaction();
            lastTransaction.IntrmyAgt2Acct = intermediaryAgent2Account;
            return this;
        }

        /// <summary>
        /// Sets the remittance information for the last credit transfer transaction.
        /// </summary>
        /// <param name="remittanceInformation">Information about the remittance, such as invoice details.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when remittanceInformation is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetRemittanceInformation(RemittanceInformation11 remittanceInformation)
        {
            ValidateParameter(remittanceInformation, nameof(remittanceInformation));

            var lastTransaction = GetLastTransaction();
            lastTransaction.RmtInf = remittanceInformation;
            return this;
        }

        /// <summary>
        /// Adds unstructured remittance information to the last credit transfer transaction.
        /// </summary>
        /// <param name="unstructuredText">Unstructured remittance information text.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when unstructuredText is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder AddUnstructuredRemittance(string unstructuredText)
        {
            ValidateParameter(unstructuredText, nameof(unstructuredText));

            var lastTransaction = GetLastTransaction();
            
            if (lastTransaction.RmtInf == null)
            {
                lastTransaction.RmtInf = new RemittanceInformation11();
            }

            lastTransaction.RmtInf.Ustrd.Add(unstructuredText);
            return this;
        }

        /// <summary>
        /// Sets the purpose of the last credit transfer transaction.
        /// </summary>
        /// <param name="purpose">Purpose of the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when purpose is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder SetPurpose(Purpose2Choice purpose)
        {
            ValidateParameter(purpose, nameof(purpose));

            var lastTransaction = GetLastTransaction();
            lastTransaction.Purp = purpose;
            return this;
        }

        /// <summary>
        /// Adds instructions for the creditor agent to the last credit transfer transaction.
        /// </summary>
        /// <param name="instruction">Instruction for the creditor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when instruction is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100108Builder AddInstructionForCreditorAgent(InstructionForCreditorAgent1 instruction)
        {
            ValidateParameter(instruction, nameof(instruction));

            var lastTransaction = GetLastTransaction();
            lastTransaction.InstrForCdtrAgt.Add(instruction);
            return this;
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="supplementaryData">Additional data related to the message.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        public Pain00100108Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            ValidateParameter(supplementaryData, nameof(supplementaryData));

            _document.CstmrCdtTrfInitn.SplmtryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Updates the group header with calculated totals from all payment instructions.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when group header is not set.</exception>
        public Pain00100108Builder UpdateGroupHeaderTotals()
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                throw new InvalidOperationException("Group header must be set before updating totals. Call SetGroupHeader first.");

            var totalTransactions = 0;
            var totalAmount = 0m;

            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                totalTransactions += paymentInstruction.CdtTrfTxInf.Count;

                foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                {
                    if (transaction.Amt?.InstdAmt != null)
                    {
                        totalAmount += transaction.Amt.InstdAmt.Value;
                    }
                    else if (transaction.Amt?.EqvtAmt?.Amt != null)
                    {
                        totalAmount += transaction.Amt.EqvtAmt.Amt.Value;
                    }
                }
            }

            _document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs = totalTransactions.ToString();
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = totalAmount;
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified = true;

            return this;
        }

        /// <summary>
        /// Gets the number of payment instructions in the message.
        /// </summary>
        /// <returns>The number of payment instructions.</returns>
        public int GetPaymentInstructionCount()
        {
            return _document.CstmrCdtTrfInitn.PmtInf?.Count ?? 0;
        }

        /// <summary>
        /// Gets the total number of transactions across all payment instructions.
        /// </summary>
        /// <returns>The total number of transactions.</returns>
        public int GetTotalTransactionCount()
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null)
                return 0;

            var total = 0;
            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                total += paymentInstruction.CdtTrfTxInf?.Count ?? 0;
            }
            return total;
        }

        /// <summary>
        /// Creates a new instance of the builder with the same configuration.
        /// </summary>
        /// <returns>A new Pain00100108Builder instance.</returns>
        public Pain00100108Builder Clone()
        {
            var clone = new Pain00100108Builder();
            
            // Note: This is a shallow clone for demonstration purposes.
            // For production use, implement deep cloning as needed.
            if (_document.CstmrCdtTrfInitn.GrpHdr != null)
            {
                clone._document.CstmrCdtTrfInitn.GrpHdr = _document.CstmrCdtTrfInitn.GrpHdr;
            }

            foreach (var pmtInf in _document.CstmrCdtTrfInitn.PmtInf)
            {
                clone._document.CstmrCdtTrfInitn.PmtInf.Add(pmtInf);
            }

            foreach (var supplementaryData in _document.CstmrCdtTrfInitn.SplmtryData)
            {
                clone._document.CstmrCdtTrfInitn.SplmtryData.Add(supplementaryData);
            }

            return clone;
        }

        /// <summary>
        /// Resets the builder to its initial state.
        /// </summary>
        /// <returns>A new Pain00100108Builder instance.</returns>
        public Pain00100108Builder Reset()
        {
            return new Pain00100108Builder();
        }

        /// <summary>
        /// Builds and returns the completed message document.
        /// </summary>
        /// <returns>The completed ISO 20022 pain.001.001.08 document.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required components are missing.</exception>
        public Document Build()
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                throw new InvalidOperationException("Group header is required. Call SetGroupHeader before building.");

            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("At least one payment instruction is required. Call AddPaymentInstruction before building.");

            return _document;
        }

        /// <summary>
        /// Builds and serializes the message to XML format.
        /// </summary>
        /// <returns>XML representation of the pain.001.001.08 message.</returns>
        public string BuildXml()
        {
            return XmlSerializationService.Serialize(Build());
        }

        /// <summary>
        /// Builds and serializes the provided message to XML format.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns>XML representation of the message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="InvalidCastException">Thrown when message is not a valid Document.</exception>
        public string BuildXml(object message)
        {
            ValidateParameter(message, nameof(message));

            if (message is not Document doc)
                throw new InvalidCastException($"Expected message of type {typeof(Document).Name}, but received {message.GetType().Name}");

            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Gets the message type identifier for this builder.
        /// </summary>
        /// <returns>The message type identifier "pain.001.001.08".</returns>
        public string GetMessageType() => "pain.001.001.08";

        /// <summary>
        /// Validates that a parameter is not null or empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter for error reporting.</param>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the parameter is an empty string.</exception>
        private static void ValidateParameter(object? parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);

            if (parameter is string str && string.IsNullOrWhiteSpace(str))
                throw new ArgumentException($"Parameter '{parameterName}' cannot be null or empty.", parameterName);
        }

        /// <summary>
        /// Gets the last credit transfer transaction from the last payment instruction.
        /// </summary>
        /// <returns>The last credit transfer transaction.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        private CreditTransferTransaction26 GetLastTransaction()
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("No payment instructions exist. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];

            if (lastPaymentInstruction.CdtTrfTxInf == null || lastPaymentInstruction.CdtTrfTxInf.Count == 0)
                throw new InvalidOperationException("No credit transfer transactions exist in the last payment instruction. Add a transaction first.");

            return lastPaymentInstruction.CdtTrfTxInf[lastPaymentInstruction.CdtTrfTxInf.Count - 1];
        }
    }
}
