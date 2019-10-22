@ignore
Feature: Supplier Get Client Application Type
    As a Supplier
    I want to read the ClientApplicationType Section

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
    And MarketingDetail exist
        | Solution | ClientApplication                                                    |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] } |
        | Sln3     |                                                                      |

@2724
Scenario: 1. Client Application Types are retrieved for the solution
    When a GET request is made for client-application-types for solution Sln1
    Then a successful response is returned
    And the client-application-types element contains
    | ClientApplicationTypes           |
    | "browser-based","native-desktop" |

@2724
Scenario: 2. Client Application Types are retrieved for the solution where no marketing detail exists
    When a GET request is made for client-application-types for solution Sln2
    Then a successful response is returned
    And the client-application-types element contains
    | ClientApplicationTypes |
    |                        |

@2724
Scenario: 3. Client Application Types are retrieved for the solution where no client application types exist
    When a GET request is made for client-application-types for solution Sln3
    Then a successful response is returned
    And the client-application-types element contains
    | ClientApplicationTypes |
    |                        |

@2726
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for client-application-types for solution Sln4
    Then a response status of 404 is returned

@2726
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for client-application-types for solution Sln1
    Then a response status of 500 is returned

@2726
Scenario: 6. Solution id not present in request
    When a GET request is made for client-application-types with no solution id
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 400 is returned
