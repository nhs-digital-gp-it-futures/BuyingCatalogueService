Feature:  Supplier Edit Mobile Operating Systems
    As a Supplier
    I want to Edit the Mobile Operating Systems Section
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

@3605
Scenario: Mobile Operating Systems is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" } } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems | OperatingSystemsDescription |
        | Linux, Windows   | Added Linux                 |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Linux", "Windows"], "OperatingSystemsDescription": "Added Linux" } } |
        
@3605
Scenario: Mobile Operating Systems is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" } } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems                | OperatingSystemsDescription |
        | "    Linux    ", "Windows     " | "      Added Linux       "  |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "MobileOperatingSystems": { "OperatingSystems": ["Linux", "Windows"], "OperatingSystemsDescription": "Added Linux" } } |

@3605
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln2
      | OperatingSystems | OperatingSystemsDescription |
      | Linux            | Added Linux                 |
    Then a response status of 404 is returned 

@3605
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
       | OperatingSystems | OperatingSystemsDescription |
       | Linux, Windows   | Some more description       |
    Then a response status of 500 is returned

@3605
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-mobile-operating-systems section with no solution id
       | OperatingSystems | OperatingSystemsDescription |
       | Linux, Windows   | Added Linux                 |
    Then a response status of 400 is returned
