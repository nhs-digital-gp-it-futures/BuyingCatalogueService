Feature: Supplier Get Client Application Type
    As a Supplier
    I want to read the ClientApplicationType Section

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                    |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] } |
        | Sln2       | NULL                           | NULL                | NULL                                                                 |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                      |

@2724
Scenario: 1. Client Application Types are retrieved for the solution
    When a GET request is made for client-application-types section for solution Sln1
    Then a successful response is returned
    And the client-application-types element contains
        | Elements                      |
        | browser-based, native-desktop |

@2724
Scenario: 2. Client Application Types are retrieved for the solution where no solution detail exists
    When a GET request is made for client-application-types section for solution Sln2
    Then a successful response is returned
    And the client-application-types element contains
        | Elements |
        |          |

@2724
Scenario: 3. Client Application Types are retrieved for the solution where no client application types exist
    When a GET request is made for client-application-types section for solution Sln3
    Then a successful response is returned
    And the client-application-types element contains
        | Elements |
        |          |

@2726
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for client-application-types section for solution Sln4
    Then a response status of 404 is returned

@2726
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for client-application-types section for solution Sln1
    Then a response status of 500 is returned

@2726
Scenario: 6. Solution id not present in request
    When a GET request is made for client-application-types section with no solution id
    Then a response status of 400 is returned