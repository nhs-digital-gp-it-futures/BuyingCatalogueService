Feature:  Display Marketing Page Form Native Desktop Memory And Storage Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Memory And Storage
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
        | Sln3       | PracticeMgr    | 1                | Sup 1      |
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": "3.5Ghz", "RecommendedResolution": "800x600" } } |
        | Sln3       | Testing System                 | Full System       | {  }                                                                                                                                                                                       |

@3620
Scenario: Native Desktop Memory And Storage are retrieved for the solution
    When a GET request is made for native-desktop-memory-and-storage section for solution Sln1
    Then a successful response is returned
    And the string value of element minimum-memory-requirement is 1GB
    And the string value of element storage-requirements-description is A description
    And the string value of element minimum-cpu is 3.5Ghz
    And the string value of element recommended-resolution is 800x600

@3620
Scenario: Native Desktop Memory And Storage are retrieved for the solution where no solution detail exists
    When a GET request is made for native-mobile-memory-and-storage section for solution Sln2
    Then a successful response is returned
    And the minimum-memory-requirement string does not exist
    And the storage-requirements-description string does not exist
    And the minimum-cpu string does not exist
    And the recommended-resolution string does not exist

@3620
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-memory-and-storage section for solution Sln4
    Then a response status of 404 is returned

@3620
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-memory-and-storage section for solution Sln1
    Then a response status of 500 is returned

@3620
Scenario: Solution id not present in request
    When a GET request is made for native-desktop-memory-and-storage section with no solution id
    Then a response status of 400 is returned
    
@3620
Scenario: Native Desktop Memory And Storage are retrieved as empty if they do not exist yet
    When a GET request is made for native-desktop-memory-and-storage section for solution Sln3
    Then a successful response is returned
    And the minimum-memory-requirement string does not exist
    And the storage-requirements-description string does not exist
    And the minimum-cpu string does not exist
    And the recommended-resolution string does not exist
