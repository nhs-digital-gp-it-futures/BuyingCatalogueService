Feature:  Supplier Edit Mobile Memory And Storage
    As a Supplier
    I want to Edit the Mobile Memory And Storage Section
    So that I can ensure the information is correct

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
@3607
Scenario: Mobile StorageRequirementsDescription is updated to be too long
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description                 |
        | NULL                     | A string with length of 301 |
    Then a response status of 400 is returned
    And the storage-requirements-description field value is the validation failure maxLength
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | {}                |
        
@3607
Scenario: Client Application is updated with no Mobile StorageRequirementsDescription
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description |
        | 1GB                      | NULL        |
    Then a response status of 400 is returned
    And the storage-requirements-description field value is the validation failure required
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | {}                |

@3607
Scenario: Client Application is updated with no Mobile MinimumMemoryRequirement
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description   |
        | NULL                     | A description |
    Then a response status of 400 is returned
    And the minimum-memory-requirement field value is the validation failure required
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | {}                |
