Feature:  Display Marketing Page Form Solution RoadMap Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Solution RoadMap
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
    And solutions have the following details
        | SolutionId | RoadMap            |
        | Sln1       | some valid summary |
       
@3664
Scenario: Solution RoadMap is retrieved for the solution
    When a GET request is made for roadmap section for solution Sln1
    Then a successful response is returned
    And the string value of element summary is some valid summary 
 
@3664
Scenario: Solution RoadMap summary is not retrieved when no RoadMap exists
    When a GET request is made for roadmap section for solution Sln2
    Then a successful response is returned
    And the summary string does not exist

@3664
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made for roadmap section for solution Sln3
    Then a response status of 404 is returned

@3664
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for roadmap section for solution Sln1
    Then a response status of 500 is returned

@3664
Scenario: Solution id not present in request
    When a GET request is made for roadmap section with no solution id
    Then a response status of 400 is returned
