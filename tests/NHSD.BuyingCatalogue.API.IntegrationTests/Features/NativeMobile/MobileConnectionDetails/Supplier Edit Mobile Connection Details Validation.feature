Feature:  Supplier Edit Mobile Connection Details
    As a Supplier
    I want to Edit the Mobile Connection Details Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
@3606
Scenario: Mobile Connection Requirement Details is updated to be too long
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1       | An full online medicine system | Online medicine 1 | {} |
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | NULL                   | A string with length of 301       | Horse, Moose, Giraffe |
    Then a response status of 400 is returned
    And the connection-requirements-description field value is the validation failure maxLength
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | {}                |
