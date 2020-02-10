Feature: Display Marketing Page Dashboard Authority Epic Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Epics
    So that I can ensure the information is correct

Background:
     Given Suppliers exist
	   | Id    | SupplierName |
	   | Sup 1 | Supplier 1   |
     And Solutions exist
	   | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
	   | Sln1       | MedicOnline    | 1                | Sup 1      |

@3681
Scenario: 1. Epics section is mandatory and is reported incomplete
    When a GET request is made for solution authority dashboard Sln1
    Then a successful response is returned
    And the solution epics section requirement is Mandatory
    And the solution epics section status is INCOMPLETE
