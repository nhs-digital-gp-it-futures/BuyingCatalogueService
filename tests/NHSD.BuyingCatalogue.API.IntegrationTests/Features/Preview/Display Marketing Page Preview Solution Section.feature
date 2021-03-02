Feature: Display Marketing Page Preview Solution Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | LastUpdated | SupplierId |
        | Sln1       | MedicOnline    | 19/11/2019  | Sup 1      |
        | Sln2       | TakeTheRedPill | 15/11/2019  | Sup 2      |
        | Sln3       | PracticeMgr    | 20/11/2019  | Sup 2      |
    And Framework Solutions exist
        | SolutionId | IsFoundation |
        | Sln1       | true         |
        | Sln2       | false        |

@3507
Scenario: Solution section is presented where the solution isFoundation is true
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the string value of element supplierName is Supplier 1
    And the string value of element isFoundation is True
    And the last updated date in the solution is 19/11/2019

@3507
Scenario: Solution section is presented where the solution isFoundation is false
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the string value of element supplierName is Supplier 2
    And the string value of element isFoundation is False
    And the last updated date in the solution is 15/11/2019

@3507
Scenario: Solution is not linked to Framework Solution
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the string value of element supplierName is Supplier 2
    And the string value of element isFoundation is False
    And the last updated date in the solution is 20/11/2019
