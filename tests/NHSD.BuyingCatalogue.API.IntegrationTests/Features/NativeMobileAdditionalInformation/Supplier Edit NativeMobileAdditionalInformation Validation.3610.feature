Feature:  Display Marketing Page Form NativeMobileAdditionalInformation Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's NativeMobileAdditionalInformation
    So that I can ensure the information is correct & valid

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
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-mobile"], "NativeMobileAdditionalInformation": "Some additional info" } |

@3610
Scenario: 1. AdditionalInformation exceeds the maxLength
    When a PUT request is made to update the native-mobile-additional-information section for solution Sln1
    | AdditionalInformation       |
    | A string with length of 501 |
    Then a response status of 400 is returned
    And the additional-information field value is the validation failure maxLength
