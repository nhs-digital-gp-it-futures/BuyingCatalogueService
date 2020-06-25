Feature: GET a single prices from a Price ID
    As a Public User
    I want to be able to retrieve the price
    So that I know the pricing structure

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName    | SupplierId |
        | Sln1       | MedicOnline     | Sup 1      |
        | Sln2       | TakeTheRedPill  | Sup 1      |
        | Sln4       | MedicRUs        | Sup 1      |
    Given CataloguePrice exists
        | CatalogueItemId | ProvisioningTypeEnum | CataloguePriceTypeEnum | CurrencyCode | Price  | PricingUnitId                        | TimeUnitEnum | CataloguePriceTierRef | CataloguePriceIdRef |
        | Sln1            | OnDemand             | Flat                   | £            | 521.34 | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Month        |                       | priceId1            |
        | Sln2            | PatientNumbers       | Tiered                 | $            |        | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year         | 1                     | priceId2            |
        | Sln4            | Declarative          | Tiered                 | EUR          |        | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year         | 2                     |                     |
    Given CataloguePriceTier exists
        | CataloguePriceTierRef | BandStart | BandEnd | Price   |
        | 1                     | 1         | 5       | 700.00  |
        | 1                     | 6         | 10      | 600.00  |
        | 1                     | 11        |         | 500.00  |
        | 2                     | 1         | 8       | 900.00  |
        | 2                     | 9         | 15      | 800.00  |
        | 2                     | 16        |         | 700.00  |

@7840
Scenario: 1. Get a single Flat Price by CatalogugePriceId 
    When a GET request is made to retrieve a single price using the PriceId associated with CaltaloguePriceIdRef priceId1
    Then a successful response is returned
    And a Price is returned
        | Type | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Flat | OnDemand         | £            | 521.34 | consultation    | per consultation       | consultations       | month        | per month           |

@7840
Scenario: 2. Get a single Tierred Price by CatalogugePriceId
    When a GET request is made to retrieve a single price using the PriceId associated with CaltaloguePriceIdRef priceId2
    Then a successful response is returned
    And a Price is returned
        | Type   | ProvisioningType | CurrencyCode | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
        | Tiered | PatientNumbers   | $            | bed             | per bed                | beds                | year         | per year            |
    And the Price Tiers are returned
        | Start | End | Price   |
        | 1     | 5   | 700.000 |
        | 6     | 10  | 600.000 |
        | 11    |     | 500.000 |
