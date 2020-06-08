Feature: No selection criteria browse
    As a Public User
    I want to browse the Solutions
    So that I know what those Solutions are

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Workflow                | true         |
        | Clinical Safety         | false        |
        | Resource Management     | false        |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId | PublicationStatus |
        | Sln1       | MedicOnline    | 1                | Sup 1      | 3                 |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      | 3                 |
        | Sln3       | PracticeMgr    | 1                | Sup 2      | 3                 |
        | Sln4       | Unpublished    | 1                | Sup 2      | 1                 |
    And Solutions are linked to Capabilities
        | Solution       | Capability              |
        | MedicOnline    | Appointments Management |
        | MedicOnline    | Clinical Safety         |
        | MedicOnline    | Workflow                |
        | TakeTheRedPill | Prescribing             |
        | TakeTheRedPill | Resource Management     |
        | PracticeMgr    | Clinical Safety         |
        | PracticeMgr    | Prescribing             |
        | PracticeMgr    | Workflow                |
    And Framework Solutions exist
        | SolutionId | IsFoundation |
        | Sln1       | true         |
        | Sln2       | false        |

@2053
Scenario: 1. No selection criteria applied
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill,PracticeMgr are found in the response
    And the solutions Unpublished are not found in the response

@2053
Scenario: 2. Card Content
    Given SolutionDetail exist
        | Solution | SummaryDescription     | FullDescription   | AboutUrl | Features                                            |
        | Sln1     | NULL                   | Online medicine 1 | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | Eye opening experience | Eye opening6      | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the details of the solutions returned are as follows
        | SolutionId | SolutionName   | SummaryDescription     | SupplierName | Capabilities                                       | IsFoundation |
        | Sln1       | MedicOnline    |                        | Supplier 1   | Appointments Management, Clinical Safety, Workflow | true         |
        | Sln2       | TakeTheRedPill | Eye opening experience | Supplier 2   | Prescribing, Resource Management                   | false        |
        | Sln3       | PracticeMgr    |                        | Supplier 2   | Clinical Safety, Prescribing, Workflow             | false        |

Scenario: 3. List all Solutions with no marketing data
    Given a SolutionDetail Sln1 does not exist
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill,PracticeMgr are found in the response

Scenario: 4. Retrieve all Solutions with Marketing data.
    Given SolutionDetail exist
        | Solution | SummaryDescription      | FullDescription     | AboutUrl | Features                                            |
        | Sln1     |                         | Online medicine 1   | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | Eye opening experience  | Eye opening6        | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
        | Sln3     | Fully fledged GP system | Fully fledged GP 12 | UrlSln3  | { "customJson" : { "id" : 3, "name" : "feature3" }} |
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill,PracticeMgr are found in the response

@5350
Scenario: 5. List all solutions filtered by SupplierID
    Given SolutionDetail exist
        | Solution | SummaryDescription     | FullDescription   | AboutUrl | Features                                            |
        | Sln1     | NULL                   | Online medicine 1 | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | Eye opening experience | Eye opening6      | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
    When a GET request is made containing a filter on supplierID Sup 1
    Then a successful response is returned
    And the details of the solutions returned are as follows
        | SolutionId | SolutionName   | SummaryDescription     | SupplierName | Capabilities                                       | IsFoundation |
        | Sln1       | MedicOnline    |                        | Supplier 1   | Appointments Management, Clinical Safety, Workflow | true         |

@5350
Scenario: 6. List all solutions filtered by an non existant SupplierID
    Given SolutionDetail exist
        | Solution | SummaryDescription     | FullDescription   | AboutUrl | Features                                            |
        | Sln1     | NULL                   | Online medicine 1 | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | Eye opening experience | Eye opening6      | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
    When a GET request is made containing a filter on supplierID INVALID
    Then a successful response is returned
    And an empty solution is returned
