Feature:  Native Desktop Operating Systems
    As a Supplier
    I want to Edit the Native Desktop Operating Systems Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
@3617
Scenario: Native Desktop Operating Systems is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-operating-systems section for solution Sln1
        | NativeDesktopOperatingSystemsDescription |
        | New Desc                                 |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeDesktopOperatingSystemsDescription": "New Desc" } |
        
@3617
Scenario: Native Desktop Operating Systems is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-operating-systems section for solution Sln1
        | NativeDesktopOperatingSystemsDescription |
        | "               New Desc             "   |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeDesktopOperatingSystemsDescription": "New Desc" } |

@3617
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-desktop-operating-systems section for solution Sln2
       | NativeDesktopOperatingSystemsDescription |
       | New Desc                                 |
    Then a response status of 404 is returned 

@3617
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-desktop-operating-systems section for solution Sln2
        | NativeDesktopOperatingSystemsDescription |
        | New Desc                                 |
    Then a response status of 500 is returned

@3617
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-desktop-operating-systems section with no solution id
        | NativeDesktopOperatingSystemsDescription |
        | New Desc                                 |
    Then a response status of 400 is returned
