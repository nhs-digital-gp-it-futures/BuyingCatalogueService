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
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
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
