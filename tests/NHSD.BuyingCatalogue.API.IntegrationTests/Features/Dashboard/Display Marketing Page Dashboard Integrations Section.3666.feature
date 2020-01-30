Feature: Display Marketing Page Dashboard Integrations Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Integrations
    So that I can ensure the information is correct
    Background:
        Given Suppliers exist
            | Id    | SupplierName |
            | Sup 1 | Supplier 1   |
        And Solutions exist
            | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
            | Sln1       | MedicOnline    | 1                | Sup 1      |

    @3666
    Scenario: 1. Integrations section is optional and is incomplete
        When a GET request is made for solution dashboard Sln1
        Then a successful response is returned
        And the solution integrations section status is INCOMPLETE
        And the solution integrations section requirement is Optional


