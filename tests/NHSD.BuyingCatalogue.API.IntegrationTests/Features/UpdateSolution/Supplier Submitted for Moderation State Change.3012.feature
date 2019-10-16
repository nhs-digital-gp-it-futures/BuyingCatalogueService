Feature: Supplier Submitted for Moderation State Change
	As a Supplier or Authority User
    I want the State of the Marketing Page to change after Submission of the Marketing Page

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |

@3012
Scenario: 1. Supplier status successfully updated upon Solution submitted for review
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  | An full online medicine system | GPs-R-Us         | 1                |
	When a request is made to submit Solution Sln1 for review
    Then a successful response is returned
    And the field [SupplierStatusId] for Solution Sln1 should correspond to 'Authority Review'

@3012
Scenario: 1. Supplier status not updated due to missing Solution summary
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  |                    | GPs-R-Us         | 1                |
	When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
    And the field [SupplierStatusId] for Solution Sln1 should correspond to 'Draft'
