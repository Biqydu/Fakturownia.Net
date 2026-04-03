# SDK Constants & Enums Reference

## Namespaces

- **Constants:** `Biqydu.Fakturownia.Net.Abstractions.Constants`
- **Enums:** `Biqydu.Fakturownia.Net.Abstractions.Enums`

---

## Summary Table

| Type | Description | Example Key Values |
|------|-------------|--------------------|
| `InvoiceKinds` | Type of invoice document | `Vat`, `Proforma`, `Correction`, `Advance`, `Final`, `Receipt`, `Bill`, `VatMp`, `VatMargin`, `Kp`, `Kw`, `Estimate` |
| `InvoiceStatuses` | Current invoice status | `Issued`, `Sent`, `Paid`, `Partial`, `Rejected`, `Cancelled` |
| `VatRates` | Polish VAT rates and exemptions | `Vat23`, `Vat8`, `Vat5`, `Vat0`, `Exempt`, `NotTaxable`, `ReverseCharge` |
| `GtuCodes` | GTU codes for JPK_V7 reporting | `Gtu01` … `Gtu13` |
| `Currencies` | Currency codes | `PLN`, `EUR`, `USD`, `GBP`, `CHF` |
| `PaymentMethods` | Payment types | `Transfer`, `Cash`, `Card`, `PayU`, `PayPal`, `CashOnDelivery`, `Compensation`, `LetterOfCredit`, `Off` |
| `Languages` | Invoice language | `PL`, `EN`, `DE`, `FR`, `CZ`, `SK`, `AR`, `CN`, `TR`, etc. |
| `Countries` | Country codes for seller/buyer | `Poland`, `Germany`, `UnitedKingdom`, `UnitedStates`, `China`, `Japan`, etc. |
| `LogicalStatus` | Boolean-like toggle (Yes/No) | `Yes`, `No` |
| `DiscountKinds` | Discount application mode | `PercentUnit`, `PercentUnitGross`, `PercentTotal`, `Amount` |
| `IncomeKind` | Accounting direction | `Income`, `Expense` |
| `AdvanceCreationModes` | Advance invoice creation mode | `Percent`, `Amount` |
| `PeriodTemplates` | Predefined date periods for filtering | `ThisMonth`, `LastMonth`, `Last30Days`, `ThisYear`, `LastYear`, `All`, `More` |
| `SearchDateTypes` | Date field used in search | `IssueDate`, `PaidDate`, `TransactionDate` |
| `TaxNoKinds` | Type of tax identification number | `Nip`, `NipUe`, `Other`, `Empty` |
| `IssuerRoles` | Role of invoice issuer (KSeF) | `Issuer`, `Factor`, `OriginalEntity`, `JstIssuer`, `GvMemberIssuer` |
| `RecipientRoles` | Role of invoice recipient (KSeF) | `Recipient`, `AdditionalBuyer`, `Payer`, `JstRecipient`, `GvMemberRecipient`, `Employee` |

---

## Invoice Basics

### Invoice Kinds (`InvoiceKinds`)

Used in `InvoiceRequest.Kind`.

- `Vat`: Standard VAT invoice (default).
- `Proforma`: Proforma invoice.
- `Bill`: Non-VAT bill or simplified invoice.
- `Receipt`: Fiscal receipt.
- `Advance`: Advance payment invoice.
- `Correction`: Correction invoice.
- `VatMp`: VAT MP invoice (specific payment method).
- `InvoiceOther`: Other type of invoice.
- `VatMargin`: VAT margin invoice (used goods, travel agencies).
- `Kp`: Cash Received document.
- `Kw`: Cash Paid document.
- `Final`: Final invoice (settling advances).
- `Estimate`: Estimated invoice or quote.

### Invoice Statuses (`InvoiceStatuses`)

Used in `InvoiceRequest.Status` or when calling `UpdateStatusAsync`.

- `Issued`: New document, not yet sent/paid.
- `Sent`: Document sent to the buyer.
- `Paid`: Fully paid invoice.
- `Partial`: Partially paid.
- `Rejected`: Document cancelled or rejected.
- `Cancelled`: Explicitly cancelled invoice.

---

## Finance & Tax

### VAT Rates (`VatRates`)

Standard tax rates for the Polish market.

- `Vat23`, `Vat8`, `Vat5`: Standard percentage rates.
- `Vat0`: 0% rate.
- `Exempt`: VAT exempt (`zw`).
- `NotTaxable`: Not subject to VAT (`np`).
- `ReverseCharge`: Reverse charge (`oo`).

> **Pro Tip:** Use `VatRates.ToRate(VatRates.Vat23)` to get the decimal value `0.23m` for manual calculations.

### GTU Codes (`GtuCodes`)

Mandatory for JPK_V7 reporting in Poland.

