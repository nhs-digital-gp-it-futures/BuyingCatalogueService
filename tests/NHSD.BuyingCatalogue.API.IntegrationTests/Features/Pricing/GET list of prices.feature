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
        | Sln4       | MedicRUs        | Sup 1      |
    Given CataloguePrice exists
        | CatalogueItemId | CataloguePriceType | CurrencyCode | Price  | PricingUnitId                        | TimeUnit | CataloguePriceTierRef |
        | Sln1            | Flat               | £            | 521.34 | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Month    |                       |
        | Sln2            | Tiered             | $            |        | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year     | 1                     |
        | Sln3            | Flat               | GBP          | 348.92 | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Month    |                       |
        | Sln3            | Flat               | USD          | 567.32 | 8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65 | Year     |                       |
        | Sln4            | Tiered             | EUR          |        | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year     | 2                     |
        | Sln4            | Tiered             | AUZ          |        | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Year     | 3                     |
    Given CataloguePriceTier exists
        | CataloguePriceTierRef | BandStart | BandEnd | Price  |
        | 1                     | 1         | 5       | 700.00 |
        | 1                     | 6         | 10      | 600.00 |
        | 1                     | 11        |         | 500.00 |
        | 2                     | 1         | 8       | 900.00 |
        | 2                     | 9         | 15      | 800.00 |
        | 2                     | 16        |         | 700.00 |
        | 3                     | 1         | 8       | 800.00 |
        | 3                     | 19        |         | 700.00 |

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

@7260
Scenario: 4. Get a list of tired prices
    When a GET request is made to retrieve the pricing with Solution ID Sln4
    Then a successful response is returned
    And the string value of element name is MedicRUs
    And Prices are returned
        | Type   | CurrencyCode |
        | Tiered | EUR          |
        | Tiered | AUZ          |
    And has Pricing Item Unit
        | Name         | Description      | TierName      |
        | bed          | per bed          | beds          |
        | consultation | per consultation | consultations |
    And has Pricing Time Unit
        | Name | Description |
        | year | per year    |
        | year | per year    |
    And the Prices Tiers are returned
        | Start | End | Price  | Section |
        | 1     | 8   | 900.00 | 0       |
        | 9     | 15  | 800.00 | 0       |
        | 16    |     | 700.00 | 0       |
        | 1     | 8   | 800.00 | 1       |
        | 19    |     | 700.00 | 1       |
