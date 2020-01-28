Feature: Suppliers Edit Solution Supplier Section
    As a Supplier
    I want to Edit the Solution Supplier Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName | OrganisationName | Summary      | SupplierUrl |
        | Sup 1 | Supplier 1   | GPs-R-Us         | Some Summary | www.url.com |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |

@3653
Scenario: 1. Solution Supplier data is updated
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary      | SupplierUrl  |
        | More Summary | www.link.com |
    Then a successful response is returned
    And Suppliers exist
        | Id    | Summary      | SupplierUrl  |
        | Sup 1 | More Summary | www.link.com |
    And Last Updated has updated on the Supplier for supplier Sup 1

@3653
Scenario: 2. Solution Supplier data is updated with trimmed whitespace
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary                     | SupplierUrl            |
        | "     More Summary        " | "     www.link.com   " |
    Then a successful response is returned
     And Suppliers exist
         | Id    | Summary      | SupplierUrl  |
         | Sup 1 | More Summary | www.link.com |
    And Last Updated has updated on the Supplier for supplier Sup 1

@3653
Scenario: 3. Solution not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the about-supplier section for solution Sup 2
        | Summary      | SupplierUrl  |
        | More Summary | www.link.com |
    Then a response status of 404 is returned
    
@3653
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary      | SupplierUrl  |
        | More Summary | www.link.com |
    Then a response status of 500 is returned
    
@3653
Scenario: 5. Solution id not present in request
    When a PUT request is made to update the about-supplier section with no solution id
        | Summary      | SupplierUrl  |
        | More Summary | www.link.com |
    Then a response status of 400 is returned
