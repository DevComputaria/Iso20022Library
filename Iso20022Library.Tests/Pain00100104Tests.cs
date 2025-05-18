using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Validators;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                    PmtInf = new PaymentInstruction6[] // Corrected type to array as required by CustomerCreditTransferInitiationV04
                    {
                        new PaymentInstruction6
                        {
                            PmtInfId = "TestPmtInfId",
                            PmtMtd = PaymentMethod3Code.TRF,
                            NbOfTxs = "1",
                            CtrlSum = 100.00m,
                            CtrlSumSpecified = true,
                            ReqdExctnDt = DateTime.Today,
                            Dbtr = new PartyIdentification43 { Nm = "Debtor Name" },
                            DbtrAcct = new CashAccount24 { Id = new AccountIdentification4Choice { Item = "DE89370400440532013000" } },
                            DbtrAgt = new BranchAndFinancialInstitutionIdentification5 { FinInstnId = new FinancialInstitutionIdentification8 { BICFI = "TESTBICD" } },
                            // Changed from List<CreditTransferTransaction1> to CreditTransferTransaction1[] as required by PaymentInstruction6
                            CdtTrfTxInf = new CreditTransferTransaction1[]
                            {
                                new CreditTransferTransaction1
                                {
                                    PmtId = new PaymentIdentification1 { InstrId = "InstrId", EndToEndId = "EndToEndId" },
                                    Amt = new AmountType3Choice { Item = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 100.00m } },
                                    CdtrAgt = new BranchAndFinancialInstitutionIdentification5 { FinInstnId = new FinancialInstitutionIdentification8 { BICFI = "TESTBICC" } },
                                    Cdtr = new PartyIdentification43 { Nm = "Creditor Name" },
                                    CdtrAcct = new CashAccount24 { Id = new AccountIdentification4Choice { Item = "DE02120300000000202051" } }
                                }
                            }
                        }
                    }
                }
            };

            var factory = new MessageBuilderFactory();
            var builder = factory.GetBuilder(MessageType.Pain00100104);
            var xml = builder.BuildXml(document);

            var xsdPath = "C:\\Users\\marci\\source\\repos\\Iso20022Library\\Iso20022Library.Messages\\Payments\\Pain\\Xsd\\pain.001.001.04.xsd";

            // Act
            bool isValid = XmlValidator.Validate(xml, xsdPath, out string errors);

            // Assert
            Assert.IsTrue(isValid, $"XML inválido: {errors}");
        }
    }
}
