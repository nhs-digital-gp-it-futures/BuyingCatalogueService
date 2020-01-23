Feature: Display Marketing Page Form About Supplier Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's About Supplier Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName | Summary      | SupplierUrl |
        | Sup 1 | Supplier 1   | GPs-R-Us         | Some Summary | www.url.com |
        | Sup 2 | Supplier 1   | GPs-R-Us         | NULL         | NULL        |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | GPs-R-Us         | 1                | Sup 2      |

@3653
Scenario: 1. About Supplier is retreived for the solution
    When a GET request is made for about-supplier for solution Sln1
    Then a successful response is returned
    And the string value of element description is Some Summary
    And the string value of element link is www.url.com
    
@3653
Scenario: 2. About Supplier is retrieved for the solution where no about supplier exists
    When a GET request is made for about-supplier for solution Sln2
    Then a successful response is returned
    And the description string does not exist
    And the link string does not exist

@3653
Scenario: 3. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for about-supplier for solution Sln2
    Then a response status of 500 is returned

@3653
Scenario: 4. Solution id not present in request
    When a GET request is made for about-supplier with no solution id
    Then a response status of 400 is returned
