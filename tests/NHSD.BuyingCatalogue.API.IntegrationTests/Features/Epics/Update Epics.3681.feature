Feature: Update Epics
    As a Public User
    I want to update the Epics for a Solution
    So that I can modify what the Solution Epics are

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
	And Epics exist
		| Id    | CapabilityRef |
		| Epic1 | C1            |
		| Epic2 | C2            |
		| Epic3 | C3            |
	And Solutions are linked to Epics
		| SolutionId | EpicIds      |
		| Sln2       | Epic2, Epic3 |

@3681
Scenario: 1. An Epic is added to a solution which has no epics
	When a PUT request is made to update a epics section for solution Sln1
		| EpicId | StatusName |
		| Epic1  | Passed     |
	Then a successful response is returned
	Then Solutions are linked to Epics
		| SolutionId | EpicIds      |
		| Sln1       | Epic1        |
		| Sln2       | Epic2, Epic3 |

@3681
Scenario: 2. An Epic is added to a solution which has multiple epics
	When a PUT request is made to update a epics section for solution Sln2
		| EpicId | StatusName |
		| Epic1  | Passed     |
	Then a successful response is returned
	Then Solutions are linked to Epics
		| SolutionId | EpicIds |
		| Sln2       | Epic1   |

Scenario: 3. Multiple Epics are added to a solution which has no epics
	When a PUT request is made to update a epics section for solution Sln1
		| EpicId | StatusName    |
		| Epic1  | Passed        |
		| Epic2  | Not Evidenced |
	Then a successful response is returned
	Then Solutions are linked to Epics
		| SolutionId | EpicIds      |
		| Sln1       | Epic1, Epic2 |
		| Sln2       | Epic2, Epic3 |

Scenario: 4. Multiple Epics are added to a solution which has multiple epics
	When a PUT request is made to update a epics section for solution Sln2
		| EpicId | StatusName |
		| Epic1  | Passed     |
		| Epic3  | Passed     |
	Then a successful response is returned
	Then Solutions are linked to Epics
		| SolutionId | EpicIds      |
		| Sln2       | Epic1, Epic3 |
