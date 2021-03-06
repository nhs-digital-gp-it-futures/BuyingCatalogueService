Feature: Display Marketing Page Public Solution LastUpdated Section
    As a Catalogue User
    I want to view Marketing Page Information for the Solutions Last Updated Section
    So that I can ensure the lastUpdated is the latest within solution

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
@3520
Scenario Outline: Last Updated is the latest of last updated in the solution tables
    Given Solutions exist
        | SolutionId | SolutionName | SupplierId | LastUpdated |
        | Sln1       | MedicOnline  | Sup 1      | <Solution>  |
    And solutions have the following details
        | SolutionId | LastUpdated      |
        | Sln1       | <SolutionDetail> |
    And MarketingContacts exist
        | SolutionId | LastUpdated |
        | Sln1       | <MarketingContact1> |
        | Sln1       | <MarketingContact2> |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the last updated date in the solution is <LastUpdatedSolution>
Examples:
    | Solution   | SolutionDetail | MarketingContact1 | MarketingContact2 | LastUpdatedSolution |
    | 01/01/1753 | 01/01/1753     | 01/01/1753        | 01/01/1753        | 01/01/1753          |
    | 31/12/9999 | 31/12/9999     | 31/12/9999        | 31/12/9999        | 31/12/9999          |
    | 27/02/2020 | 27/02/2020     | 30/01/2019        | 31/12/2019        | 27/02/2020          |
    | 01/01/1753 | 31/12/9999     | 21/12/2019        | 25/12/2025        | 31/12/9999          |
    | 07/12/2019 | 03/12/2019     | 02/01/2020        | 01/01/2020        | 02/01/2020          |
    | 23/02/2022 | 03/10/2019     | 05/12/2024        | 28/09/2025        | 28/09/2025          |
