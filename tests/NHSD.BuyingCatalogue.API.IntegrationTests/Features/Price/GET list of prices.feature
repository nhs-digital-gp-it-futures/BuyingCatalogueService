Feature: GET a list of prices
    As a Public User
    I want to be able to retrieve the prices for a catalogue item
    So that I know the price structure for each catalogue item

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
	And AdditionalService exist
		| CatalogueItemId | CatalogueItemName                | CatalogueSupplierId | Summary                 | SolutionId |
		| Sln1-001A001    | MedicOnline Additional Service 1 | Sup 1               | Addition to MedicOnline | Sln1       |
	And CataloguePrice exists
		| CatalogueItemId | CataloguePriceTypeEnum | ProvisioningTypeEnum | CurrencyCode | Price  | PricingUnitId                        | TimeUnitEnum | CataloguePriceTierRef |
		| Sln1            | Flat                   | OnDemand             | £            | 521.34 | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Month        |                       |
		| Sln2            | Tiered                 | Patient              | $            |        | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year         | 1                     |
		| Sln3            | Flat                   | Declarative          | GBP          | 348.92 | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Month        |                       |
		| Sln3            | Flat                   | OnDemand             | USD          | 567.32 | 8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65 | Year         |                       |
		| Sln4            | Tiered                 | Patient              | EUR          |        | D43C661A-0587-45E1-B315-5E5091D6E9D0 | Year         | 2                     |
		| Sln4            | Tiered                 | Declarative          | AUZ          |        | 774E5A1D-D15C-4A37-9990-81861BEAE42B | Year         | 3                     |
		| Sln5            | Flat                   | OnDemand             | GBP          | 521.90 | 8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65 | NULL         |                       |
		| Sln5            | Tiered                 | Patient              | GBP          |        | 90119522-D381-4296-82EE-8FE630593B56 | Year         | 4                     |
		| Sln1-001A001    | Flat                   | Patient              | GBP          | 199.99 | F8D06518-1A20-4FBA-B369-AB583F9FA8C0 | Month        |                       |
	And CataloguePriceTier exists
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

@7264
Scenario: 1. Get prices returns all prices
	When a GET request is made to retrieve the list of prices
	Then a successful response is returned
	And Prices are returned
		| Type   | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Flat   | OnDemand         | £            | 521.34 | consultation    | per consultation       | consultations       | month        | per month           |
		| Tiered | Patient          | $            |        | bed             | per bed                | beds                | year         | per year            |
		| Flat   | Declarative      | GBP          | 348.92 | bed             | per bed                | beds                | month        | per month           |
		| Flat   | OnDemand         | USD          | 567.32 | licence         | per licence            | licences            | year         | per year            |
		| Tiered | Patient          | EUR          |        | bed             | per bed                | beds                | year         | per year            |
		| Tiered | Declarative      | AUZ          |        | consultation    | per consultation       | consultations       | year         | per year            |
		| Flat   | OnDemand         | GBP          | 521.90 | licence         | per licence            | licences            | NULL         | NULL                |
		| Tiered | Patient          | GBP          |        | sms             | per SMS                | SMS                 | year         | per year            |
		| Flat   | Patient          | GBP          | 199.99 | patient         | per patient            | patients            | month        | per month           |

@7264
Scenario: 2. Get a single Flat Price
	When a GET request is made to retrieve the list of prices using catalogue item ID Sln1
	Then a successful response is returned
	And Prices are returned
		| Type | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Flat | OnDemand         | £            | 521.34 | consultation    | per consultation       | consultations       | month        | per month           |

@7264
Scenario: 3. Get a single Tiered Price
	When a GET request is made to retrieve the list of prices using catalogue item ID Sln2
	Then a successful response is returned
	And Prices are returned
		| Type   | ProvisioningType | CurrencyCode | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Tiered | Patient          | $            | bed             | per bed                | beds                | year         | per year            |
	And the Prices Tiers are returned
		| Start | End | Price   |
		| 1     | 5   | 700.000 |
		| 6     | 10  | 600.000 |
		| 11    |     | 500.000 |

@7264
Scenario: 4. Get a list of flat prices
	When a GET request is made to retrieve the list of prices using catalogue item ID Sln3
	Then a successful response is returned
	And Prices are returned
		| Type | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Flat | Declarative      | GBP          | 348.92 | bed             | per bed                | beds                | month        | per month           |
		| Flat | OnDemand         | USD          | 567.32 | licence         | per licence            | licences            | year         | per year            |

@7264
Scenario: 5. Get a list of Tiered prices
	When a GET request is made to retrieve the list of prices using catalogue item ID Sln4
	Then a successful response is returned
	And Prices are returned
		| Type   | ProvisioningType | CurrencyCode | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Tiered | Patient          | EUR          | bed             | per bed                | beds                | year         | per year            |
		| Tiered | Declarative      | AUZ          | consultation    | per consultation       | consultations       | year         | per year            |
	And the Prices Tiers are returned
		| Start | End | Price  | Section |
		| 1     | 8   | 900.00 | 0       |
		| 9     | 15  | 800.00 | 0       |
		| 16    |     | 700.00 | 0       |
		| 1     | 8   | 800.00 | 1       |
		| 19    |     | 700.00 | 1       |

@7264
Scenario: 6. Get a list of flat and tiered prices
	When a GET request is made to retrieve the list of prices using catalogue item ID Sln5
	Then a successful response is returned
	And Prices are returned
		| Type   | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Flat   | OnDemand         | GBP          | 521.90 | licence         | per licence            | licences            | NULL         | NULL                |
		| Tiered | Patient          | GBP          |        | sms             | per SMS                | SMS                 | year         | per year            |
	And the Prices Tiers are returned
		| Start | End | Price   | Section |
		| 1     | 10  | 2100.93 | 0       |
		| 11    |     | 1943.21 | 0       |

@7264
Scenario: 7. Get a single Flat Price for an additional service
	When a GET request is made to retrieve the list of prices using catalogue item ID Sln1-001A001
	Then a successful response is returned
	And Prices are returned
		| Type | ProvisioningType | CurrencyCode | Price  | PricingItemName | PricingItemDescription | PricingItemTierName | TimeUnitName | TimeUnitDescription |
		| Flat | Patient          | GBP          | 199.99 | patient         | per patient            | patients            | month        | per month           |

@7264
Scenario: 8. Get prices with a catalogue item ID that doesn't exist
	When a GET request is made to retrieve the list of prices using catalogue item ID Solution99
	Then a successful response is returned
	And an empty price list is returned
