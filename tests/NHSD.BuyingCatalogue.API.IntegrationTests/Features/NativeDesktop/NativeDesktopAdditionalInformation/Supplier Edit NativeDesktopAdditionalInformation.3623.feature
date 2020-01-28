Feature:  Supplier Edit Native Desktop Additional Information
    As a Supplier
    I want to Edit the Native Desktop Additional Information Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |

@3623
Scenario: 1. Native Desktop Additional Information is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                      |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeDesktopAdditionalInformation": "New Additional Info" } |
        
@3623
Scenario: 2. Native Desktop Additional Information is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation          |
        | "      New Additional Info   " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                      |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeDesktopAdditionalInformation": "New Additional Info" } |

@3623
Scenario: 3. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln2
       | AdditionalInformation |
       | New Additional Info   |
    Then a response status of 404 is returned 

@3623
Scenario: 4. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 500 is returned

@3623
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the native-desktop-additional-information section with no solution id
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 400 is returned

@3623
Scenario: 6. AdditionalInformation is set to null
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopAdditionalInformation": "Some additional info" } |
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation |
        | NULL                  |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [] } |
