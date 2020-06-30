Feature: GET a list of prices from a Solution ID
    As a Public User
    I want to be able to retrieve the price for a solution
    So that I know the price structure for each Solution

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
        | Sln5       | GP Practice     | Sup 1      |
    Given CataloguePrice exists
        | CatalogueItemId | CataloguePriceTypeEnum | ProvisioningTypeEnum | CurrencyCode | Price   | PricingUnitId                        | TimeUnitEnum | CataloguePriceTierRef |
        | Sln1            | Flat                   | OnDemand             | £            | 521.34  | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Month        |                       |
        | Sln2            | Tiered                 | PatientNumbers       | $            |         | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year         | 1                     |
        | Sln3            | Flat                   | Declarative          | GBP          | 348.92  | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Month        |                       |
        | Sln3            | Flat                   | OnDemand             | USD          | 567.32  | 8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65 | Year         |                       |
        | Sln4            | Tiered                 | PatientNumbers       | EUR          |         | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year         | 2                     |
        | Sln4            | Tiered                 | Declarative          | AUZ          |         | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Year         | 3                     |
        | Sln5            | Flat                   | OnDemand             | GBP          | 521.90  | 8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65 | NULL         |                       |
        | Sln5            | Tiered                 | PatientNumbers       | GBP          |         | 90119522-D381-4296-82EE-8FE630593B56 | Year         | 4                     |
    Given CataloguePriceTier exists
        | CataloguePriceTierRef | BandStart | BandEnd | Price   |
        | 1                     | 1         | 5       | 700.00  |
        | 1                     | 6         | 10      | 600.00  |
        | 1                     | 11        |         | 500.00  |
        | 2                     | 1         | 8       | 900.00  |
        | 2                     | 9         | 15      | 800.00  |
        | 2                     | 16        |         | 700.00  |
        | 3                     | 1         | 8       | 800.00  |
        | 3                     | 19        |         | 700.00  |
        | 4                     | 1         | 10      | 2100.93 |
        | 4                     | 11        |         | 1943.21 |

@7260
Scenario: 1. Get a single Flat Price
    When a GET request is made to retrieve the pricing with Solution ID Sln1
    Then a successful response is returned
    And the string value of element name is MedicOnline
    And Prices are returned
        | Type | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Flat | OnDemand         | £            | 521.34 | consultation    | per consultation       | consultations       | month        | per month           |

@7260
Scenario: 2. Get a single Tiered Price
    When a GET request is made to retrieve the pricing with Solution ID Sln2
    Then a successful response is returned
    And the string value of element name is TakeTheRedPill
    And Prices are returned
        | Type   | ProvisioningType | CurrencyCode | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Tiered | PatientNumbers   | $            | bed             | per bed                | beds                | year         | per year            |
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
        | Type | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Flat | Declarative      | GBP          | 348.92 | bed             | per bed                | beds                | month        | per month           |
        | Flat | OnDemand         | USD          | 567.32 | licence         | per license            | licenses            | year         | per year            |

@7260
Scenario: 4. Get a list of Tiered prices
    When a GET request is made to retrieve the pricing with Solution ID Sln4
    Then a successful response is returned
    And the string value of element name is MedicRUs
    And Prices are returned
        | Type   | ProvisioningType | CurrencyCode | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Tiered | PatientNumbers   | EUR          | bed             | per bed                | beds                | year         | per year            |
        | Tiered | Declarative      | AUZ          | consultation    | per consultation       | consultations       | year         | per year            |
    And the Prices Tiers are returned
        | Start | End | Price  | Section |
        | 1     | 8   | 900.00 | 0       |
        | 9     | 15  | 800.00 | 0       |
        | 16    |     | 700.00 | 0       |
        | 1     | 8   | 800.00 | 1       |
        | 19    |     | 700.00 | 1       |

@7260
Scenario: 5. Get a list of flat and tiered prices
    When a GET request is made to retrieve the pricing with Solution ID Sln5
    Then a successful response is returned
    And the string value of element name is GP Practice
    And Prices are returned
        | Type   | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Flat   | OnDemand         | GBP          | 521.90 | licence         | per license            | licenses            | NULL         | NULL                |
        | Tiered | PatientNumbers   | GBP          |        | sms             | per SMS                | SMS                 | year         | per year            |
    And the Prices Tiers are returned
        | Start | End | Price   | Section |
        | 1     | 10  | 2100.93 | 0       |
        | 11    |     | 1943.21 | 0       |

@7260
Scenario: 6. Get a price with no solution
     When a GET request is made to retrieve the pricing with Solution ID INVALID
     Then a response status of 404 is returned
