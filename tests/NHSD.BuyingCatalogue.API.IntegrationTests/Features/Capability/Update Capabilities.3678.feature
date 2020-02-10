Feature: Update Capabilities
    As a Public User
    I want to update the Capabilities for a Solution
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
		| SolutionID | SolutionName   | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill | 1                | Sup 1      |
	And Solutions are linked to Capabilities
		| Solution       | Capability          |
		| TakeTheRedPill | Resource Management |

@3678
Scenario: 1. A Capability is added to a solution which has no capabilities
	When a PUT request is made to update the capabilities section for solution Sln1
		| CapabilityRefs |
		| C2             |
	Then a successful response is returned
	Then Solutions are linked to Capabilities
		| SolutionId | CapabilityRefs |
		| Sln2       | C1             |
		| Sln1       | C2             |

@3678
Scenario: 2. A Capability is added to a solution which has has a capability
	When a PUT request is made to update the capabilities section for solution Sln2
		| CapabilityRefs |
		| C2             |
	Then a successful response is returned
	Then Solutions are linked to Capabilities
		| SolutionId | CapabilityRefs |
		| Sln2       | C2             |

@3678
Scenario: 3. Multiple Capabilies are added to a solution which has has a capability
	When a PUT request is made to update the capabilities section for solution Sln2
		| CapabilityRefs |
		| C1, C2, C3     |
	Then a successful response is returned
	Then Solutions are linked to Capabilities
		| SolutionId | CapabilityRefs |
		| Sln2       | C1, C2, C3     |

@3678
Scenario: 4. Capabilies are removed for a solution which has has a capability
	When a PUT request is made to update the capabilities section for solution Sln2
		| CapabilityRefs |
	Then a successful response is returned
	Then Solutions are linked to Capabilities
		| SolutionId | CapabilityRefs |
		| Sln2       |                |

@3678
Scenario: 5. A Capability is added for a solution which has has multiple capabilies
	And Solutions are linked to Capabilities
		| Solution    | Capability          |
		| MedicOnline | Resource Management |
		| MedicOnline | Prescribing         |
	When a PUT request is made to update the capabilities section for solution Sln1
		| CapabilityRefs |
		| C3             |
	Then a successful response is returned
	Then Solutions are linked to Capabilities
		| SolutionId | CapabilityRefs |
		| Sln1       | C3             |

@3678
Scenario: 6. Two of the same capability references are added, only one is linked.
    When a PUT request is made to update the capabilities section for solution Sln1
		| CapabilityRefs |
		| C1, C1         |
    Then a response status of 204 is returned
    Then Solutions are linked to Capabilities
		| SolutionId | CapabilityRefs |
		| Sln1       | C1             |
		| Sln2       | C1             |
