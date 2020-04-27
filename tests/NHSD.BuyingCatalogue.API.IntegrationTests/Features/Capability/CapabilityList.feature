Feature: CapabilityList
    As a Public User
    I want to browse the Capabilities
    So that I know what those Capabilities are

@mytag
Scenario: Should Read Capabilities
	Given Capabilities exist
		| CapabilityRef | Version | CapabilityName          | IsFoundation |
		| C5            | 1.0.1   | Resource Management     | false        |
		| C10           | 1.0.2   | Prescribing             | true         |
		| C11           | 1.0.3   | Workflow                | true         |
		| C27           | 1.0.4   | Clinical Safety         | false        |
		| C1            | 1.0.5   | Appointments Management | true         |
	When a GET request is made for the capability list
	Then a successful response is returned
	And  the capabilities are returned ordered by Capability Name containing the values
		| CapabilityRef | Version | CapabilityName          | IsFoundation |
		| C1            | 1.0.5   | Appointments Management | true         |
        | C27           | 1.0.4   | Clinical Safety         | false        |
		| C10           | 1.0.2   | Prescribing             | true         |
		| C5            | 1.0.1   | Resource Management     | false        |
		| C11           | 1.0.3   | Workflow                | true         |
		

