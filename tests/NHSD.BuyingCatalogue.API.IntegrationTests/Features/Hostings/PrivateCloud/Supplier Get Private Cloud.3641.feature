Feature: Display Marketing Page Form Private Cloud Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Private Cloud Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | TakeTheGreenPill | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription            | FullDescription   | Hosting                                                                                                                                                              |
        | Sln1     | A full online medicine system | Online medicine 1 | { "PrivateCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "AZDO", "RequiresHscn": "This Solution requires a HSCN/N3 connection" } } |
        | Sln2     | An online medicine system     | Online medicine 2 | {  }                                                                                                                                                                 |

@3641
Scenario: 1.Private Cloud is retreived for the solution
    When a GET request is made for hosting-type-private-cloud section for solution Sln1
    Then a successful response is returned
    And the string value of element summary is Some summary
    And the string value of element link is www.somelink.com
    And the string value of element hosting-model is AZDO
    And the requires-hscn element contains
        | Elements                                    |
        | This Solution requires a HSCN/N3 connection |

    
@3641
Scenario: 2.Private Cloud is retrieved for the solution where no public cloud data exists
    When a GET request is made for hosting-type-private-cloud section for solution Sln2
    Then a successful response is returned
    And the summary string does not exist
    And the link string does not exist
    And the hosting-model string does not exist
    And the requires-hscn element contains
        | Elements |
        |          |

@3641
Scenario: 3. Private Cloud is retrieved for the solution where no solution detail exists
    When a GET request is made for hosting-type-private-cloud section for solution Sln3
    Then a successful response is returned
    And the summary string does not exist
    And the link string does not exist
    And the hosting-model string does not exist
    And the requires-hscn element contains
        | Elements |
        |          |

@3641
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for hosting-type-private-cloud section for solution Sln4
    Then a response status of 404 is returned

@3641
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for hosting-type-private-cloud section for solution Sln2
    Then a response status of 500 is returned

@3641
Scenario: 6. Solution id not present in request
    When a GET request is made for hosting-type-private-cloud section with no solution id
    Then a response status of 400 is returned
