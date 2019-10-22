Feature:  Display Marketing Page Form Client Application Type Section
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

    And MarketingDetail exist
        | Solution | ClientApplication                                                    |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] } |
        | Sln3     |                                                                      |
        | Sln4     | { "ClientApplicationTypes" : [] }                                    |


@2724
Scenario: 1. Sections presented where the Solution exists
    When a GET request is made for solution Sln1
    Then a successful response is returned
    And the solution client-application-types section contains clientapplication
        | ClientApplication |
        | browser-based     |
        | native-desktop    |
    And the solution client-application-types section status is COMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario Outline: 2.Sections presented where the MarketingDetail does not exist
    When a GET request is made for solution <Solution>
    Then a successful response is returned
    And the solution client-application-types section contains clientapplication
        | ClientApplication |
    And the solution client-application-types section status is INCOMPLETE

Examples:
    | Solution |
    | Sln2     |
    | Sln3     |

@2724
Scenario: 3. Sections presented where the Solution exists
    When a GET request is made for solution Sln4
    Then a successful response is returned
    And the solution client-application-types section contains clientapplication
        | ClientApplication |
    And the solution client-application-types section status is INCOMPLETE
