Feature: Display Marketing Page Public Capabilities Epics Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Epics
    So that I can ensure the information is correct

Background:
	Given Capabilities exist
		| CapabilityName          | CapabilityRef | IsFoundation | Version | Description | SourceUrl                          |
		| Appointments Management | C1            | true         | 2.0     | AM          | http://appointments.management.com |		
	And Suppliers exist
		| Id    | SupplierName |
		| Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionId | SolutionName   | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill | 1                | Sup 1      |
	And Solutions are linked to Capabilities
		| Solution       | Capability              | Pass |
		| MedicOnline    | Appointments Management | True |
		| TakeTheRedPill | Appointments Management | True |
    And Epics exist
        | Id   | CapabilityRef | Name              | CompliancyLevel | Active |
        | C1E1 | C1            | Epic Must Met     | Must            | True   |
        | C1E2 | C1            | Epic Must Not Met | Must            | True   |
        | C1E3 | C1            | Epic May Met      | May             | True   |
        | C1E4 | C1            | Epic May Not Met  | May             | True   |
        | C1E5 | C1            | Epic InActive     | May             | False  |
    And Solutions are linked to Epics
        | SolutionId | EpicIds        | Status       |
        | Sln1       | C1E1,C1E3,C1E5 | Passed       |
        | Sln1       | C1E2,C1E4      | NotEvidenced |
        | Sln2       | C1E5           | NotEvidenced |

@3681
Scenario: 1. Sections presented where Epics exist
	When a GET request is made for solution public Sln1
	Then a successful response is returned
	And the response contains the following values
		| Section      | Field                                         | Value                              |
		| capabilities | capabilities-met[0].name                      | Appointments Management            |
		| capabilities | capabilities-met[0].version                   | 2.0                                |
		| capabilities | capabilities-met[0].description               | AM                                 |
		| capabilities | capabilities-met[0].link                      | http://appointments.management.com |
		| capabilities | capabilities-met[0].epic.must.met[0].id       | C1E1                               |
		| capabilities | capabilities-met[0].epic.must.met[0].name     | Epic Must Met                      |
		| capabilities | capabilities-met[0].epic.must.not-met[0].id   | C1E2                               |
		| capabilities | capabilities-met[0].epic.must.not-met[0].name | Epic Must Not Met                  |
		| capabilities | capabilities-met[0].epic.may.met[0].id        | C1E3                               |
		| capabilities | capabilities-met[0].epic.may.met[0].name      | Epic May Met                       |
		| capabilities | capabilities-met[0].epic.may.not-met[0].id    | C1E4                               |
		| capabilities | capabilities-met[0].epic.may.not-met[0].name  | Epic May Not Met                   |
	And the response contains lists with the following counts
        | Section      | Field                                 | Count |
        | capabilities | capabilities-met                      | 1     |
        | capabilities | capabilities-met[0].epic.must.met     | 1     |
        | capabilities | capabilities-met[0].epic.must.not-met | 1     |
        | capabilities | capabilities-met[0].epic.may.met      | 1     |
        | capabilities | capabilities-met[0].epic.may.not-met  | 1     |

@3681
Scenario: 2. Sections not presented where no Epics exist
	When a GET request is made for solution public Sln2
	Then a successful response is returned
    And the response contains the following values
		| Section      | Field                    | Value                   |
		| capabilities | capabilities-met[0].name | Appointments Management |
	And the response does not contain the following fields
        | Section      | Field                    |
        | capabilities | capabilities-met[0].epic |
