Feature: Display Marketing Page Public Capabilities Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Capabilities
    So that I can ensure the information is correct

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Workflow                | true         |
        | Clinical Safety         | false        |
    And Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
        | Sup 2 | Supplier 2   | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And Solutions are linked to Capabilities
        | Solution       | Capability              |
        | MedicOnline    | Appointments Management |
        | MedicOnline    | Clinical Safety         |
        | MedicOnline    | Workflow                |
        | PracticeMgr    | Clinical Safety         |
        | PracticeMgr    | Prescribing             |
        | PracticeMgr    | Workflow                |

@3507
Scenario: 1. Sections presented where Capabilities exists
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solution capabilities section contains Capabilities
    | Capability              |
    | Appointments Management |
    | Clinical Safety         |
    | Workflow                |
    
@3507
Scenario: 2. Sections not presented where no Capabilities exists
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solution capabilities section contains no Capabilities
