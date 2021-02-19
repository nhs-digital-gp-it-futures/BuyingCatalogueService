Feature: Display Marketing Page Form On Premise Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's On Premise Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName     | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | 1                | Sup 1      |
        | Sln3       | TakeTheGreenPill | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | SummaryDescription            | FullDescription   | Hosting                                                                                                                                                           |
        | Sln1       | A full online medicine system | Online medicine 1 | { "OnPremise": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "AZDO", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
        | Sln2       | An online medicine system     | Online medicine 2 | { }                                                                                                                                                               |

@3651
Scenario: On Premise is retreived for the solution
    When a GET request is made for hosting-type-on-premise section for solution Sln1
    Then a successful response is returned
    And the string value of element summary is Some summary
    And the string value of element link is www.somelink.com
    And the string value of element hosting-model is AZDO
    And the requires-hscn element contains
        | Elements                                    |
        | This Solution requires a HSCN/N3 connection |

@3651
Scenario: On Premise is retrieved for the solution where no public cloud data exists
    When a GET request is made for hosting-type-on-premise section for solution Sln2
    Then a successful response is returned
    And the summary string does not exist
    And the link string does not exist
    And the hosting-model string does not exist
    And the requires-hscn element contains
        | Elements |
        |          |

@3651
Scenario: On Premise is retrieved for the solution where no solution detail exists
    When a GET request is made for hosting-type-on-premise section for solution Sln3
    Then a successful response is returned
    And the summary string does not exist
    And the link string does not exist
    And the hosting-model string does not exist
    And the requires-hscn element contains
        | Elements |
        |          |

@3651
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for hosting-type-on-premise section for solution Sln4
    Then a response status of 404 is returned

@3651
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for hosting-type-on-premise section for solution Sln2
    Then a response status of 500 is returned

@3651
Scenario: Solution id not present in request
    When a GET request is made for hosting-type-on-premise section with no solution id
    Then a response status of 400 is returned
