Feature: Display Marketing Page Dashboard Authority Epic Section
    As an authority user
    I want to manage marketing page information for the Solution's epics
    So that I can ensure the information is correct

Background:
	Given Capabilities exist
		| CapabilityName      | CapabilityRef | IsFoundation |
		| Resource Management | C1            | false        |
		| Prescribing         | C2            | true         |
    Given Epics exist
       | Id | CapabilityRef | Active |
       | E1 | C1            | true   |
       | E2 | C1            | true   |
       | E3 | C2            | false  |
     And Suppliers exist
	   | Id    | SupplierName |
	   | Sup 1 | Supplier 1   |
     And Solutions exist
	   | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
	   | Sln1       | MedicOnline    | 1                | Sup 1      |

@3681
Scenario: 1. Epics section should be mandatory
    When a GET request is made for solution authority dashboard Sln1
    Then a successful response is returned
    And the solution epics section requirement is Mandatory
    
@3681
Scenario Outline: 1. Epics section should be marked as complete when an active claimed epic exists and a claimed capability exists
    Given Solutions are linked to Capabilities
		| Solution       | Capability   |
		| <SolutionName> | <Capability> |
    And Solutions are linked to Epics
        | SolutionId | EpicIds   |
        | <Solution> | <EpicIds> |
    When a GET request is made for solution authority dashboard Sln1
    Then a successful response is returned
    And the solution epics section status is <Status>
    Examples:
        | Solution | SolutionName | Capability          | EpicIds | Status     |
        | Sln1     | MedicOnline  |                     |         | INCOMPLETE |
        | Sln1     | MedicOnline  |                     | E1      | INCOMPLETE |
        | Sln1     | MedicOnline  | Resource Management |         | INCOMPLETE |
        | Sln1     | MedicOnline  | Prescribing         | E2      | INCOMPLETE |
        | Sln1     | MedicOnline  | Prescribing         | E3      | INCOMPLETE |
        | Sln1     | MedicOnline  | Resource Management | E1      | COMPLETE   |
        | Sln1     | MedicOnline  | Resource Management | E1,E2   | COMPLETE   |
