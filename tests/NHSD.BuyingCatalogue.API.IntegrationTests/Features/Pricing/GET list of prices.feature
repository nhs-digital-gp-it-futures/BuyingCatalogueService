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
        | CatalogueItemId | CurrencyCode |
        | Sln1            | £            |

@7260
Scenario: 1. Get the stuff
    When a GET request is made to retrieve the pricing with Solution ID Sln1
    Then a successful response is returned
    And Prices are returned
        | PriceId | CurrencyCode |
        | Sln1    | £            |
