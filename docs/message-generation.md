# ISO 20022 Message Types and Generation Guidelines

## Message Types

ISO 20022 messages are organized into several business domains, each with a specific 4-character identifier prefix:

1. **Payments (pacs, pain)**
   - `pacs`: Payments Clearing and Settlement
   - `pain`: Payment Initiation

### Currently Supported Payment Messages

- **Pain.001.001.02**: Customer Credit Transfer Initiation V02
- **Pain.001.001.03**: Customer Credit Transfer Initiation V03
- **Pain.001.001.04**: Customer Credit Transfer Initiation V04

Each pain message has a corresponding builder in the `Iso20022Library.Application.Builders.Pain` namespace that provides a fluent API for message construction.

2. **Cash Management (camt)**
   - Cash Management and Account Services

3. **Securities (seev, semt, sese, secl)**
   - `seev`: Securities Events
   - `semt`: Securities Management
   - `sese`: Securities Settlement
   - `secl`: Securities Clearing

4. **Trade Services (tsmt)**
   - Trade Services Management

5. **Account Management (acmt)**
   - Account Management

6. **Reference Data (reda)**
   - Reference Data

7. **Foreign Exchange (fxtr)**
   - Foreign Exchange Trade

8. **Authorities (auth)**
   - Messages for Authorities

9. **Collateral (colr)**
   - Collateral Management

10. **Cards (caaa, caad, casr)**
    - `caaa`: Card Acceptor to Acquirer
    - `caad`: Card Transactions
    - `casr`: Card Services

11. **Treasury (trea)**
    - Treasury Operations

## Folder Structure Organization

The library organizes ISO 20022 messages by their business domains. Each message type should be placed in a dedicated folder under the main `Iso20022Library.Messages` directory.

The recommended folder structure is:

```
Iso20022Library.Messages/
│
├── Payments/
│   ├── Pacs/        # Payments Clearing and Settlement
│   └── Pain/        # Payment Initiation
│
├── CashManagement/
│   └── Camt/        # Cash Management
│
├── Securities/
│   ├── Seev/        # Securities Events
│   ├── Semt/        # Securities Management
│   ├── Sese/        # Securities Settlement
│   └── Secl/        # Securities Clearing
│
├── TradeServices/
│   └── Tsmt/        # Trade Services Management
│
├── AccountManagement/
│   └── Acmt/        # Account Management
│
├── ReferenceData/
│   └── Reda/        # Reference Data
│
├── ForeignExchange/
│   └── Fxtr/        # Foreign Exchange Trade
│
├── Authorities/
│   └── Auth/        # Authorities
│
├── Collateral/
│   └── Colr/        # Collateral Management
│
├── Cards/
│   ├── Caaa/        # Card Acceptor to Acquirer
│   ├── Caad/        # Card Transactions
│   └── Casr/        # Card Services
│
└── Treasury/
    └── Trea/        # Treasury Operations
```

This structure allows for easy navigation and maintenance of different message types within the ISO 20022 standard.

## Message Generation Guidelines

When generating message classes for ISO 20022:

1. Follow the domain-specific schema definitions
2. Generate strongly-typed C# classes for each message type
3. Implement validation according to ISO 20022 rules
4. Maintain proper namespacing that aligns with the folder structure
5. Generate serialization/deserialization capabilities for XML and JSON formats

Example namespace convention:
```csharp
namespace Iso20022Library.Messages.Payments.Pain
{
    // Pain message classes
}
```

## Builder Examples

### Pain.001.001.03 (Customer Credit Transfer Initiation V03)

```csharp
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100103;

// Create the builder
var builder = new Pain00100103Builder();

// Set up required elements
var initiatingParty = new PartyIdentification32 { Nm = "Your Company Name" };
var debtor = new PartyIdentification32 { Nm = "Debtor Name" };
var debtorAccount = new CashAccount16 
{ 
    Id = new AccountIdentification4Choice { Iban = "GB82WEST12345698765432" } 
};
var debtorAgent = new BranchAndFinancialInstitutionIdentification4
{
    FinInstnId = new FinancialInstitutionIdentification7 { Bic = "DEUTGB2L" }
};
var creditor = new PartyIdentification32 { Nm = "Creditor Name" };
var creditorAccount = new CashAccount16 
{ 
    Id = new AccountIdentification4Choice { Iban = "GB82WEST98765412345678" } 
};
var amount = new AmountType3Choice
{
    InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "GBP", Value = 100.00m }
};

// Build the message using fluent API
var document = builder
    .WithGroupHeader("MSG001", DateTime.Now, "1", initiatingParty)
    .AddPaymentInstruction("PMTINF001", PaymentMethod3Code.Trf,
        DateTime.Now.AddDays(1), debtor, debtorAccount, debtorAgent)
    .AddCreditTransferTransaction("E2E001", amount, creditor, creditorAccount)
    .Build();

// Serialize to XML
var xml = builder.BuildXml();
```

The builder provides a fluent API that ensures proper message structure and validation according to ISO 20022 standards.
