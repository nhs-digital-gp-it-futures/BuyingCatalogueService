Feature:  Supplier Edit Mobile Operating Systems
    As a Supplier
    I want to Edit the Mobile Operating Systems Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
@3605
Scenario: 1. Mobile Operating Systems is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" } } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems | OperatingSystemsDescription |
        | Linux, Windows   | Added Linux                 |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Linux", "Windows"], "OperatingSystemsDescription": "Added Linux" } } |
        
@3605
Scenario: 2. Mobile Operating Systems is updated with trimmed whitespace
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" } } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems                | OperatingSystemsDescription |
        | "    Linux    ", "Windows     " | "      Added Linux       "  |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Linux", "Windows"], "OperatingSystemsDescription": "Added Linux" } } |

@3605
Scenario: 3. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln2
      | OperatingSystems | OperatingSystemsDescription |
      | Linux            | Added Linux                 |
    Then a response status of 404 is returned 

@3605
Scenario: 4. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
       | OperatingSystems | OperatingSystemsDescription |
       | Linux, Windows   | Some more description       |
    Then a response status of 500 is returned

@3605
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-operating-systems section with no solution id
       | OperatingSystems | OperatingSystemsDescription |
       | Linux, Windows   | Added Linux                 |
    Then a response status of 400 is returned
