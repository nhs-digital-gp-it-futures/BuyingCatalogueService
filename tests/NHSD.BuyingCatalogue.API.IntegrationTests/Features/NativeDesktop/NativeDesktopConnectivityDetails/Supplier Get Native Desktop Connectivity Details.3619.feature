Feature:  Display Marketing Page Form Native Desktop Connectivity Details Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Connectivity Details
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | TakeTheGreenPill | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription            | FullDescription   | ClientApplication                                                                                |
        | Sln1     | A full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-desktop"], "NativeDesktopMinimumConnectionSpeed": "6Mbps" } |
        | Sln2     | An online medicine system     | Online medicine 2 | { "ClientApplicationTypes": ["native-desktop"] }                                                 |

@3619
Scenario: 1.Native Mobile Desktop Connectivity Details are retreived for the solution
    When a GET request is made for native-desktop-connection-details for solution Sln1
    Then a successful response is returned
    And the string value of element minimum-connection-speed is 6Mbps
    
@3619
Scenario: 2. Native Desktop Connectivity Details are retrieved for the solution where no hardware requirements exist
    When a GET request is made for native-desktop-connection-details for solution Sln2
    Then a successful response is returned
    And the minimum-connection-speed string does not exist

@3619
Scenario: 3. Native Desktop Connectivity Details are retrieved for the solution where no solution detail exists
    When a GET request is made for native-desktop-connection-details for solution Sln3
    Then a successful response is returned
    And the minimum-connection-speed string does not exist

@3619
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-connection-details for solution Sln4
    Then a response status of 404 is returned

@3619
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-connection-details for solution Sln2
    Then a response status of 500 is returned

@3619
Scenario: 6. Solution id not present in request
    When a GET request is made for native-desktop-connection-details with no solution id
    Then a response status of 400 is returned
