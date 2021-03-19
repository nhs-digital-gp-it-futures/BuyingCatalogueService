Feature:  Display Marketing Page Public Client Application Type Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Client Application Types
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
        | Sln2       | PracticeMgr  | Sup 2      |
        | Sln3       | PracticeMgr  | Sup 2      |
        | Sln4       | Potions      | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                               |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported" : [ "Edge", "Chrome" ]  } |
        | Sln2       | Eye opening experience         | Eye opening6        |                                                                                                 |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 | { "ClientApplicationTypes" : [] }                                                               |
        | Sln4       | Fully fledged GP system        | Fully fledged GP 12 | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                            |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |
        | Sln4       | false        | NHSDGP001   |

@2724
Scenario: When a client application type is selected and it contains data, client application types should show
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the client-application-types section contains 1 subsections
    And the client-application-types section contains subsection browser-based

@2724
Scenario Outline: When Solution client application types does not exist
    When a GET request is made for solution public <SolutionId>
    Then a successful response is returned
    And the client-application-types section is missing
Examples:
    | SolutionId |
    | Sln2       |

@2724
Scenario: When Solution.ClientApplicationTypes is empty, client application types should not show
    When a GET request is made for solution public Sln3
    Then a successful response is returned
    And the client-application-types section is missing
    
@2724
Scenario: When ClientApplicationTypes is selected but there is no subsection data, client application types should not show
    When a GET request is made for solution public Sln4
    Then a successful response is returned
    And the client-application-types section is missing
