Feature: Supplier Submit for Moderation
	As a Supplier
    I want to submit my Marketing Page for Moderation
    So that my Marketing Page can be approved and published

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         |

@2836
Scenario: 1. Solution successfully submitted for review
    Given a Solution Sln1 exists
    When a request is made to submit Solution Sln1 for review
    Then a response status of 204 is returned

@2836
Scenario: 2. Solution not found
    Given a Solution Sln2 does not exist
    When a request is made to submit Solution Sln2 for review
    Then a response status of 404 is returned

@2836
Scenario: 3. Solution id not present in request
    Given a Solution Sln1 exists
    When a request is made to submit Solution for review with no solution id
    Then a response status of 400 is returned

@2836
@ignore
Scenario: 4. Service failure
    Given a Solution Sln1 exists
        And the call to the database to set the field fails
    When a request is made to submit Solution Sln1 for review
    Then a response status of 500 is returned
