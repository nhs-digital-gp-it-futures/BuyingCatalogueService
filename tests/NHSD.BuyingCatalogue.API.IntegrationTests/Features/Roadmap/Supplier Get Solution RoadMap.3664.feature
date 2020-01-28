Feature:  Display Marketing Page Form Solution RoadMap Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Solution RoadMap
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | RoadMap            |
        | Sln1     | some valid summary |
       
@3664
Scenario: 1. Solution RoadMap is retrieved for the solution
    When a GET request is made for roadmap section for solution Sln1
    Then a successful response is returned
    And the string value of element summary is some valid summary 
 
@3664
Scenario: 2. Solution RoadMap is retrieved empty for the solution where no solution detail exists
    When a GET request is made for roadmap section for solution Sln2
    Then a successful response is returned
    And the summary string does not exist

@3664
Scenario: 4. Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made for roadmap section for solution Sln3
    Then a response status of 404 is returned

@3664
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for roadmap section for solution Sln1
    Then a response status of 500 is returned

@3664
Scenario: 6. Solution id not present in request
    When a GET request is made for roadmap section with no solution id
    Then a response status of 400 is returned
