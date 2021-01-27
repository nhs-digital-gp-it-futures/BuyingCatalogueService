Feature: GetSolutionDashboardExceptions
    When a solution cannot be returned from the service
    I want to provide an appropriate error code

@1793
Scenario: Solution not found
    Given a Solution Sln2 does not exist
    When a GET request is made for solution dashboard Sln2 
    Then a response status of 404 is returned

@1793
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for solution dashboard Sln2
    Then a response status of 500 is returned

@1793
Scenario: Solution id not present in request
    When a GET request is made for dashboard with no solution id
    Then a response status of 400 is returned
