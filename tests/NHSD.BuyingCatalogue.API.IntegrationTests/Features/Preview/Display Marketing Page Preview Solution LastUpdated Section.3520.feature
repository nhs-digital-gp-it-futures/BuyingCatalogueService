Feature: Display Marketing Page Preview Solution LastUpdated Section
    As a Catalogue User
    I want to view Marketing Page Information for the Solutions Last Updated Section
    So that I can ensure the lastUpdated is the latest within solution

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |

@3520
Scenario Outline: 1. Last Updated is the latest of last updated in the solution tables
    Given Solutions exist
        | SolutionId | SolutionName | SupplierId | LastUpdated |
        | Sln1       | MedicOnline  | Sup 1      | <Solution>  |
    And MarketingContacts exist
        | SolutionId | LastUpdated |
        | Sln1       | <MarketingContact1> |
        | Sln1       | <MarketingContact2> |
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the last updated date in the solution is <LastUpdatedSolution>
Examples:
    | Solution   | MarketingContact1 | MarketingContact2 | LastUpdatedSolution |
    | 01/01/1753 | 01/01/1753        | 01/01/1753        | 01/01/1753          |
    | 31/12/9999 | 31/12/9999        | 31/12/9999        | 31/12/9999          |
    | 27/02/2020 | 30/01/2019        | 31/12/2019        | 27/02/2020          |
    | 07/12/2019 | 02/01/2020        | 01/01/2020        | 02/01/2020          |
    | 23/02/2022 | 05/12/2024        | 28/09/2025        | 28/09/2025          |

@3520
Scenario Outline: 2. No marketing contact exist, Last Updated is the latest of last updated in the solution tables
    Given Solutions exist
        | SolutionId | SolutionName | SupplierId | LastUpdated |
        | Sln1       | MedicOnline  | Sup 1      | <Solution>  |
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the last updated date in the solution is <LastUpdatedSolution>
Examples:
    | Solution   | LastUpdatedSolution |
    | 01/01/1753 | 01/01/1753          |
    | 31/12/9999 | 31/12/9999          |
