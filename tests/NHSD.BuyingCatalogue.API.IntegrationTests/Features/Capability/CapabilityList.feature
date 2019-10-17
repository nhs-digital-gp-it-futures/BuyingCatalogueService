Feature: CapabilityList
    As a Public User
    I want to browse the Capabilities
    So that I know what those Capabilities are

@mytag
Scenario: Should Read Capabilities
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Resource Management     | false        |
        | Prescribing             | true         |
        | Workflow                | true         |
        | Clinical Safety         | false        |
        | Appointments Management | true         |
    When a GET request is made for the capability list
    Then a successful response is returned
    And  the capabilities are returned ordered by IsFoundation then Capability Name
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Workflow                | true         |
        | Clinical Safety         | false        |
        | Resource Management     | false        |
