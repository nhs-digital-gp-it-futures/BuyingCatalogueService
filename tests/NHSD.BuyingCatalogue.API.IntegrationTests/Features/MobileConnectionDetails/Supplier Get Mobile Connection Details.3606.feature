Feature:  Display Marketing Page Form Mobile Connection Details Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Mobile Connection Details
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | PracticeMgr    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                              |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileConnectionDetails" : { "ConnectionType" : [ "3G", "4G", "5G" ], "MinimumConnectionSpeed": "1GBps", "Description": "A description" } } |
        | Sln3     | Testing System                 | Full System       | {  }                                                                                                                                           |

@3606
Scenario: 1. Mobile Connection Details are retrieved for the solution
    When a GET request is made for mobile-connection-details for solution Sln1
    Then a successful response is returned
    And the connection-type element contains
        | Elements |
        | 3G,4G,5G |
    And the string value of element minimum-connection-speed is 1GBps
    And the string value of element connection-requirements-description is A description

@3606
Scenario: 2. Mobile Operating Systems are retrieved for the solution where no solution detail exists
    When a GET request is made for mobile-connection-details for solution Sln2
    Then a successful response is returned
    And the connection-type element contains
        | Elements |
        |          |
    And the minimum-connection-speed string does not exist
    And the connection-requirements-description string does not exist

@3606
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for mobile-connection-details for solution Sln4
    Then a response status of 404 is returned

@3606
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for mobile-connection-details for solution Sln1
    Then a response status of 500 is returned

@3606
Scenario: 5. Solution id not present in request
    When a GET request is made for mobile-connection-details with no solution id
    Then a response status of 400 is returned
    
@3606
Scenario: 6. Mobile Connection Details are retrieved as empty if they do not exist yet
    When a GET request is made for mobile-connection-details for solution Sln3
    Then a successful response is returned
    And the connection-type element contains
        | Elements |
        |          |
    And the minimum-connection-speed string does not exist
    And the connection-requirements-description string does not exist
