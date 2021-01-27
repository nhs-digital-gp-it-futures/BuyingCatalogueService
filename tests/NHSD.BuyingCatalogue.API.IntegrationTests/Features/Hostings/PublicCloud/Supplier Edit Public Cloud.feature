Feature: Public Cloud
    As a Supplier
    I want to Edit the Public Cloud Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |

@3639
Scenario: Public Cloud is updated
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary     | Link     | RequiresHSCN     |
        | New Summary | New Link | New Connectivity |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                 |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "New Summary", "Link": "New Link", "RequiresHSCN": "New Connectivity" } } |

@3639
Scenario: Public Cloud is updated with trimmed whitespace
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary              | Link             | RequiresHSCN            |
        | "     New Summary  " | "   New Link   " | " New Connectivity    " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                 |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "New Summary", "Link": "New Link", "RequiresHSCN": "New Connectivity" } } |

@3639
Scenario: Public Cloud variables are updated to null or empty
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary | Link | RequiresHSCN |
        | NULL    | NULL |              |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { } } |

@3639
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln2
        | Summary     | Link     | RequiresHSCN     |
        | New Summary | New Link | New Connectivity |
    Then a response status of 404 is returned

@3639
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary     | Link     | RequiresHSCN     |
        | New Summary | New Link | New Connectivity |
    Then a response status of 500 is returned

@3639
Scenario: Solution id is not present in the request
    When a PUT request is made to update the hosting-type-public-cloud section with no solution id
        | Summary     | Link     | RequiresHSCN     |
        | New Summary | New Link | New Connectivity |
    Then a response status of 400 is returned
