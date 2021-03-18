Feature:  Supplier Edit Native Desktop Additional Information
    As a Supplier
    I want to Edit the Native Desktop Additional Information Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3623
Scenario: Native Desktop Additional Information is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                      |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeDesktopAdditionalInformation": "New Additional Info" } |
        
@3623
Scenario: Native Desktop Additional Information is updated with leading and trailing white space
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation          |
        | "      New Additional Info   " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                      |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeDesktopAdditionalInformation": "New Additional Info" } |

@3623
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln2
       | AdditionalInformation |
       | New Additional Info   |
    Then a response status of 404 is returned 

@3623
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 500 is returned

@3623
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-desktop-additional-information section with no solution id
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 400 is returned

@3623
Scenario: AdditionalInformation is set to null
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopAdditionalInformation": "Some additional info" } |
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation |
        | NULL                  |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [] } |
