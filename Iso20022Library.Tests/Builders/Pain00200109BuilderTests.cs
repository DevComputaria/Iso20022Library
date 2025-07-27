using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200109;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00200109Builder class.
    /// Tests the functionality of building Pain.002.001.09 (Customer Payment Status Report V09) messages.
    /// </summary>
    [TestClass]
    public class Pain00200109BuilderTests
    {
        private Pain00200109Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00200109Builder();
        }

        #region Basic Builder Tests

        [TestMethod]
        public void Build_WithMinimalRequiredData_ShouldBuildSuccessfully()
        {
            // Arrange & Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-INST-001")
                .Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GrpHdr);
            Assert.IsNotNull(result.OrgnlGrpInfAndSts);
            Assert.AreEqual(1, result.OrgnlPmtInfAndSts.Count);
        }

        [TestMethod]
        public void BuildXml_WithValidData_ShouldReturnXmlString()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-INST-001");

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrPmtStsRpt"));
        }

        #endregion

        #region SetMessageIdentification Tests

        [TestMethod]
        public void SetMessageIdentification_WithValidParameters_ShouldSetCorrectly()
        {
            // Arrange
            const string messageId = "MSG-001";
            var creationDateTime = new DateTime(2024, 1, 14, 10, 0, 0);

            // Act
            var result = _builder
                .SetMessageIdentification(messageId, creationDateTime)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-INST-001");
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(messageId, report.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, report.GrpHdr.CreDtTm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMessageIdentification_WithNullMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetMessageIdentification(null, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMessageIdentification_WithEmptyMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetMessageIdentification(string.Empty, DateTime.Now);
        }

        #endregion

        #region SetInitiatingParty Tests

        [TestMethod]
        public void SetInitiatingParty_WithValidParameters_ShouldSetInitiatingPartyCorrectly()
        {
            // Arrange
            const string partyName = "ABC Bank Ltd";
            const string bicfi = "ABCDEFGH";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetInitiatingParty(partyName, bicfi);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(partyName, report.GrpHdr.InitgPty.Nm);
            Assert.IsNotNull(report.GrpHdr.InitgPty.Id);
            Assert.IsNotNull(report.GrpHdr.InitgPty.Id.OrgId);
            Assert.AreEqual(bicfi, report.GrpHdr.InitgPty.Id.OrgId.AnyBic);
        }

        [TestMethod]
        public void SetInitiatingParty_WithoutBicfi_ShouldSetNameOnly()
        {
            // Arrange
            const string partyName = "XYZ Corporation";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetInitiatingParty(partyName);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(partyName, report.GrpHdr.InitgPty.Nm);
            Assert.IsNull(report.GrpHdr.InitgPty.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetInitiatingParty_WithNullPartyName_ShouldThrowException()
        {
            // Act
            _builder.SetInitiatingParty(null, "BICFI123");
        }

        #endregion

        #region SetForwardingAgent Tests

        [TestMethod]
        public void SetForwardingAgent_WithValidParameters_ShouldSetForwardingAgentCorrectly()
        {
            // Arrange
            const string agentName = "Central Bank";
            const string bicfi = "CBANKXXX";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetForwardingAgent(agentName, bicfi);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.IsNotNull(report.GrpHdr.FwdgAgt);
            Assert.AreEqual(agentName, report.GrpHdr.FwdgAgt.FinInstnId.Nm);
            Assert.AreEqual(bicfi, report.GrpHdr.FwdgAgt.FinInstnId.Bicfi);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetForwardingAgent_WithNullAgentName_ShouldThrowException()
        {
            // Act
            _builder.SetForwardingAgent(null, "BICFI123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetForwardingAgent_WithNullBicfi_ShouldThrowException()
        {
            // Act
            _builder.SetForwardingAgent("Agent Name", null);
        }

        #endregion

        #region SetDebtorAgent Tests

        [TestMethod]
        public void SetDebtorAgent_WithValidParameters_ShouldSetDebtorAgentCorrectly()
        {
            // Arrange
            const string agentName = "Debtor Bank";
            const string bicfi = "DEBTORXX";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetDebtorAgent(agentName, bicfi);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.IsNotNull(report.GrpHdr.DbtrAgt);
            Assert.AreEqual(agentName, report.GrpHdr.DbtrAgt.FinInstnId.Nm);
            Assert.AreEqual(bicfi, report.GrpHdr.DbtrAgt.FinInstnId.Bicfi);
        }

        #endregion

        #region SetCreditorAgent Tests

        [TestMethod]
        public void SetCreditorAgent_WithValidParameters_ShouldSetCreditorAgentCorrectly()
        {
            // Arrange
            const string agentName = "Creditor Bank";
            const string bicfi = "CREDTRXX";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetCreditorAgent(agentName, bicfi);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.IsNotNull(report.GrpHdr.CdtrAgt);
            Assert.AreEqual(agentName, report.GrpHdr.CdtrAgt.FinInstnId.Nm);
            Assert.AreEqual(bicfi, report.GrpHdr.CdtrAgt.FinInstnId.Bicfi);
        }

        #endregion

        #region SetOriginalGroupInformation Tests

        [TestMethod]
        public void SetOriginalGroupInformation_WithAllParameters_ShouldSetOriginalGroupInfoCorrectly()
        {
            // Arrange
            const string originalMessageId = "ORIG-MSG-001";
            const string originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = new DateTime(2024, 1, 14, 10, 0, 0);
            const string numberOfTransactions = "5";
            const decimal controlSum = 1000.50m;

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime, numberOfTransactions, controlSum);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(originalMessageId, report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, report.OrgnlGrpInfAndSts.OrgnlMsgNmId);
            Assert.AreEqual(originalCreationDateTime, report.OrgnlGrpInfAndSts.OrgnlCreDtTm);
            Assert.IsTrue(report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified);
            Assert.AreEqual(numberOfTransactions, report.OrgnlGrpInfAndSts.OrgnlNbOfTxs);
            Assert.AreEqual(controlSum, report.OrgnlGrpInfAndSts.OrgnlCtrlSum);
            Assert.IsTrue(report.OrgnlGrpInfAndSts.OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithRequiredParametersOnly_ShouldSetRequiredFieldsOnly()
        {
            // Arrange
            const string originalMessageId = "ORIG-MSG-002";
            const string originalMessageNameId = "pain.001.001.03";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetOriginalGroupInformation(originalMessageId, originalMessageNameId);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(originalMessageId, report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, report.OrgnlGrpInfAndSts.OrgnlMsgNmId);
            Assert.IsFalse(report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified);
            Assert.IsNull(report.OrgnlGrpInfAndSts.OrgnlNbOfTxs);
            Assert.IsFalse(report.OrgnlGrpInfAndSts.OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupInformation(null, "pain.001.001.03");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageNameId_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupInformation("ORIG-MSG-001", null);
        }

        #endregion

        #region SetOriginalGroupStatus Tests

        [TestMethod]
        public void SetOriginalGroupStatus_WithValidStatus_ShouldSetStatusCorrectly()
        {
            // Arrange
            const string groupStatus = "ACTC";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetOriginalGroupStatus(groupStatus);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(groupStatus, report.OrgnlGrpInfAndSts.GrpSts);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetOriginalGroupStatus_WithNullStatus_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupStatus(null);
        }

        #endregion

        #region AddOriginalGroupStatusReason Tests

        [TestMethod]
        public void AddOriginalGroupStatusReason_WithValidParameters_ShouldAddReasonCorrectly()
        {
            // Arrange
            const string reasonCode = "AC04";
            const string additionalInfo = "Insufficient funds";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .AddOriginalGroupStatusReason(reasonCode, additionalInfo);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.OrgnlGrpInfAndSts.StsRsnInf.Count);

            var statusReason = report.OrgnlGrpInfAndSts.StsRsnInf[0];
            Assert.AreEqual(reasonCode, statusReason.Rsn.Cd);
            Assert.AreEqual(1, statusReason.AddtlInf.Count);
            Assert.AreEqual(additionalInfo, statusReason.AddtlInf[0]);
        }

        [TestMethod]
        public void AddOriginalGroupStatusReason_WithoutAdditionalInfo_ShouldAddReasonCodeOnly()
        {
            // Arrange
            const string reasonCode = "MS03";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .AddOriginalGroupStatusReason(reasonCode);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.OrgnlGrpInfAndSts.StsRsnInf.Count);

            var statusReason = report.OrgnlGrpInfAndSts.StsRsnInf[0];
            Assert.AreEqual(reasonCode, statusReason.Rsn.Cd);
            Assert.AreEqual(0, statusReason.AddtlInf.Count);
        }

        #endregion

        #region AddOriginalPaymentInstruction Tests

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithAllParameters_ShouldAddInstructionCorrectly()
        {
            // Arrange
            const string instructionId = "PMT-INST-001";
            const string paymentStatus = "ACCP";
            const string numberOfTransactions = "3";
            const decimal controlSum = 1500.50m;

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction(instructionId, paymentStatus, numberOfTransactions, controlSum);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.OrgnlPmtInfAndSts.Count);

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(instructionId, instruction.OrgnlPmtInfId);
            Assert.AreEqual(paymentStatus, instruction.PmtInfSts);
            Assert.AreEqual(numberOfTransactions, instruction.OrgnlNbOfTxs);
            Assert.AreEqual(controlSum, instruction.OrgnlCtrlSum);
            Assert.IsTrue(instruction.OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithRequiredParametersOnly_ShouldAddInstructionCorrectly()
        {
            // Arrange
            const string instructionId = "PMT-INST-002";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction(instructionId);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.OrgnlPmtInfAndSts.Count);

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(instructionId, instruction.OrgnlPmtInfId);
            Assert.IsNull(instruction.PmtInfSts);
            Assert.IsNull(instruction.OrgnlNbOfTxs);
            Assert.IsFalse(instruction.OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOriginalPaymentInstruction_WithNullInstructionId_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction(null);
        }

        #endregion

        #region AddPaymentInstructionStatusReason Tests

        [TestMethod]
        public void AddPaymentInstructionStatusReason_WithValidParameters_ShouldAddReasonCorrectly()
        {
            // Arrange
            const string reasonCode = "AC04";
            const string additionalInfo = "Insufficient funds";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-INST-001", "RJCT")
                .AddPaymentInstructionStatusReason(reasonCode, additionalInfo);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(1, instruction.StsRsnInf.Count);

            var statusReason = instruction.StsRsnInf[0];
            Assert.AreEqual(reasonCode, statusReason.Rsn.Cd);
            Assert.AreEqual(1, statusReason.AddtlInf.Count);
            Assert.AreEqual(additionalInfo, statusReason.AddtlInf[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentInstructionStatusReason_WithoutPaymentInstruction_ShouldThrowException()
        {
            // Act
            _builder.AddPaymentInstructionStatusReason("AC04");
        }

        #endregion

        #region AddPaymentTransaction Tests

        [TestMethod]
        public void AddPaymentTransaction_WithAllParameters_ShouldAddTransactionCorrectly()
        {
            // Arrange
            const string originalEndToEndId = "TXN-001";
            const string transactionStatus = "ACSC";
            const string statusId = "STS-001";
            const string originalInstructionId = "ORIG-INSTR-001";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP")
                .AddPaymentTransaction(originalEndToEndId, transactionStatus, statusId, originalInstructionId);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(1, instruction.TxInfAndSts.Count);

            var transaction = instruction.TxInfAndSts[0];
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(transactionStatus, transaction.TxSts);
            Assert.AreEqual(statusId, transaction.StsId);
            Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
        }

        [TestMethod]
        public void AddPaymentTransaction_WithRequiredParametersOnly_ShouldAddTransactionCorrectly()
        {
            // Arrange
            const string originalEndToEndId = "TXN-002";
            const string transactionStatus = "PDNG";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP")
                .AddPaymentTransaction(originalEndToEndId, transactionStatus);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(1, instruction.TxInfAndSts.Count);

            var transaction = instruction.TxInfAndSts[0];
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(transactionStatus, transaction.TxSts);
            Assert.IsNull(transaction.StsId);
            Assert.IsNull(transaction.OrgnlInstrId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentTransaction_WithoutPaymentInstruction_ShouldThrowException()
        {
            // Act
            _builder.AddPaymentTransaction("TXN-001", "ACSC");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPaymentTransaction_WithNullEndToEndId_ShouldThrowException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.AddPaymentTransaction(null, "ACSC");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPaymentTransaction_WithNullTransactionStatus_ShouldThrowException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.AddPaymentTransaction("TXN-001", null);
        }

        #endregion

        #region AddTransactionStatusReason Tests

        [TestMethod]
        public void AddTransactionStatusReason_WithValidParameters_ShouldAddReasonCorrectly()
        {
            // Arrange
            const string reasonCode = "AC04";
            const string additionalInfo = "Insufficient funds";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP")
                .AddPaymentTransaction("TXN-001", "RJCT")
                .AddTransactionStatusReason(reasonCode, additionalInfo);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var transaction = report.OrgnlPmtInfAndSts[0].TxInfAndSts[0];
            Assert.AreEqual(1, transaction.StsRsnInf.Count);

            var statusReason = transaction.StsRsnInf[0];
            Assert.AreEqual(reasonCode, statusReason.Rsn.Cd);
            Assert.AreEqual(1, statusReason.AddtlInf.Count);
            Assert.AreEqual(additionalInfo, statusReason.AddtlInf[0]);
        }

        [TestMethod]
        public void AddTransactionStatusReason_WithoutAdditionalInfo_ShouldAddReasonCodeOnly()
        {
            // Arrange
            const string reasonCode = "MS03";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP")
                .AddPaymentTransaction("TXN-001", "RJCT")
                .AddTransactionStatusReason(reasonCode);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var transaction = report.OrgnlPmtInfAndSts[0].TxInfAndSts[0];
            Assert.AreEqual(1, transaction.StsRsnInf.Count);

            var statusReason = transaction.StsRsnInf[0];
            Assert.AreEqual(reasonCode, statusReason.Rsn.Cd);
            Assert.AreEqual(0, statusReason.AddtlInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTransactionStatusReason_WithoutPaymentInstruction_ShouldThrowException()
        {
            // Act
            _builder.AddTransactionStatusReason("AC04");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTransactionStatusReason_WithoutTransaction_ShouldThrowException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.AddTransactionStatusReason("AC04");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTransactionStatusReason_WithNullReasonCode_ShouldThrowException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001", "ACCP")
                .AddPaymentTransaction("TXN-001", "RJCT");

            // Act
            _builder.AddTransactionStatusReason(null);
        }

        #endregion

        #region AddSupplementaryData Tests

        [TestMethod]
        public void AddSupplementaryData_WithValidData_ShouldAddDataCorrectly()
        {
            // Arrange
            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = "CustomData"
            };

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .AddSupplementaryData(supplementaryData);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.SplmtryData.Count);
            Assert.AreEqual("CustomData", report.SplmtryData[0].PlcAndNm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullData_ShouldThrowException()
        {
            // Act
            _builder.AddSupplementaryData(null);
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutMessageId_ShouldThrowValidationException()
        {
            // Arrange
            _builder
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-001");

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalMessageId_ShouldThrowValidationException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT-001");

            // Act
            _builder.Build();
        }

        [TestMethod] 
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutAnyPaymentInstructions_ShouldThrowValidationException()
        {
            // Arrange
            _builder.SetMessageIdentification("MSG001", DateTime.Now);
            _builder.SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03");
            // Don't add any payment instructions to cause validation error

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutPaymentInstructions_ShouldThrowValidationException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03");

            // Act
            _builder.Build();
        }

        #endregion

        #region Complex Integration Tests

        [TestMethod]
        public void ComplexMessage_WithMultipleInstructionsAndTransactions_ShouldBuildCorrectly()
        {
            // Arrange & Act
            _builder.SetMessageIdentification("COMPLEX-MSG-001", new DateTime(2024, 1, 15, 15, 30, 0));
            _builder.SetInitiatingParty("Complex Bank Ltd", "CMPLXYYY");
            _builder.SetForwardingAgent("Central Processing Bank", "CPBANKZZ");
            _builder.SetDebtorAgent("Debtor Processing Bank", "DEBTORZZ");
            _builder.SetCreditorAgent("Creditor Processing Bank", "CREDITZZ");
            _builder.SetOriginalGroupInformation("ORIG-COMPLEX-001", "pain.001.001.09", new DateTime(2024, 1, 14, 10, 0, 0), "10", 5000.00m);
            _builder.SetOriginalGroupStatus("PART");
            _builder.AddOriginalGroupStatusReason("AC01", "Account identifier invalid");

            // Add first payment instruction with transactions
            _builder.AddOriginalPaymentInstruction("PMT-INST-001", "ACCP", "5", 2500.00m);
            _builder.AddPaymentInstructionStatusReason("AC01", "Account identifier invalid");
            _builder.AddPaymentTransaction("TXN-001", "ACSC", "STS-001", "ORIG-INSTR-001");
            _builder.AddTransactionStatusReason("AC01", "Account identifier invalid");
            _builder.AddPaymentTransaction("TXN-002", "ACSP");
            _builder.AddPaymentTransaction("TXN-003", "RJCT", "STS-003", "ORIG-INSTR-003");
            _builder.AddTransactionStatusReason("AC04");

            // Add second payment instruction with transactions
            _builder.AddOriginalPaymentInstruction("PMT-INST-002", "RJCT", "5", 1800.75m);
            _builder.AddPaymentTransaction("TXN-004", "RJCT");
            _builder.AddTransactionStatusReason("MS03", "Reason not specified");
            _builder.AddPaymentTransaction("TXN-005", "RJCT");

            // Add supplementary data
            var supplementaryData = new SupplementaryData1 { PlcAndNm = "AdditionalProcessingInfo" };
            _builder.AddSupplementaryData(supplementaryData);

            // Act
            var report = _builder.Build();

            // Assert
            Assert.AreEqual("COMPLEX-MSG-001", report.GrpHdr.MsgId);
            Assert.AreEqual("Complex Bank Ltd", report.GrpHdr.InitgPty.Nm);
            Assert.AreEqual("Central Processing Bank", report.GrpHdr.FwdgAgt.FinInstnId.Nm);
            Assert.AreEqual("Debtor Processing Bank", report.GrpHdr.DbtrAgt.FinInstnId.Nm);
            Assert.AreEqual("Creditor Processing Bank", report.GrpHdr.CdtrAgt.FinInstnId.Nm);
            Assert.AreEqual("ORIG-COMPLEX-001", report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual("pain.001.001.09", report.OrgnlGrpInfAndSts.OrgnlMsgNmId);
            Assert.AreEqual("PART", report.OrgnlGrpInfAndSts.GrpSts);
            Assert.AreEqual(1, report.OrgnlGrpInfAndSts.StsRsnInf.Count);

            Assert.AreEqual(2, report.OrgnlPmtInfAndSts.Count);

            // Verify first payment instruction
            var firstInstruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual("PMT-INST-001", firstInstruction.OrgnlPmtInfId);
            Assert.AreEqual("ACCP", firstInstruction.PmtInfSts);
            Assert.AreEqual(1, firstInstruction.StsRsnInf.Count);
            Assert.AreEqual(3, firstInstruction.TxInfAndSts.Count);

            // Verify first transaction with status reason
            var firstTransaction = firstInstruction.TxInfAndSts[0];
            Assert.AreEqual("TXN-001", firstTransaction.OrgnlEndToEndId);
            Assert.AreEqual("ACSC", firstTransaction.TxSts);
            Assert.AreEqual("STS-001", firstTransaction.StsId);
            Assert.AreEqual("ORIG-INSTR-001", firstTransaction.OrgnlInstrId);
            Assert.AreEqual(1, firstTransaction.StsRsnInf.Count);
            Assert.AreEqual("AC01", firstTransaction.StsRsnInf[0].Rsn.Cd);

            // Verify second payment instruction
            var secondInstruction = report.OrgnlPmtInfAndSts[1];
            Assert.AreEqual("PMT-INST-002", secondInstruction.OrgnlPmtInfId);
            Assert.AreEqual("RJCT", secondInstruction.PmtInfSts);
            Assert.AreEqual(2, secondInstruction.TxInfAndSts.Count);

            // Verify supplementary data
            Assert.AreEqual(1, report.SplmtryData.Count);
            Assert.AreEqual("AdditionalProcessingInfo", report.SplmtryData[0].PlcAndNm);
        }

        [TestMethod]
        public void SaveToFile_WithValidData_ShouldCreateXmlFile()
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT-INST-001");

            try
            {
                // Act
                _builder.SaveToFile(tempFilePath);

                // Assert
                Assert.IsTrue(File.Exists(tempFilePath));
                var content = File.ReadAllText(tempFilePath);
                Assert.IsTrue(content.Contains("<?xml"));
                Assert.IsTrue(content.Contains("CstmrPmtStsRpt"));
            }
            finally
            {
                // Cleanup
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

        #endregion
    }
}

#pragma warning restore CS8625
