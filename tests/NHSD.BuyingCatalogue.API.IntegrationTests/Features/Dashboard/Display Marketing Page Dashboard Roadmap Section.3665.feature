Feature: Display Marketing Page Dashboard Roadmap Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Roadmap
    So that I can ensure the information is correct
Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   |
        | Sln1     | An full online medicine system | Online medicine 1 |

@3665
Scenario: 1. Roadmap section is optional and is reported incomplete
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    And the solution roadmap section status is INCOMPLETE
    And the solution roadmap section requirement is Optional