- `Gtu01` to `Gtu13`: Full range of Goods and Services Groups.
- Example: `Gtu06` for most electronic devices (laptops, phones).

### Currencies (`Currencies`)

Commonly used currency codes.

- `PLN`, `EUR`, `USD`, `GBP`, `CHF`.

### Tax No Kinds (`TaxNoKinds`)

Used in `InvoiceRequest.BuyerTaxNoKind` and `SellerTaxNoKind`.

- `Nip`: Standard Polish NIP (default, empty string).
- `NipUe`: European VAT ID (NIP UE).
- `Other`: Other type of identification number.
- `Empty`: No tax ID number.

---

## Logistics & Roles

### Payment Methods (`PaymentMethods`)

Used in `InvoiceRequest.PaymentType`.

- `Transfer`: Standard bank transfer (default).
- `Cash`: Cash payment.
- `Card`: Credit/Debit card.
- `PayU`: PayU gateway.
- `PayPal`: PayPal.
- `CashOnDelivery`: Cash on delivery.
- `Compensation`: Compensation.
- `LetterOfCredit`: Letter of credit.
- `Off`: No payment method specified.
- Additional: `Barter`, `Cheque`, `BillOfExchange`.

### Languages (`Languages`)

Used in `InvoiceRequest.Lang`.

- `PL`, `EN`, `EN_GB`, `DE`, `FR`, `CZ`, `RU`, `ES`, `IT`, `NL`, `HR`, `AR`, `SK`, `SL`, `EL`, `ET`, `CN`, `HU`, `TR`, `FA`.

> **Helper:** `Languages.Bilingual("pl", "en")` returns `"pl/en"` for bilingual invoices.

### Countries (`Countries`)

ISO 3166-1 alpha-2 codes for seller/buyer country fields.

- Extensive list including `Poland (PL)`, `Germany (DE)`, `UnitedKingdom (GB)`, `UnitedStates (US)`, `China (CN)`, `Japan (JP)`, and many European countries.

---

## Invoice Advanced

### Advance Creation Modes (`AdvanceCreationModes`)

Used in `InvoiceRequest.AdvanceCreationMode` for advance invoices.

- `Percent`: Advance as a percentage of the order value.
- `Amount`: Advance as a specific gross amount.

### Discount Kinds (`DiscountKinds`)

Defines how discounts are displayed.

- `PercentUnit`: Percentage discount from net unit price.
- `PercentUnitGross`: Percentage discount from gross unit price.
- `PercentTotal`: Percentage discount from total item price.
- `Amount`: Fixed amount discount (subtracted from price).

### Logical Status (`LogicalStatus`)

Enum for Fakturownia's toggle fields (`Yes` = 1, `No` = 0).

Used in: `ShowDiscount`, `SplitPayment`, `AdditionalInfo`, `UseOss`.

### Income Kind (`IncomeKind`)

Enum for accounting purposes in `InvoiceRequest.Income`.

- `Expense = 0`
- `Income = 1`

---

## Date Filtering & Periods

### Period Templates (`PeriodTemplates`)

Used in `InvoiceQueryParams.Period` to quickly select date ranges.

- `ThisMonth`, `LastMonth`, `Last30Days`, `ThisYear`, `LastYear`, `All`.
- `More`: Requires additional `date_from` and `date_to` parameters.

### Search Date Types (`SearchDateTypes`)

Used in `InvoiceQueryParams.SearchDateType` to specify which date field to filter on.

- `IssueDate`: Filter by invoice issue date.
- `PaidDate`: Filter by payment date.
- `TransactionDate`: Filter by transaction date.

---

## KSeF & Role Management

### Issuer Roles (`IssuerRoles`)

Roles for invoice issuer, used in KSeF (National e-Invoice System) contexts.

- `Issuer`: Standard invoice issuer.
- `Factor`: Factoring entity.
- `OriginalEntity`: Original issuer.
- `JstIssuer`: Local Government Unit issuer.
- `GvMemberIssuer`: VAT Group member issuer.
- `Other`: Other role.

### Recipient Roles (`RecipientRoles`)

Roles for invoice recipient.

- `Recipient`: Standard recipient.
- `AdditionalBuyer`: Additional buyer.
- `Payer`: Entity making the payment.
- `JstRecipient`: Local Government Unit recipient.
- `GvMemberRecipient`: VAT Group member recipient.
- `Employee`: Employee.
- `Other`: Other role.

---

## Helper Methods

### `VatRates.ToRate(string vatRate)`

Converts a VAT rate string constant to a decimal value (e.g., `"23"` → `0.23m`). Returns `0m` for exempt, not taxable, or reverse charge.

```csharp
decimal? rate = VatRates.ToRate(VatRates.Vat23); // 0.23m
```

### `Languages.Bilingual(string lang1, string lang2)`

Creates a bilingual language code string (e.g., `"pl/en"`).
