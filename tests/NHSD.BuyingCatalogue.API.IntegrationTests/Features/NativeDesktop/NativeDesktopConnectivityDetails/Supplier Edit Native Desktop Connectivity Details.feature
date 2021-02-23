Feature:  Supplier Edit Native Desktop Connectivity Details
    As a Supplier
    I want to Edit the Native Desktop Connectivity Details Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
@3619
Scenario: Native Desktop Hardware Requirements is updated
    Given Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopMinimumConnectionSpeed": "3Mbps" } |
    When a PUT request is made to update the native-desktop-connection-details section for solution Sln1
        | NativeDesktopMinimumConnectionSpeed |
        | 6Mbps                               |
    Then a successful response is returned
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeDesktopMinimumConnectionSpeed": "6Mbps" } |
        
@3619
Scenario: Native Desktop Hardware Requirements is updated with trimmed whitespace
    Given Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopMinimumConnectionSpeed": "3Mbps" } |
    When a PUT request is made to update the native-desktop-connection-details section for solution Sln1
        | NativeDesktopMinimumConnectionSpeed |
        | "            6Mbps               "  |
    Then a successful response is returned
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeDesktopMinimumConnectionSpeed": "6Mbps" } |

@3619
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-desktop-connection-details section for solution Sln2
       | NativeDesktopMinimumConnectionSpeed |
       | 2Mbps                               |
    Then a response status of 404 is returned 

@3619
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-desktop-connection-details section for solution Sln1
        | NativeDesktopMinimumConnectionSpeed |
        | 10Mbps                              |
    Then a response status of 500 is returned

@3619
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-desktop-connection-details section with no solution id
        | NativeDesktopMinimumConnectionSpeed |
        | 4Mbps                               |
    Then a response status of 400 is returned
