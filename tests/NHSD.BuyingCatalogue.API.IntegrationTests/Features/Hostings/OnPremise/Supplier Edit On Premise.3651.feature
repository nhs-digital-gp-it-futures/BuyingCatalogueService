Feature:  Supplier Edit On Premise
    As a Supplier
    I want to Edit the On Premise Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting |
        | Sln1     | An full online medicine system | Online medicine 1 | { }     |
@3651
Scenario: 1. On Premise is updated
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | A string     |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                           |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": { "Summary": "New Summary", "Link": "a@b.c", "HostingModel": "AWS", "RequiresHSCN": "A string" } } |
        
@3651
Scenario: 2. On Premise is updated with trimmed whitespace
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary                  | Link             | HostingModel | RequiresHSCN           |
        | "      New Summary     " | "     a@b.c    " | "  AWS     " | "     A string       " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                           |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": { "Summary": "New Summary", "Link": "a@b.c", "HostingModel": "AWS", "RequiresHSCN": "A string" } } |
        
@3651
Scenario: 3. On Premise fields are set to null
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary | Link | HostingModel | RequiresHSCN |
        | NULL    | NULL | NULL         |              |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting             |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": {} } |

@3651
Scenario: 4. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln2
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | yES          |
    Then a response status of 404 is returned 

@3651
Scenario: 5. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | yES          |
    Then a response status of 500 is returned

@3651
Scenario: 6. Solution id is not present in the request
    When a PUT request is made to update the hosting-type-on-premise section with no solution id
        | Summary     | Link  | HostingModel | RequiresHSCN |
        | New Summary | a@b.c | AWS          | yES          |
    Then a response status of 400 is returned
