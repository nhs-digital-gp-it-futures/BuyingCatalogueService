Feature: Foundation Solution (Single) Capability filtered browse
    As a Public User
    I want to use Foundation Capabilities when browsing Solutions
    So that I know what Solutions (Single) include those Capabilities

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Clinical Safety         | false        |
        | Workflow                | false        |
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
        | MedicOnline    | Prescribing             |
        | TakeTheRedPill | Prescribing             |
        | TakeTheRedPill | Appointments Management |
        | TakeTheRedPill | Workflow                |
        | PracticeMgr    | Clinical Safety         |

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
