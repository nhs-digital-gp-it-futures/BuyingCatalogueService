Feature: Mobile Memory And Storage
    As a Supplier
    I want to Edit the Mobile Memory And Storage section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | {}                |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3607
Scenario: Client Application is updated for the solution
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description   |
        | 1GB                      | A description |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                           |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" } } |
        
@3607
Scenario: Client Application is updated for the solution with trimmed whitespace
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement   | Description                 |
        | "        1GB             " | "       A description     " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                           |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" } } |

@3607
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln2
    | MinimumMemoryRequirement | Description   |
    | 1GB                      | A description |
    Then a response status of 404 is returned 

@3607
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
    | MinimumMemoryRequirement | Description   |
    | 1GB                      | A description |
    Then a response status of 500 is returned

@3607
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-mobile-memory-and-storage section with no solution id
    | MinimumMemoryRequirement | Description   |
    | 1GB                      | A description |
    Then a response status of 400 is returned
