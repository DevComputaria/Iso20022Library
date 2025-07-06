using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200104;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00200104Builder class.
    /// </summary>
    /// <remarks>
    /// These tests validate the functionality of the ISO 20022 pain.002.001.04 
    /// (Customer Payment Status Report) message builder, ensuring proper construction,
    /// validation, and XML serialization of payment status report messages.
    /// </remarks>
    [TestClass]
    public class Pain00200104BuilderTests
    {
        private Pain00200104Builder _builder = null!;

        /// <summary>
        /// Initializes the test environment before each test method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _builder = new Pain00200104Builder();
        }

        /// <summary>
        /// Tests that the builder initializes correctly with default values.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var builder = new Pain00200104Builder();

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsFalse(builder.IsValid(), "Builder should not be valid without required fields");
        }

        /// <summary>
        /// Tests successful creation of a complete pain.002.001.04 message.
        /// </summary>
        [TestMethod]
        public void BuildCompleteMessage_ShouldCreateValidDocument()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();
            var originalGroupHeader = CreateSampleOriginalGroupHeader();
            var originalPaymentInstruction = CreateSampleOriginalPaymentInstruction();

            // Act
            var document = _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInstruction)
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtStsRpt);
            Assert.AreSame(groupHeader, document.CstmrPmtStsRpt.GrpHdr);
            Assert.AreSame(originalGroupHeader, document.CstmrPmtStsRpt.OrgnlGrpInfAndSts);
            Assert.AreEqual(1, document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count);
            Assert.AreSame(originalPaymentInstruction, document.CstmrPmtStsRpt.OrgnlPmtInfAndSts[0]);
        }

        /// <summary>
        /// Tests XML serialization of a complete pain.002.001.04 message.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithValidData_ShouldReturnXmlString()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();
            var originalGroupHeader = CreateSampleOriginalGroupHeader();

            // Act
            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader);

            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
            Assert.IsTrue(xml.Contains("CstmrPmtStsRpt"));
            Assert.IsTrue(xml.Contains("MSG20250706001"));
            Assert.IsTrue(xml.Contains("SAMPLE_BANK"));
        }

        /// <summary>
        /// Tests that adding multiple original payment instructions works correctly.
        /// </summary>
        [TestMethod]
        public void AddMultipleOriginalPaymentInstructions_ShouldAddAllInstructions()
        {
            // Arrange
            var instructions = new Collection<OriginalPaymentInstruction1>
            {
                CreateSampleOriginalPaymentInstruction("PMT001"),
                CreateSampleOriginalPaymentInstruction("PMT002"),
                CreateSampleOriginalPaymentInstruction("PMT003")
            };

            // Act
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddOriginalPaymentInstructionsAndStatus(instructions);

            var document = _builder.Build();

            // Assert
            Assert.AreEqual(3, document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count);
        }

        /// <summary>
        /// Tests validation behavior when required fields are missing.
        /// </summary>
        [TestMethod]
        public void IsValid_WithoutRequiredFields_ShouldReturnFalse()
        {
            // Act & Assert
            Assert.IsFalse(_builder.IsValid());

            // Add only group header
            _builder.WithGroupHeader(CreateSampleGroupHeader());
            Assert.IsFalse(_builder.IsValid());

            // Add original group header
            _builder.WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader());
            Assert.IsTrue(_builder.IsValid());
        }

        /// <summary>
        /// Tests that Build throws an exception when group header is missing.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowException()
        {
            // Arrange
            _builder.WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader());

            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests that Build throws an exception when original group information is missing.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalGroupInformation_ShouldThrowException()
        {
            // Arrange
            _builder.WithGroupHeader(CreateSampleGroupHeader());

            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests null parameter validation for WithGroupHeader method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithGroupHeader_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.WithGroupHeader(null!);
        }

        /// <summary>
        /// Tests null parameter validation for WithOriginalGroupInformationAndStatus method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithOriginalGroupInformationAndStatus_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.WithOriginalGroupInformationAndStatus(null!);
        }

        /// <summary>
        /// Tests null parameter validation for AddOriginalPaymentInstructionAndStatus method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalPaymentInstructionAndStatus_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstructionAndStatus(null!);
        }

        /// <summary>
        /// Tests the Reset functionality.
        /// </summary>
        [TestMethod]
        public void Reset_ShouldClearAllData()
        {
            // Arrange
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddOriginalPaymentInstructionAndStatus(CreateSampleOriginalPaymentInstruction());

            // Act
            _builder.Reset();

            // Assert
            Assert.IsFalse(_builder.IsValid());
        }

        /// <summary>
        /// Tests the Clone functionality.
        /// </summary>
        [TestMethod]
        public void Clone_ShouldCreateIdenticalBuilder()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();
            var originalGroupHeader = CreateSampleOriginalGroupHeader();
            var originalPaymentInstruction = CreateSampleOriginalPaymentInstruction();

            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInstruction);

            // Act
            var clonedBuilder = _builder.Clone();
            var originalDocument = _builder.Build();
            var clonedDocument = clonedBuilder.Build();

            // Assert
            Assert.AreNotSame(_builder, clonedBuilder);
            Assert.AreNotSame(originalDocument, clonedDocument);
            Assert.AreEqual(originalDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count, 
                           clonedDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count);
        }

        /// <summary>
        /// Tests BuildXml with an object parameter of wrong type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildXml_WithInvalidMessageType_ShouldThrowException()
        {
            // Act
            _builder.BuildXml("invalid message type");
        }

        /// <summary>
        /// Tests BuildXml with null object parameter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildXml_WithNullMessage_ShouldThrowException()
        {
            // Act
            _builder.BuildXml(null!);
        }

        #region Helper Methods

        /// <summary>
        /// Creates a sample GroupHeader52 for testing purposes.
        /// </summary>
        /// <returns>A properly initialized GroupHeader52 instance.</returns>
        private static GroupHeader52 CreateSampleGroupHeader()
        {
            return new GroupHeader52
            {
                MsgId = "MSG20250706001",
                CreDtTm = DateTime.Now,
                InitgPty = new PartyIdentification43
                {
                    Nm = "SAMPLE_BANK",
                    Id = new Party11Choice
                    {
                        OrgId = new OrganisationIdentification8
                        {
                            AnyBic = "SAMPLEGB2L"
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Creates a sample OriginalGroupHeader1 for testing purposes.
        /// </summary>
        /// <returns>A properly initialized OriginalGroupHeader1 instance.</returns>
        private static OriginalGroupHeader1 CreateSampleOriginalGroupHeader()
        {
            return new OriginalGroupHeader1
            {
                OrgnlMsgId = "ORIG_MSG_001",
                OrgnlMsgNmId = "pain.001.001.04",
                OrgnlCreDtTm = DateTime.Now.AddHours(-1),
                OrgnlNbOfTxs = "5",
                OrgnlCtrlSum = 1500.00m,
                OrgnlCtrlSumSpecified = true,
                GrpSts = TransactionGroupStatus3Code.Accp,
                GrpStsSpecified = true
            };
        }

        /// <summary>
        /// Creates a sample OriginalPaymentInstruction1 for testing purposes.
        /// </summary>
        /// <param name="paymentInfoId">Optional payment information ID. Defaults to "PMT_INFO_001".</param>
        /// <returns>A properly initialized OriginalPaymentInstruction1 instance.</returns>
        private static OriginalPaymentInstruction1 CreateSampleOriginalPaymentInstruction(string paymentInfoId = "PMT_INFO_001")
        {
            var originalPayment = new OriginalPaymentInstruction1
            {
                OrgnlPmtInfId = paymentInfoId,
                PmtInfSts = TransactionGroupStatus3Code.Accp,
                PmtInfStsSpecified = true
            };

            // Add status reason information
            originalPayment.StsRsnInf.Add(new StatusReasonInformation9
            {
                Rsn = new StatusReason6Choice
                {
                    Cd = "AC03" // Use string instead of enum that doesn't exist
                }
            });

            return originalPayment;
        }

        #endregion
    }
}
