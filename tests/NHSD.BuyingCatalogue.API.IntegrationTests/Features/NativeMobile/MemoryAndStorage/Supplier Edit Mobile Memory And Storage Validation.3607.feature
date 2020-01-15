Feature:  Supplier Edit Mobile Memory And Storage
    As a Supplier
    I want to Edit the Mobile Memory And Storage Section
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
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
@3607
Scenario: 1. Mobile StorageRequirementsDescription is updated to be too long
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description                 |
        | NULL                     | A string with length of 301 |
    Then a response status of 400 is returned
    And the storage-requirements-description field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
        
@3607
Scenario: 2. Client Application is updated with no Mobile StorageRequirementsDescription
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description |
        | 1GB                      | NULL        |
    Then a response status of 400 is returned
    And the storage-requirements-description field value is the validation failure required
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |

@3607
Scenario: 3. Client Application is updated with no Mobile MinimumMemoryRequirement
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description   |
        | NULL                     | A description |
    Then a response status of 400 is returned
    And the minimum-memory-requirement field value is the validation failure required
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
