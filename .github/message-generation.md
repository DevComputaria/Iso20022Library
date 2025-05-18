# ISO 20022 Message Types and Generation Guidelines

## Message Types

ISO 20022 messages are organized into several business domains, each with a specific 4-character identifier prefix:

1. **Payments (pacs, pain)**
   - `pacs`: Payments Clearing and Settlement
   - `pain`: Payment Initiation

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
