Feature: Supplier Submitted for Moderation State Change
	As a Supplier or Authority User
    I want the State of the Marketing Page to change after Submission of the Marketing Page

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |

@3012
Scenario: 1. Supplier status successfully updated upon Solution submitted for review
    Given Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | ClientApplication                                                                                                                             |
        | Sln1     | An full online medicine system | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"], "MobileResponsive": true, "Plugins": { "Required": false } } |
	When a request is made to submit Solution Sln1 for review
    Then a successful response is returned
    And Last Updated has updated on the SolutionEntity for solution Sln1

@3012
Scenario: 2. Supplier status not updated due to missing Solution summary
    Given Solutions exist
        | SolutionId | SolutionName | SummaryDescription | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  |                    | 1                | Sup 1      |
	When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
