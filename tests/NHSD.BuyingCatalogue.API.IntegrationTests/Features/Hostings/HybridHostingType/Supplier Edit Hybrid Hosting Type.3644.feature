Feature:  Supplier Edit Hybrid Hosting Type
    As a Supplier
    I want to Edit the Hybrid Hosting Type Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SummaryDescription             | FullDescription   | Hosting |
        | Sln1       | An full online medicine system | Online medicine 1 | { }     |
@3644
Scenario: 1. Hybrid Hosting Type is updated
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | A string     |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": { "Summary": "New Summary", "Link": "a@b.c", "HostingModel": "AWS", "RequiresHSCN": "A string" } } |

@3644
Scenario: 2. Hybrid Hosting Type is updated with trimmed whitespace
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary                  | Link             | HostingModel | RequiresHSCN           |
        | "      New Summary     " | "     a@b.c    " | "  AWS     " | "     A string       " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": { "Summary": "New Summary", "Link": "a@b.c", "HostingModel": "AWS", "RequiresHSCN": "A string" } } |

@3644
Scenario: 3. Hybrid Hosting Type fields are set to null
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary | Link | HostingModel | RequiresHSCN |
        | NULL    | NULL | NULL         |              |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": {} } |

@3644
Scenario: 4. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln2
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | yES          |
    Then a response status of 404 is returned

@3644
Scenario: 5. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | yES          |
    Then a response status of 500 is returned

@3644
Scenario: 6. Solution id is not present in the request
    When a PUT request is made to update the hosting-type-hybrid section with no solution id
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | yES          |
    Then a response status of 400 is returned
