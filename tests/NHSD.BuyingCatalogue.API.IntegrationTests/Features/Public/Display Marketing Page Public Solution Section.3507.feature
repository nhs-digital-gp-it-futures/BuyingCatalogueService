Feature: Display Marketing Page Public Solution Section
	As a Supplier
    I want to manage Marketing Page Information for the Solution Section
    So that I can ensure the information is correct

Background:
    And Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | LastUpdated | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 19/11/2019  | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 15/11/2019  | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 20/11/2019  | 1                | Sup 2      |
    And Framework Solutions exist
        | SolutionId | IsFoundation |
        | Sln1       | true         |
        | Sln2       | false        |

@3507
Scenario: 1. Solution section is presented where the solution isFoundation is true
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solution organisationName is GPs-R-Us
    And the solution IsFoundation is true
    And the last updated date in the solution is 19/11/2019

@3507
Scenario: 2. Solution section is presented where the solution isFoundation is false
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solution organisationName is Drs. Inc
    And the solution IsFoundation is false
    And the last updated date in the solution is 15/11/2019

@3507
Scenario: 3. Solution is not linked to Framwork Solution
    When a GET request is made for solution public Sln3
    Then a successful response is returned
    And the solution organisationName is Drs. Inc
    And the solution IsFoundation is false
    And the last updated date in the solution is 20/11/2019
