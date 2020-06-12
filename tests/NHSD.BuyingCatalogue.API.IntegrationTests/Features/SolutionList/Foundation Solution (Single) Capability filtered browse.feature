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
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId | PublishedStatus |
        | Sln1       | MedicOnline    | 1                | Sup 1      | Published       |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      | Published       |
        | Sln3       | PracticeMgr    | 1                | Sup 2      | Published       |
    And Solutions are linked to Capabilities
        | Solution   | Capability              |
        | Sln1       | Appointments Management |
        | Sln1       | Clinical Safety         |
        | Sln1       | Prescribing             |
        | Sln2       | Prescribing             |
        | Sln2       | Appointments Management |
        | Sln2       | Workflow                |
        | Sln3       | Clinical Safety         |

@2649
Scenario: 1. All the Foundation Capabilities and no other Capabilities are selected, only Solutions (Single) that deliver all the Foundation Capabilities are included
    When a POST request is made containing the capabilities Prescribing,Appointments Management
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill are found in the response
    And the solutions PracticeMgr are not found in the response

@2649
Scenario: 2. All the Foundation Capabilities and one or more other Capabilities are selected, only Solutions that deliver all the Foundation Capabilities and the other selected Capabilities are included
    When a POST request is made containing the capabilities Prescribing,Appointments Management,Clinical Safety
    Then a successful response is returned
    And the solutions MedicOnline are found in the response
    And the solutions PracticeMgr,TakeTheRedPill are not found in the response
