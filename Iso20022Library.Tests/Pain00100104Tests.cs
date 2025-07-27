using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Validators;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100104;
using static Iso20022Library.Messages.Payments.Pain.Generated.Pain00100104.PaymentMethod3Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Iso20022Library.Tests
{
    [TestClass]
    public class Pain00100104Tests
    {
        [TestMethod]
        public void BuildAndValidateXml_Pain00100104_ShouldPassValidation()
        {
            // Arrange
            var document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV04
                {
                    GrpHdr = new GroupHeader48
                    {
                        MsgId = "TestMsgId",
                        CreDtTm = DateTime.Now,
                        NbOfTxs = "1",
                        InitgPty = new PartyIdentification43 { Nm = "Test Initiating Party" },
                        CtrlSum = 100.00m,
                        CtrlSumSpecified = true
                    },
                    PmtInf =
                        {
                            new PaymentInstruction6
                            {
                                PmtInfId = "TestPmtInfId",
                                PmtMtd = PaymentMethod3Code.Trf,
                                NbOfTxs = "1",
                                CtrlSum = 100.00m,
                                CtrlSumSpecified = true,
                                ReqdExctnDt = DateTime.Today,
                                Dbtr = new PartyIdentification43 { Nm = "Debtor Name" },
                                DbtrAcct = new CashAccount24 { Id = new AccountIdentification4Choice { Iban = "DE89370400440532013000" } },
                                DbtrAgt = new BranchAndFinancialInstitutionIdentification5 { FinInstnId = new FinancialInstitutionIdentification8 { Bicfi = "TESTBICD" } },
                                CdtTrfTxInf =
                                {
                                    new CreditTransferTransaction1
                                    {
                                        PmtId = new PaymentIdentification1 { InstrId = "InstrId", EndToEndId = "EndToEndId" },
                                        Amt = new AmountType3Choice { InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 100.00m } },
                                        CdtrAgt = new BranchAndFinancialInstitutionIdentification5 { FinInstnId = new FinancialInstitutionIdentification8 { Bicfi = "TESTBICC" } },
                                        Cdtr = new PartyIdentification43 { Nm = "Creditor Name" },
                                        CdtrAcct = new CashAccount24 { Id = new AccountIdentification4Choice { Iban = "DE02120300000000202051" } }
                                    }
                                }
                            }
                        }
                }
            };

            var factory = new MessageBuilderFactory();
            var builder = factory.GetBuilder(MessageType.Pain00100104);
            var xml = builder.BuildXml(document);

            // Get the XSD path relative to the solution directory
            var xsdPath = GetXsdPath("pain.001.001.04.xsd");

            // Act
            bool isValid = XmlValidator.Validate(xml, xsdPath, out string errors);

            // Assert
            Assert.IsTrue(isValid, $"XML inválido: {errors}");
        }

        /// <summary>
        /// Gets the XSD file path relative to the solution directory.
        /// This method works across different operating systems and environments.
        /// </summary>
        /// <param name="xsdFileName">The name of the XSD file</param>
        /// <returns>The full path to the XSD file</returns>
        private static string GetXsdPath(string xsdFileName)
        {
            // Get the current test assembly location
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            
            if (string.IsNullOrEmpty(assemblyDirectory))
            {
                throw new InvalidOperationException("Could not determine assembly directory");
            }
            
            // Navigate up to find the solution root
            var currentDir = new DirectoryInfo(assemblyDirectory);
            while (currentDir != null && !File.Exists(Path.Combine(currentDir.FullName, "Iso20022Library.sln")))
            {
                currentDir = currentDir.Parent;
            }
            
            if (currentDir == null)
            {
                throw new DirectoryNotFoundException("Could not find solution root directory");
            }
            
            // Build the path to the XSD file
            var xsdPath = Path.Combine(
                currentDir.FullName,
                "Iso20022Library.Messages",
                "Payments",
                "Pain",
                "Xsd",
                xsdFileName
            );
            
            if (!File.Exists(xsdPath))
            {
                throw new FileNotFoundException($"XSD file not found: {xsdPath}");
            }
            
            return xsdPath;
        }
    }
}
