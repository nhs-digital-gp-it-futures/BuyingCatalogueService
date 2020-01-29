Feature:  Display Marketing Page Form Native Desktop Additional Information
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Additional Information
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopAdditionalInformation": "Some more info" } |
        | Sln3     | Testing System                 | Full System       | { "ClientApplicationTypes": ["native-desktop"] }           |

@3623
Scenario: 1. Native Desktop Additional Information are retreived for the solution
    When a GET request is made for native-desktop-additional-information section for solution Sln1
    Then a successful response is returned
    And the string value of element additional-information is Some more info

@3623
Scenario: 2. Native Desktop Additional Information are retrieved for the solution where no solutiondetail exists
    When a GET request is made for native-desktop-additional-information section for solution Sln2
    Then a successful response is returned
    And the additional-information string does not exist

@3623
Scenario: 3. Native Desktop Additional Information are retrieved for the solution where there is no additional information
    When a GET request is made for native-desktop-additional-information section for solution Sln3
    Then a successful response is returned
    And the additional-information string does not exist

@3623
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-additional-information section for solution Sln4
    Then a response status of 404 is returned

@3623
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-additional-information section for solution Sln1
    Then a response status of 500 is returned

@3623
Scenario: 6. Solution id not present in request
    When a GET request is made for native-desktop-additional-information section with no solution id
    Then a response status of 400 is returned

