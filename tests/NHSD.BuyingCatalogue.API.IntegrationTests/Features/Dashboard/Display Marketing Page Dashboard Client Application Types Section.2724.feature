Feature:  Display Marketing Page Dashboard Client Application Type Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Client Application Types
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
        | Sln4       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
        | Sln5       | Integral       | GPs-R-Us         | 1                | Sup 1      |

    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                       |
        | Sln4     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [] }                                                     |
        | Sln5     | Fully fledged GP system        | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop", "native-mobile" ] } |


@2724
Scenario: 1. Sections presented where the Solution exists
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    And the solution client-application-types section status is COMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario Outline: 2.Sections presented where the SolutionDetail does not exist
    When a GET request is made for solution dashboard <Solution>
    Then a successful response is returned
    And the solution client-application-types section status is INCOMPLETE
    And the solution client-application-types section requirement is Mandatory
Examples:
    | Solution |
    | Sln2     |
    | Sln3     |

@2724
Scenario: 3. Sections presented where ClientApplicationTypes is empty
    When a GET request is made for solution dashboard Sln4
    Then a successful response is returned
    And the solution client-application-types section status is INCOMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario: 4. Sections Mandatory when ClientApplicationTypes is set
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    And the solution browser-based section requirement is Mandatory
    And the solution native-desktop section requirement is Mandatory
    And the solution native-mobile section requirement is Mandatory
