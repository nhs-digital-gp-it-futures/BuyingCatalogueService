Feature: GET a list of prices from a Solution ID
    As a Public User
    I want to be able to retrieve the price for a solution
    So that I know the pricing structure for each Solution

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    Given CataloguePrice exists
        | CatalogueItemId | CurrencyCode | Price  | PricingUnitId                        | TimeUnitId |
        | Sln1            | £            | 521.34 | D43C661A-0587-45E1-B315-5E5091D6E9D0 | 1          |
        | Sln2            | $            | 481.65 | D43C661A-0587-45E1-B315-5E5091D6E9D0 | 1          |
    Given CataloguePriceTier exists
        | CataloguePriceCurrencyCode | BandStart | BandEnd | Price  |
        | $                          | 1         | 5       | 700.00 |
        | $                          | 6         | 10      | 600.00 |
        | $                          | 11        | NULL    | 500.00 |

@7260
Scenario: 1. Get a single Flat Price
    When a GET request is made to retrieve the pricing with Solution ID Sln1
    Then a successful response is returned
    And the string value of element name is NOT IMPLEMENTED
    And Prices are returned
        | Type | CurrencyCode | Price  |
        | Flat | £            | 521.34 |
    And has Pricing Item Unit
        | Name | Description | TierName |
        | bed  | per bed     | beds     |
    And has Pricing Time Unit
        | Name  | Description |
        | month | per month   |

@7260
Scenario: 2. Get a single Tierred Price
    When a GET request is made to retrieve the pricing with Solution ID Sln2
    Then a successful response is returned
    And the string value of element name is NOT IMPLEMENTED
