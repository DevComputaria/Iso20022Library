using Iso20022Library.Messages.Payments.Pain.Generated;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing Pain.001.001.04 messages.
    /// </summary>
    public class Pain00100104Builder
    {
        private readonly Document _document;

        public Pain00100104Builder()
        {
            _document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV04()
            };
        }

        public Pain00100104Builder WithGroupHeader(GroupHeader48 groupHeader)
        {
            _document.CstmrCdtTrfInitn.GrpHdr = groupHeader;
            return this;
        }

        public Pain00100104Builder AddPaymentInstruction(PaymentInstruction6 paymentInstruction)
        {
            var paymentInstructions = _document.CstmrCdtTrfInitn.PmtInf?.ToList() ?? new List<PaymentInstruction6>();
            paymentInstructions.Add(paymentInstruction);
            _document.CstmrCdtTrfInitn.PmtInf = paymentInstructions.ToArray();
            return this;
        }

        public Pain00100104Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            var supplementaryDataList = _document.CstmrCdtTrfInitn.SplmtryData?.ToList() ?? new List<SupplementaryData1>();
            supplementaryDataList.Add(supplementaryData);
            _document.CstmrCdtTrfInitn.SplmtryData = supplementaryDataList.ToArray();
            return this;
        }

        public Document Build()
        {
            // Add validation logic if necessary
            return _document;
        }
    }
}