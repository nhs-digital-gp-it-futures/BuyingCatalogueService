Feature: Display Marketing Page Form About Supplier Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's About Supplier Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName | Summary      | SupplierUrl |
        | Sup 1 | Supplier 1   | Some Summary | www.url.com |
        | Sup 2 | Supplier 1   | NULL         | NULL        |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 2      |

@3653
Scenario: About Supplier is retreived for the solution
    When a GET request is made for about-supplier section for solution Sln1
    Then a successful response is returned
    And the string value of element description is Some Summary
    And the string value of element link is www.url.com

@3653
Scenario: About Supplier is retrieved for the solution where no about supplier exists
    When a GET request is made for about-supplier section for solution Sln2
    Then a successful response is returned
    And the description string does not exist
    And the link string does not exist

@3653
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for about-supplier section for solution Sln2
    Then a response status of 500 is returned

@3653
Scenario: Solution id not present in request
    When a GET request is made for about-supplier section with no solution id
    Then a response status of 400 is returned
