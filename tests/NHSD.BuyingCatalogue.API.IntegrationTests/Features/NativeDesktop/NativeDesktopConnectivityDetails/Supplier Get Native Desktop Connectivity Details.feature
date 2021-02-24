Feature:  Display Marketing Page Form Native Desktop Connectivity Details Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Connectivity Details
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName     | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | 1                | Sup 1      |
        | Sln3       | TakeTheGreenPill | 1                | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription            | FullDescription   | ClientApplication                                                                                |
        | Sln1       | A full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-desktop"], "NativeDesktopMinimumConnectionSpeed": "6Mbps" } |
        | Sln2       | An online medicine system     | Online medicine 2 | { "ClientApplicationTypes": ["native-desktop"] }                                                 |

@3619
Scenario: Native Mobile Desktop Connectivity Details are retreived for the solution
    When a GET request is made for native-desktop-connection-details section for solution Sln1
    Then a successful response is returned
    And the string value of element minimum-connection-speed is 6Mbps
    
@3619
Scenario: Native Desktop Connectivity Details are retrieved for the solution where no hardware requirements exist
    When a GET request is made for native-desktop-connection-details section for solution Sln2
    Then a successful response is returned
    And the minimum-connection-speed string does not exist

@3619
Scenario: Native Desktop Connectivity Details are retrieved for the solution where no solution detail exists
    When a GET request is made for native-desktop-connection-details section for solution Sln3
    Then a successful response is returned
    And the minimum-connection-speed string does not exist

@3619
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-connection-details section for solution Sln4
    Then a response status of 404 is returned

@3619
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-connection-details section for solution Sln2
    Then a response status of 500 is returned

@3619
Scenario: Solution id not present in request
    When a GET request is made for native-desktop-connection-details section with no solution id
    Then a response status of 400 is returned
