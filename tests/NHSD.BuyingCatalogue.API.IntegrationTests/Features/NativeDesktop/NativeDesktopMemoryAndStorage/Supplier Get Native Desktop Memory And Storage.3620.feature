Feature:  Display Marketing Page Form Native Desktop Memory And Storage Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Memory And Storage
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | PracticeMgr    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": "3.5Ghz", "RecommendedResolution": "800x600" } } |
        | Sln3     | Testing System                 | Full System       | {  }                                                                                                                                                                                       |

@3607
Scenario: 1. Native Desktop Memory And Storage are retrieved for the solution
    When a GET request is made for native-desktop-memory-and-storage for solution Sln1
    Then a successful response is returned
    And the string value of element minimum-memory-requirement is 1GB
    And the string value of element storage-requirements-description is A description
    And the string value of element minimum-cpu is 3.5Ghz
    And the string value of element recommended-resolution is 800x600

@3607
Scenario: 2. Native Desktop Memory And Storage are retrieved for the solution where no solution detail exists
    When a GET request is made for native-mobile-memory-and-storage for solution Sln2
    Then a successful response is returned
    And the minimum-memory-requirement string does not exist
    And the storage-requirements-description string does not exist
    And the minimum-cpu string does not exist
    And the recommended-resolution string does not exist

@3607
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-memory-and-storage for solution Sln4
    Then a response status of 404 is returned

@3607
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-memory-and-storage for solution Sln1
    Then a response status of 500 is returned

@3607
Scenario: 5. Solution id not present in request
    When a GET request is made for native-desktop-memory-and-storage with no solution id
    Then a response status of 400 is returned
    
@3607
Scenario: 6. Native Desktop Memory And Storage are retrieved as empty if they do not exist yet
    When a GET request is made for native-desktop-memory-and-storage for solution Sln3
    Then a successful response is returned
    And the minimum-memory-requirement string does not exist
    And the storage-requirements-description string does not exist
    And the minimum-cpu string does not exist
    And the recommended-resolution string does not exist
