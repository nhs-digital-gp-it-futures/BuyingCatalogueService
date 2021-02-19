Feature:  Display Marketing Page Form Solution Integrations Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Solution Integrations
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | IntegrationsUrl |
        | Sln1       | some valid url  |

@3667
Scenario: Solution Integrations is retrieved for the solution
    When a GET request is made for integrations section for solution Sln1
    Then a successful response is returned
    And the string value of element link is some valid url

@3667
Scenario: Solution Integrations is retrieved empty for the solution where no solution detail exists
    When a GET request is made for integrations section for solution Sln2
    Then a successful response is returned
    And the link string does not exist

@3667
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made for integrations section for solution Sln3
    Then a response status of 404 is returned

@3667
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for integrations section for solution Sln1
    Then a response status of 500 is returned

@3667
Scenario: Solution id not present in request
    When a GET request is made for integrations section with no solution id
    Then a response status of 400 is returned
