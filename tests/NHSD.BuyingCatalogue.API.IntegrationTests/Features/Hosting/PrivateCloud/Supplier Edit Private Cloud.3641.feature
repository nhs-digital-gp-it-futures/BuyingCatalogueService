Feature:  Supplier Edit Private Cloud
    As a Supplier
    I want to Edit the Private Cloud Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
@3641
Scenario: 1. Private Cloud is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting |
        | Sln1     | An full online medicine system | Online medicine 1 | { }     |
    When a PUT request is made to update the hosting-type-private-cloud section for solution Sln1
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS         | yES          |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PrivateCloud": { "Summary": "New Summary", "Link": "a@b.c", "RequiresHSCN": "true" } } |
        
@3641
Scenario: 2. Private Cloud is updated with trimmed whitespace
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting |
        | Sln1     | An full online medicine system | Online medicine 1 | { }     |
    When a PUT request is made to update the hosting-type-private-cloud section for solution Sln1
        | Summary                  | Link             | HostingModel  | RequiresHSCN      |
        | "      New Summary     " | "     a@b.c    " | "  AWS     " | "     yES       " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PrivateCloud": { "Summary": "New Summary", "Link": "a@b.c", "HostingModel": "AWS", "RequiresHSCN": "true" } } |

@3641
Scenario: 3. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the hosting-type-private-cloud section for solution Sln2
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS         | yES          |
    Then a response status of 404 is returned 

@3641
Scenario: 4. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the hosting-type-private-cloud section for solution Sln1
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS         | yES          |
    Then a response status of 500 is returned

@3641
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the hosting-type-private-cloud section with no solution id
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS         | yES          |
    Then a response status of 400 is returned
