Feature:  Display Marketing Page Form Native Mobile Third Party Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Mobile Third Party
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | Medics         | Drs. Inc         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                  |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-mobile"], "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" } } |
        | Sln2     | Testing System                 | Full System       | { "ClientApplicationTypes": ["native-mobile"] }                                                                                                    |

@3608
Scenario: 1.Native Mobile Third Party is retreived for the solution
    When a GET request is made for native-mobile-third-party for solution Sln1
    Then a successful response is returned
    And the string value of element third-party-components is Component
    And the string value of element device-capabilities is Capabilities

@3608
Scenario: 2. SolutionDetail does not have a Mobile Third Party
    When a GET request is made for native-mobile-third-party for solution Sln2
    Then a successful response is returned
    And the third-party-components string does not exist
    And the device-capabilities string does not exist

@3608
Scenario: 3. Native Mobile Third Party is retrieved for the solution where no solutiondetail exists
    When a GET request is made for native-mobile-third-party for solution Sln3
    Then a successful response is returned
    And the third-party-components string does not exist
    And the device-capabilities string does not exist

@3608
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-mobile-third-party for solution Sln4
    Then a response status of 404 is returned

@3608
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-mobile-third-party for solution Sln1
    Then a response status of 500 is returned

@3608
Scenario: 6. Solution id not present in request
    When a GET request is made for native-mobile-third-party with no solution id
    Then a response status of 400 is returned

