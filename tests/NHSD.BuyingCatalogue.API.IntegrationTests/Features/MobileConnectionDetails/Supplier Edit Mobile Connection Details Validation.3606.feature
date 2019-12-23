Feature:  Supplier Edit Mobile Connection Details
    As a Supplier
    I want to Edit the Mobile Connection Details Section
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
@3606
Scenario: 1. Mobile Connection Requirement Details is updated to be too long
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | {} |
    When a PUT request is made to update the mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | NULL                   | A string with length of 301       | Horse, Moose, Giraffe |
    Then a response status of 400 is returned
    And the maxLength field contains connection-requirements-description
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
