Feature:  Display Marketing Page Preview Client Application Type Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Client Application Types
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
        | Sln4       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
        | Sln5       | Potions        | Lotions                        | GPs-R-Us         | Cauldronsinc.       | 1                |

    And MarketingDetail exist
        | Solution | ClientApplication                                                                               |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported" : [ "Edge", "Chrome" ]  } |
        | Sln3     |                                                                                                 |
        | Sln4     | { "ClientApplicationTypes" : [] }                                                               |
        | Sln5     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                            |

@2724
Scenario: 1. When a client application type is selected and it contains data, client application types should show
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the client-application-types section contains 1 subsections
    And the client-application-types section contains subsection browser-based

@2724
Scenario Outline: 2. When MarketingDetail record does not exist, client application types should not show
    When a GET request is made for solution preview <Solution>
    Then a successful response is returned
    And the client-application-types section is missing
Examples:
    | Solution |
    | Sln2     |
    | Sln3     |

@2724
Scenario: 3. When MarketingDetail.ClientApplicationTypes is empty, client application types should not show
    When a GET request is made for solution preview Sln4
    Then a successful response is returned
    And the client-application-types section is missing
    
@2724
Scenario: 3. When ClientApplicationTypes is selected but there is no subsection data, client application types should not show
    When a GET request is made for solution preview Sln5
    Then a successful response is returned
    And the client-application-types section is missing