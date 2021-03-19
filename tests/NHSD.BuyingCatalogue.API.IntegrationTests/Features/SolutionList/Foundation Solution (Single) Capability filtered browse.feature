Feature: Foundation Solution (Single) Capability filtered browse
    As a Public User
    I want to use Foundation Capabilities when browsing Solutions
    So that I know what Solutions (Single) include those Capabilities

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation | CapabilityRef |
        | Appointments Management | true         | C1            |
        | Prescribing             | true         | C10           |
        | Clinical Safety         | false        | C11           |
        | Workflow                | false        | C27           |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 2      |
        | Sln3       | PracticeMgr    | Sup 2      |
    And Solutions are linked to Capabilities
        | Solution       | Capability              |
        | MedicOnline    | Appointments Management |
        | MedicOnline    | Clinical Safety         |
        | MedicOnline    | Prescribing             |
        | TakeTheRedPill | Prescribing             |
        | TakeTheRedPill | Appointments Management |
        | TakeTheRedPill | Workflow                |
        | PracticeMgr    | Clinical Safety         |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |

@2649
Scenario: All the Foundation Capabilities and no other Capabilities are selected, only Solutions (Single) that deliver all the Foundation Capabilities are included
    When a POST request is made containing the capabilities Prescribing,Appointments Management
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill are found in the response
    And the solutions PracticeMgr are not found in the response

@2649
Scenario: All the Foundation Capabilities and one or more other Capabilities are selected, only Solutions that deliver all the Foundation Capabilities and the other selected Capabilities are included
    When a POST request is made containing the capabilities Prescribing,Appointments Management,Clinical Safety
    Then a successful response is returned
    And the solutions MedicOnline are found in the response
    And the solutions PracticeMgr,TakeTheRedPill are not found in the response
