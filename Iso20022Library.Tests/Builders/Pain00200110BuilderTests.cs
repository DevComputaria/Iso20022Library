using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200110;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00200110Builder class.
    /// Tests the functionality of building Pain.002.001.10 (Customer Payment Status Report V10) messages.
    /// </summary>
    [TestClass]
    public class Pain00200110BuilderTests
    {
        private Pain00200110Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00200110Builder();
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
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\""));
            Assert.IsTrue(xml.Contains("MSG001"));
            Assert.IsTrue(xml.Contains("PMT-INST-001"));
        }

        [TestMethod]
        public void Build_WithoutMessageId_ShouldThrowInvalidOperationException()
        {
            // Arrange & Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => _builder.Build());
            Assert.AreEqual("Message ID is required. Use SetMessageIdentification method.", ex.Message);
        }

        [TestMethod]
        public void Build_WithoutOriginalGroupInformation_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder.SetMessageIdentification("MSG001", DateTime.Now);

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => _builder.Build());
            Assert.AreEqual("Original message ID is required. Use SetOriginalGroupInformation method.", ex.Message);
        }

        [TestMethod]
        public void Build_WithoutPaymentInstructions_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03");

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => _builder.Build());
            Assert.AreEqual("At least one original payment instruction is required. Use AddOriginalPaymentInstruction method.", ex.Message);
        }

        #endregion

        #region SetMessageIdentification Tests

        [TestMethod]
        public void SetMessageIdentification_WithValidParameters_ShouldSetMessageIdAndCreationTime()
        {
            // Arrange
            const string messageId = "TEST-MSG-001";
            var creationTime = new DateTime(2024, 1, 15, 10, 30, 0);

            // Act
            var result = _builder.SetMessageIdentification(messageId, creationTime);
            var report = _builder
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001")
                .Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(messageId, report.GrpHdr.MsgId);
            Assert.AreEqual(creationTime, report.GrpHdr.CreDtTm);
        }

        [TestMethod]
        public void SetMessageIdentification_WithCurrentTime_ShouldSetMessageIdAndCurrentTime()
        {
            // Arrange
            const string messageId = "TEST-MSG-002";
            var beforeTime = DateTime.Now;

            // Act
            var result = _builder.SetMessageIdentification(messageId);
            var report = _builder
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001")
                .Build();

            var afterTime = DateTime.Now;

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(messageId, report.GrpHdr.MsgId);
            Assert.IsTrue(report.GrpHdr.CreDtTm >= beforeTime && report.GrpHdr.CreDtTm <= afterTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMessageIdentification_WithNullMessageId_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetMessageIdentification(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMessageIdentification_WithEmptyMessageId_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetMessageIdentification("");
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
                .AddOriginalPaymentInstruction("PMT001")
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
                .AddOriginalPaymentInstruction("PMT001")
                .SetInitiatingParty(partyName);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result, "Should return the same builder instance for method chaining");
            Assert.AreEqual(partyName, report.GrpHdr.InitgPty.Nm);
            Assert.IsNull(report.GrpHdr.InitgPty.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetInitiatingParty_WithNullPartyName_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetInitiatingParty(null);
        }

        #endregion

        #region SetForwardingAgent Tests

        [TestMethod]
        public void SetForwardingAgent_WithValidParameters_ShouldSetForwardingAgent()
        {
            // Arrange
            const string agentName = "Forwarding Bank Ltd";
            const string bicfi = "FWDBANK1";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001")
                .SetForwardingAgent(agentName, bicfi);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.IsNotNull(report.GrpHdr.FwdgAgt);
            Assert.AreEqual(agentName, report.GrpHdr.FwdgAgt.FinInstnId.Nm);
            Assert.AreEqual(bicfi, report.GrpHdr.FwdgAgt.FinInstnId.Bicfi);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetForwardingAgent_WithNullAgentName_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetForwardingAgent(null, "BICFI123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetForwardingAgent_WithNullBicfi_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetForwardingAgent("Agent Name", null);
        }

        #endregion

        #region SetOriginalGroupInformation Tests

        [TestMethod]
        public void SetOriginalGroupInformation_WithRequiredParameters_ShouldSetGroupInformation()
        {
            // Arrange
            const string originalMessageId = "ORIG-MSG-001";
            const string originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = new DateTime(2024, 1, 15, 9, 0, 0);

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime)
                .AddOriginalPaymentInstruction("PMT001");
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(originalMessageId, report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, report.OrgnlGrpInfAndSts.OrgnlMsgNmId);
            Assert.AreEqual(originalCreationDateTime, report.OrgnlGrpInfAndSts.OrgnlCreDtTm);
            Assert.IsTrue(report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithoutOptionalDateTime_ShouldNotSetDateTime()
        {
            // Arrange
            const string originalMessageId = "ORIG-MSG-002";
            const string originalMessageNameId = "pain.001.001.03";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation(originalMessageId, originalMessageNameId)
                .AddOriginalPaymentInstruction("PMT001");
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(originalMessageId, report.OrgnlGrpInfAndSts.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, report.OrgnlGrpInfAndSts.OrgnlMsgNmId);
            Assert.IsFalse(report.OrgnlGrpInfAndSts.OrgnlCreDtTmSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetOriginalGroupInformation_WithNullMessageId_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetOriginalGroupInformation(null, "pain.001.001.03");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetOriginalGroupInformation_WithNullMessageNameId_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetOriginalGroupInformation("ORIG-MSG-001", null);
        }

        #endregion

        #region AddGroupStatusReason Tests

        [TestMethod]
        public void AddGroupStatusReason_WithValidParameters_ShouldAddStatusReason()
        {
            // Arrange
            const string reasonCode = "AC03";
            const string additionalInfo = "Invalid account number";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddGroupStatusReason(reasonCode, additionalInfo)
                .AddOriginalPaymentInstruction("PMT001");
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, report.OrgnlGrpInfAndSts.StsRsnInf.Count);
            Assert.AreEqual(reasonCode, report.OrgnlGrpInfAndSts.StsRsnInf[0].Rsn.Cd);
            Assert.AreEqual(1, report.OrgnlGrpInfAndSts.StsRsnInf[0].AddtlInf.Count);
            Assert.AreEqual(additionalInfo, report.OrgnlGrpInfAndSts.StsRsnInf[0].AddtlInf[0]);
        }

        [TestMethod]
        public void AddGroupStatusReason_WithoutAdditionalInfo_ShouldAddStatusReasonWithoutAdditionalInfo()
        {
            // Arrange
            const string reasonCode = "AC01";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddGroupStatusReason(reasonCode)
                .AddOriginalPaymentInstruction("PMT001");
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, report.OrgnlGrpInfAndSts.StsRsnInf.Count);
            Assert.AreEqual(reasonCode, report.OrgnlGrpInfAndSts.StsRsnInf[0].Rsn.Cd);
            Assert.AreEqual(0, report.OrgnlGrpInfAndSts.StsRsnInf[0].AddtlInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddGroupStatusReason_WithNullReasonCode_ShouldThrowArgumentException()
        {
            // Act
            _builder.AddGroupStatusReason(null);
        }

        #endregion

        #region AddOriginalPaymentInstruction Tests

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithValidParameters_ShouldAddPaymentInstruction()
        {
            // Arrange
            const string instructionId = "PMT-INST-001";
            const string paymentStatus = "ACCP";
            const string numberOfTransactions = "5";
            const decimal controlSum = 1250.50m;

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction(instructionId, paymentStatus, numberOfTransactions, controlSum);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, report.OrgnlPmtInfAndSts.Count);
            Assert.AreEqual(instructionId, report.OrgnlPmtInfAndSts[0].OrgnlPmtInfId);
            Assert.AreEqual(paymentStatus, report.OrgnlPmtInfAndSts[0].PmtInfSts);
            Assert.AreEqual(numberOfTransactions, report.OrgnlPmtInfAndSts[0].OrgnlNbOfTxs);
            Assert.AreEqual(controlSum, report.OrgnlPmtInfAndSts[0].OrgnlCtrlSum);
            Assert.IsTrue(report.OrgnlPmtInfAndSts[0].OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithMinimalParameters_ShouldAddBasicPaymentInstruction()
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
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, report.OrgnlPmtInfAndSts.Count);
            Assert.AreEqual(instructionId, report.OrgnlPmtInfAndSts[0].OrgnlPmtInfId);
            Assert.IsNull(report.OrgnlPmtInfAndSts[0].PmtInfSts);
            Assert.IsNull(report.OrgnlPmtInfAndSts[0].OrgnlNbOfTxs);
            Assert.IsFalse(report.OrgnlPmtInfAndSts[0].OrgnlCtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOriginalPaymentInstruction_WithNullInstructionId_ShouldThrowArgumentException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction(null);
        }

        #endregion

        #region AddPaymentTransaction Tests

        [TestMethod]
        public void AddPaymentTransaction_WithValidParameters_ShouldAddTransaction()
        {
            // Arrange
            const string endToEndId = "E2E-001";
            const string transactionStatus = "ACCP";
            const string statusId = "STS-001";
            const string originalInstructionId = "ORIG-INST-001";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001")
                .AddPaymentTransaction(endToEndId, transactionStatus, statusId, originalInstructionId);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, report.OrgnlPmtInfAndSts[0].TxInfAndSts.Count);
            Assert.AreEqual(endToEndId, report.OrgnlPmtInfAndSts[0].TxInfAndSts[0].OrgnlEndToEndId);
            Assert.AreEqual(transactionStatus, report.OrgnlPmtInfAndSts[0].TxInfAndSts[0].TxSts);
            Assert.AreEqual(statusId, report.OrgnlPmtInfAndSts[0].TxInfAndSts[0].StsId);
            Assert.AreEqual(originalInstructionId, report.OrgnlPmtInfAndSts[0].TxInfAndSts[0].OrgnlInstrId);
        }

        [TestMethod]
        public void AddPaymentTransaction_WithoutPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03");

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(
                () => _builder.AddPaymentTransaction("E2E-001", "ACCP"));
            Assert.AreEqual("Add an original payment instruction before adding transactions.", ex.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPaymentTransaction_WithNullEndToEndId_ShouldThrowArgumentException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001");

            // Act
            _builder.AddPaymentTransaction(null, "ACCP");
        }

        #endregion

        #region AddTransactionStatusReason Tests

        [TestMethod]
        public void AddTransactionStatusReason_WithValidParameters_ShouldAddStatusReason()
        {
            // Arrange
            const string reasonCode = "AC03";
            const string additionalInfo = "Invalid account number";

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001")
                .AddPaymentTransaction("E2E-001", "RJCT")
                .AddTransactionStatusReason(reasonCode, additionalInfo);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            var transaction = report.OrgnlPmtInfAndSts[0].TxInfAndSts[0];
            Assert.AreEqual(1, transaction.StsRsnInf.Count);
            Assert.AreEqual(reasonCode, transaction.StsRsnInf[0].Rsn.Cd);
            Assert.AreEqual(1, transaction.StsRsnInf[0].AddtlInf.Count);
            Assert.AreEqual(additionalInfo, transaction.StsRsnInf[0].AddtlInf[0]);
        }

        [TestMethod]
        public void AddTransactionStatusReason_WithoutTransaction_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001");

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(
                () => _builder.AddTransactionStatusReason("AC03"));
            Assert.AreEqual("No payment transaction has been added. Use AddPaymentTransaction first.", ex.Message);
        }

        #endregion

        #region AddSupplementaryData Tests

        [TestMethod]
        public void AddSupplementaryData_WithValidData_ShouldAddSupplementaryData()
        {
            // Arrange
            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = "Additional Information"
            };

            // Act
            var result = _builder
                .SetMessageIdentification("MSG001", DateTime.Now)
                .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                .AddOriginalPaymentInstruction("PMT001")
                .AddSupplementaryData(supplementaryData);
            var report = result.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, report.SplmtryData.Count);
            Assert.AreEqual("Additional Information", report.SplmtryData[0].PlcAndNm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullData_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.AddSupplementaryData(null);
        }

        #endregion

        #region SaveToFile Tests

        [TestMethod]
        public void SaveToFile_WithValidPath_ShouldCreateXmlFile()
        {
            // Arrange
            var tempFile = System.IO.Path.GetTempFileName();
            try
            {
                _builder
                    .SetMessageIdentification("MSG001", DateTime.Now)
                    .SetOriginalGroupInformation("ORIG-MSG-001", "pain.001.001.03")
                    .AddOriginalPaymentInstruction("PMT001");

                // Act
                _builder.SaveToFile(tempFile);

                // Assert
                Assert.IsTrue(System.IO.File.Exists(tempFile));
                var content = System.IO.File.ReadAllText(tempFile);
                Assert.IsTrue(content.Contains("MSG001"));
                Assert.IsTrue(content.Contains("PMT001"));
            }
            finally
            {
                if (System.IO.File.Exists(tempFile))
                {
                    System.IO.File.Delete(tempFile);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveToFile_WithNullPath_ShouldThrowArgumentException()
        {
            // Act
            _builder.SaveToFile(null);
        }

        #endregion
    }
}
