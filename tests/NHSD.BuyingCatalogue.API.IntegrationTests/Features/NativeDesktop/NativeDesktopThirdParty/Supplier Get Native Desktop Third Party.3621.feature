Feature:  Display Marketing Page Form Native Desktop Third Party Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Third Party
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | TakeTheGreenPill | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription            | FullDescription   | ClientApplication                                                                                                                                          |
        | Sln1     | A full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-desktop"], "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" } } |
        | Sln2     | An online medicine system     | Online medicine 2 | { "ClientApplicationTypes": ["native-desktop"] }                                                                                                           |

@3621
Scenario: 1.Native Mobile Third Party are retreived for the solution
    When a GET request is made for native-desktop-third-party section for solution Sln1
    Then a successful response is returned
    And the string value of element third-party-components is Component
    And the string value of element device-capabilities is Capabilities
    
@3621
Scenario: 2. Native Desktop Third Party is retrieved for the solution where no native third partys exist
    When a GET request is made for native-desktop-third-party section for solution Sln2
    Then a successful response is returned
    And the third-party-components string does not exist
    And the device-capabilities string does not exist

@3621
Scenario: 3. Native Desktop Third Party is retrieved for the solution where no solution detail exists
    When a GET request is made for native-desktop-third-party section for solution Sln3
    Then a successful response is returned
    And the third-party-components string does not exist
    And the device-capabilities string does not exist

@3621
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-third-party section for solution Sln4
    Then a response status of 404 is returned

@3621
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-third-party section for solution Sln2
    Then a response status of 500 is returned

@3621
Scenario: 6. Solution id not present in request
    When a GET request is made for native-desktop-third-party section with no solution id
    Then a response status of 400 is returned
