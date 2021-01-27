Feature: Update Capabilities Validation
    As a Public User
    I want to update the Capabilities for a Solution to check it is valid
    So that I can modify who the Solution Capabilities are

Background:
    Given Capabilities exist
        | CapabilityName      | CapabilityRef | IsFoundation |
        | Resource Management | C1            | false        |
        | Prescribing         | C2            | true         |
        | Workflow            | C3            | true         |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
    And Solutions are linked to Capabilities
        | Solution             | Capability          |
        | TakeTheRedPill       | Resource Management |

@3678
Scenario: A Capability that has no existing Capability Reference is added
    When a PUT request is made to update the capabilities section for solution Sln1
        | CapabilityRefs |
        | TEST           |
    Then a response status of 400 is returned
    And the capabilities field value is the validation failure capabilityInvalid
    Then Solutions are linked to Capabilities
        | SolutionId | CapabilityRefs |
        | Sln1       |                |
        | Sln2       | C1             |

@3678
Scenario: Muliple Capabilies are added with one that has no existing Capability Reference
    When a PUT request is made to update the capabilities section for solution Sln2
        | CapabilityRefs |
        | C1, TEST, C2   |
    Then a response status of 400 is returned
    And the capabilities field value is the validation failure capabilityInvalid
    Then Solutions are linked to Capabilities
        | SolutionId | CapabilityRefs |
        | Sln1       |                |
        | Sln2       | C1             |
