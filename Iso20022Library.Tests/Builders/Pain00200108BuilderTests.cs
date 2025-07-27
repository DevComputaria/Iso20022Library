#nullable disable
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200108;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00200108Builder class.
    /// Tests the creation and validation of Pain.002.001.08 (Customer Payment Status Report V08) messages.
    /// </summary>
    [TestClass]
    public class Pain00200108BuilderTests
    {
        private Pain00200108Builder _builder;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00200108Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeBuilderCorrectly()
        {
            // Arrange & Act
            var builder = new Pain00200108Builder();

            // Assert
            Assert.IsNotNull(builder);
        }

        [TestMethod]
        public void SetMessageIdentification_WithValidParameters_ShouldSetGroupHeaderCorrectly()
        {
            // Arrange
            const string messageId = "MSG-20240115-001";
            var creationDateTime = new DateTime(2024, 1, 15, 14, 30, 0);

            // Act
            var result = _builder
                .SetMessageIdentification(messageId, creationDateTime)
                .SetOriginalGroupInformation("ORIG-MSG-001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");
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

        [TestMethod]
        public void SetInitiatingParty_WithValidParameters_ShouldSetInitiatingPartyCorrectly()
        {
            // Arrange
            const string partyName = "ABC Bank Ltd";
            const string bicfi = "ABCDEFGH";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
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
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
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

        [TestMethod]
        public void SetForwardingAgent_WithValidParameters_ShouldSetForwardingAgentCorrectly()
        {
            // Arrange
            const string agentName = "Central Bank";
            const string bicfi = "CBANKXXX";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
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

        [TestMethod]
        public void SetOriginalGroupInformation_WithValidParameters_ShouldSetOriginalGroupInfoCorrectly()
        {
            // Arrange
            const string originalMessageId = "ORIG-MSG-001";
            var originalCreationDateTime = new DateTime(2024, 1, 14, 10, 0, 0);
            const string numberOfTransactions = "5";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetOriginalGroupInformation(originalMessageId, originalCreationDateTime, numberOfTransactions);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(originalMessageId, report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual(originalCreationDateTime, report.OrgnlGrpInfAndSts.OrgnlCreDtTm);
            Assert.IsTrue(report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified);
            Assert.AreEqual(numberOfTransactions, report.OrgnlGrpInfAndSts.OrgnlNbOfTxs);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithoutNumberOfTransactions_ShouldSetRequiredFieldsOnly()
        {
            // Arrange
            const string originalMessageId = "ORIG-MSG-002";
            var originalCreationDateTime = new DateTime(2024, 1, 14, 10, 0, 0);

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .SetOriginalGroupInformation(originalMessageId, originalCreationDateTime);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(originalMessageId, report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual(originalCreationDateTime, report.OrgnlGrpInfAndSts.OrgnlCreDtTm);
            Assert.IsTrue(report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified);
            Assert.IsNull(report.OrgnlGrpInfAndSts.OrgnlNbOfTxs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupInformation(null, DateTime.Now);
        }

        [TestMethod]
        public void SetOriginalGroupStatus_WithValidStatus_ShouldSetStatusCorrectly()
        {
            // Arrange
            const string groupStatus = "ACTC";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
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

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithValidParameters_ShouldAddInstructionCorrectly()
        {
            // Arrange
            const string instructionId = "PMT-INST-001";
            const string paymentStatus = "ACCP";
            const string numberOfTransactions = "3";
            const decimal controlSum = 1500.50m;

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
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
            const string paymentStatus = "RJCT";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
                .AddOriginalPaymentInstruction(instructionId, paymentStatus);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.OrgnlPmtInfAndSts.Count);

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(instructionId, instruction.OrgnlPmtInfId);
            Assert.AreEqual(paymentStatus, instruction.PmtInfSts);
            Assert.IsNull(instruction.OrgnlNbOfTxs);
            Assert.IsFalse(instruction.OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOriginalPaymentInstruction_WithNullInstructionId_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction(null, "ACCP");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOriginalPaymentInstruction_WithNullPaymentStatus_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction("PMT-001", null);
        }

        [TestMethod]
        public void AddPaymentTransaction_WithValidParameters_ShouldAddTransactionCorrectly()
        {
            // Arrange
            const string originalEndToEndId = "TXN-001";
            const string transactionStatus = "ACSC";
            const string originalTransactionId = "ORIG-TXN-001";

            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            var result = _builder.AddPaymentTransaction(originalEndToEndId, transactionStatus, originalTransactionId);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(1, instruction.TxInfAndSts.Count);

            var transaction = instruction.TxInfAndSts[0];
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(transactionStatus, transaction.TxSts);
            Assert.AreEqual(originalTransactionId, transaction.OrgnlInstrId);
        }

        [TestMethod]
        public void AddPaymentTransaction_WithRequiredParametersOnly_ShouldAddTransactionCorrectly()
        {
            // Arrange
            const string originalEndToEndId = "TXN-002";
            const string transactionStatus = "PDNG";

            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            var result = _builder.AddPaymentTransaction(originalEndToEndId, transactionStatus);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");

            var instruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual(1, instruction.TxInfAndSts.Count);

            var transaction = instruction.TxInfAndSts[0];
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(transactionStatus, transaction.TxSts);
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
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.AddPaymentTransaction(null, "ACSC");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPaymentTransaction_WithNullTransactionStatus_ShouldThrowException()
        {
            // Arrange
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.AddPaymentTransaction("TXN-001", null);
        }

        [TestMethod]
        public void AddTransactionStatusReason_WithValidParameters_ShouldAddReasonCorrectly()
        {
            // Arrange
            const string reasonCode = "AC04";
            const string additionalInfo = "Insufficient funds";

            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");
            _builder.AddPaymentTransaction("TXN-001", "RJCT");

            // Act
            var result = _builder.AddTransactionStatusReason(reasonCode, additionalInfo);
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

            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT-001", "ACCP");
            _builder.AddPaymentTransaction("TXN-001", "RJCT");

            // Act
            var result = _builder.AddTransactionStatusReason(reasonCode);
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
        public void AddTransactionStatusReason_WithoutPaymentTransaction_ShouldThrowException()
        {
            // Arrange
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.AddTransactionStatusReason("AC04");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTransactionStatusReason_WithNullReasonCode_ShouldThrowException()
        {
            // Arrange
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");
            _builder.AddPaymentTransaction("TXN-001", "RJCT");

            // Act
            _builder.AddTransactionStatusReason(null);
        }

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
                .SetOriginalGroupInformation("GRP001", DateTime.Now)
                .AddOriginalPaymentInstruction("PMT001", "ACCP")
                .AddSupplementaryData(supplementaryData);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(1, report.SplmtryData.Count);
            Assert.AreSame(supplementaryData, report.SplmtryData[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullData_ShouldThrowException()
        {
            // Act
            _builder.AddSupplementaryData(null);
        }

        [TestMethod]
        public void Build_WithValidData_ShouldReturnCompleteMessage()
        {
            // Arrange
            SetupValidMessage();

            // Act
            var result = _builder.Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerPaymentStatusReportV08));
            Assert.IsNotNull(result.GrpHdr);
            Assert.IsNotNull(result.OrgnlGrpInfAndSts);
            Assert.IsTrue(result.OrgnlPmtInfAndSts.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutMessageId_ShouldThrowException()
        {
            // Arrange
            _builder.SetOriginalGroupInformation("ORIG-001", DateTime.Now);
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalMessageId_ShouldThrowException()
        {
            // Arrange
            _builder.SetMessageIdentification("MSG-001", DateTime.Now);
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutPaymentInstructions_ShouldThrowException()
        {
            // Arrange
            _builder.SetMessageIdentification("MSG-001", DateTime.Now);
            _builder.SetOriginalGroupInformation("ORIG-001", DateTime.Now);

            // Act
            _builder.Build();
        }

        [TestMethod]
        public void BuildXml_WithValidData_ShouldReturnXmlString()
        {
            // Arrange
            SetupValidMessage();

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrPmtStsRpt"));
            Assert.IsTrue(xml.Contains("MSG-001"));
            Assert.IsTrue(xml.Contains("ORIG-001"));
        }

        [TestMethod]
        public void SaveToFile_WithValidPath_ShouldCreateFile()
        {
            // Arrange
            SetupValidMessage();
            var tempPath = System.IO.Path.GetTempFileName();

            try
            {
                // Act
                _builder.SaveToFile(tempPath);

                // Assert
                Assert.IsTrue(System.IO.File.Exists(tempPath));
                var content = System.IO.File.ReadAllText(tempPath);
                Assert.IsTrue(content.Contains("<?xml"));
                Assert.IsTrue(content.Contains("CstmrPmtStsRpt"));
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveToFile_WithNullPath_ShouldThrowException()
        {
            // Arrange
            SetupValidMessage();

            // Act
            _builder.SaveToFile(null);
        }

        [TestMethod]
        public void ComplexMessage_WithMultipleInstructionsAndTransactions_ShouldBuildCorrectly()
        {
            // Arrange
            _builder.SetMessageIdentification("COMPLEX-MSG-001", new DateTime(2024, 1, 15, 15, 30, 0));
            _builder.SetInitiatingParty("Complex Bank Ltd", "CMPLXYYY");
            _builder.SetForwardingAgent("Central Processing Bank", "CPBANKZZ");
            _builder.SetOriginalGroupInformation("ORIG-COMPLEX-001", new DateTime(2024, 1, 14, 10, 0, 0), "10");
            _builder.SetOriginalGroupStatus("PART");

            // Add first payment instruction with transactions
            _builder.AddOriginalPaymentInstruction("PMT-INST-001", "ACCP", "5", 2500.00m);
            _builder.AddPaymentTransaction("TXN-001", "ACSC", "ORIG-TXN-001");
            _builder.AddTransactionStatusReason("AC01", "Account identifier invalid");
            _builder.AddPaymentTransaction("TXN-002", "ACSP");
            _builder.AddPaymentTransaction("TXN-003", "RJCT", "ORIG-TXN-003");
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
            Assert.AreEqual("ORIG-COMPLEX-001", report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual("PART", report.OrgnlGrpInfAndSts.GrpSts);

            Assert.AreEqual(2, report.OrgnlPmtInfAndSts.Count);

            // Verify first payment instruction
            var firstInstruction = report.OrgnlPmtInfAndSts[0];
            Assert.AreEqual("PMT-INST-001", firstInstruction.OrgnlPmtInfId);
            Assert.AreEqual("ACCP", firstInstruction.PmtInfSts);
            Assert.AreEqual(3, firstInstruction.TxInfAndSts.Count);

            // Verify first transaction with status reason
            var firstTransaction = firstInstruction.TxInfAndSts[0];
            Assert.AreEqual("TXN-001", firstTransaction.OrgnlEndToEndId);
            Assert.AreEqual("ACSC", firstTransaction.TxSts);
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

        /// <summary>
        /// Helper method to set up a valid message for testing.
        /// </summary>
        private void SetupValidMessage()
        {
            _builder.SetMessageIdentification("MSG-001", DateTime.Now);
            _builder.SetOriginalGroupInformation("ORIG-001", DateTime.Now);
            _builder.AddOriginalPaymentInstruction("PMT-001", "ACCP");
        }
    }
}