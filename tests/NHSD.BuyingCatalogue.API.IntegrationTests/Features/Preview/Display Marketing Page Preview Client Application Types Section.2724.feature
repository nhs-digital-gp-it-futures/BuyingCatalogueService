Feature:  Display Marketing Page Preview Client Application Type Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Client Application Types
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
        | Sln4       | PracticeMgr    | 1                | Sup 2      |
        | Sln5       | Potions        | 1                | Sup 1      |

    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                               |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported" : [ "Edge", "Chrome" ]  } |
        | Sln3     | Eye opening experience         | Eye opening6        |                                                                                                 |
        | Sln4     | Fully fledged GP system        | Fully fledged GP 12 | { "ClientApplicationTypes" : [] }                                                               |
        | Sln5     | Fully fledged GP system        | Fully fledged GP 12 | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                            |

@2724
Scenario: 1. When a client application type is selected and it contains data, client application types should show
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the client-application-types section contains 1 subsections
    And the client-application-types section contains subsection browser-based

@2724
Scenario Outline: 2. When SolutionDetail record does not exist, client application types should not show
    When a GET request is made for solution preview <Solution>
    Then a successful response is returned
    And the client-application-types section is missing
Examples:
    | Solution |
    | Sln2     |
    | Sln3     |

@2724
Scenario: 3. When SolutionDetail.ClientApplicationTypes is empty, client application types should not show
    When a GET request is made for solution preview Sln4
    Then a successful response is returned
    And the client-application-types section is missing
    
@2724
Scenario: 3. When ClientApplicationTypes is selected but there is no subsection data, client application types should not show
    When a GET request is made for solution preview Sln5
    Then a successful response is returned
    And the client-application-types section is missing
