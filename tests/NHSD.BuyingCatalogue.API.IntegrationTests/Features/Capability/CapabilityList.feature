Feature: CapabilityList
    As a Public User
    I want to browse the Capabilities
    So that I know what those Capabilities are

Scenario: Should Read Capabilities
    Given Capabilities exist
        | CapabilityRef | Version | CapabilityName          | IsFoundation | CategoryId |
        | C17           | 1.0.1   | Scanning                | true         | 3          |
        | C5            | 1.0.1   | Resource Management     | false        | 1          |
        | C10           | 1.0.2   | Prescribing             | true         | 1          |
        | C29           | 1.0.2   | Telehealth              | false        | 2          |
        | C11           | 1.0.3   | Unified Care Record     | false        | 3          |
        | C39           | 1.0.3   | Workflow                | true         | 1          |
        | C27           | 1.0.4   | Clinical Safety         | false        | 1          |
        | C1            | 1.0.5   | Appointments Management | true         | 1          |
        | C41           | 1.0.5   | Digital Diagnostics     | true         | 2          |
    When a GET request is made for the capability list
    Then a successful response is returned
    And  the capabilities are returned ordered by Capability Name containing the values
        | CapabilityRef | Version | CapabilityName          | IsFoundation |
        | C1            | 1.0.5   | Appointments Management | true         |
        | C27           | 1.0.4   | Clinical Safety         | false        |
        | C10           | 1.0.2   | Prescribing             | true         |
        | C5            | 1.0.1   | Resource Management     | false        |
        | C39           | 1.0.3   | Workflow                | true         |
