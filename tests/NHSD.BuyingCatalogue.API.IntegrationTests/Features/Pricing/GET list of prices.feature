Feature: GET a list of prices from a Solution ID
    As a Public User
    I want to be able to retrieve the price for a solution
    So that I know the pricing structure for each Solution

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName    | SupplierId |
        | Sln1       | MedicOnline     | Sup 1      |
        | Sln2       | TakeTheRedPill  | Sup 1      |
        | Sln3       | TakeTheBluePill | Sup 1      |
    Given CataloguePrice exists
        | CatalogueItemId | CataloguePriceTypeId | CurrencyCode | Price  | PricingUnitId                        | TimeUnitId |
        | Sln1            | 1                    | £            | 521.34 | 774E5A1D-D15C-4A37-9990-81861BEAE42B | 1          |
        | Sln2            | 2                    | $            | 481.65 | D43C661A-0587-45E1-B315-5E5091D6E9D0 | 2          |
        | Sln3            | 1                    | GBP          | 348.92 | D43C661A-0587-45E1-B315-5E5091D6E9D0 | 1          |
        | Sln3            | 1                    | USD          | 567.32 | 8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65 | 2          |
    Given CataloguePriceTier exists
        | CataloguePriceCurrencyCode | BandStart | BandEnd | Price  |
        | $                          | 1         | 5       | 700.00 |
        | $                          | 6         | 10      | 600.00 |
        | $                          | 11        |         | 500.00 |

@7260
Scenario: 1. Get a single Flat Price
    When a GET request is made to retrieve the pricing with Solution ID Sln1
    Then a successful response is returned
    And the string value of element name is MedicOnline
    And Prices are returned
        | Type | CurrencyCode | Price  |
        | Flat | £            | 521.34 |
    And has Pricing Item Unit
        | Name         | Description      | TierName      |
        | consultation | per consultation | consultations |
    And has Pricing Time Unit
        | Name  | Description |
        | month | per month   |

@7260
Scenario: 2. Get a single Tierred Price
    When a GET request is made to retrieve the pricing with Solution ID Sln2
    Then a successful response is returned
    And the string value of element name is TakeTheRedPill
    And Prices are returned
        | Type   | CurrencyCode |
        | Tiered | $            |
    And has Pricing Item Unit
        | Name | Description | TierName |
        | bed  | per bed     | beds     |
    And has Pricing Time Unit
        | Name | Description |
        | year | per year    |
    And the Prices Tiers are returned
        | Start | End | Price   |
        | 1     | 5   | 700.000 |
        | 6     | 10  | 600.000 |
        | 11    |     | 500.000 |

@7260
Scenario: 3. Get a list of flat prices
    When a GET request is made to retrieve the pricing with Solution ID Sln3
    Then a successful response is returned
    And the string value of element name is TakeTheBluePill
    And Prices are returned
        | Type | CurrencyCode | Price  |
        | Flat | GBP          | 348.92 |
        | Flat | USD          | 567.32 |
    And has Pricing Item Unit
        | Name    | Description | TierName |
        | bed     | per bed     | beds     |
        | licence | per license | licenses |
    And has Pricing Time Unit
        | Name  | Description |
        | month | per month   |
        | year  | per year    |
