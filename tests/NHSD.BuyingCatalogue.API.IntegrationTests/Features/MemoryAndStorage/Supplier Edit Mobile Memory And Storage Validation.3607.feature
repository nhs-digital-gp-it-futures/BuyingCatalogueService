@ignore
Feature:  Supplier Edit Mobile Memory And Storage
    As a Supplier
    I want to Edit the Mobile Memory And Storage Section
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
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
@3607
Scenario: 1. Mobile StorageRequirementsDescription is updated to be too long
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
    When a PUT request is made to update the mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription |
        | NULL                     | A string with length of 301    |
    Then a response status of 400 is returned
    And the maxLength field contains storage-requirements-description
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
        
@3607
Scenario: 2. Client Application is updated with no Mobile StorageRequirementsDescription
    When a PUT request is made to update the mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription |
        | 1GB                      | NULL                           |
    Then a response status of 400 is returned
    And the required field contains storage-requirements-description
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |

@3607
Scenario: 3. Client Application is updated with no Mobile MinimumMemoryRequirement
    When a PUT request is made to update the mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription |
        | NULL                     | A description                  |
    Then a response status of 400 is returned
    And the required field contains minimum-memory-requirement
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
