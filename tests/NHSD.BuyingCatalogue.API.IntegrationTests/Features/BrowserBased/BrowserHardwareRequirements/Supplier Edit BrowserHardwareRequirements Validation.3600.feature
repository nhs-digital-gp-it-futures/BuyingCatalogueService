Feature:  Display Marketing Page Form BrowserHardwareRequirements Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's BrowserHardwareRequirements
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
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                  |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HardwareRequirements": "Hardware Information" } | 

@3600
Scenario: 1. HardwareRequirements exceeds the maxLength
    When a PUT request is made to update the browser-hardware-requirements section for solution Sln1
    | HardwareRequirements        |
    | A string with length of 501 |
    Then a response status of 400 is returned
    And the hardware-requirements-description field value is the validation failure maxLength

@3600
Scenario: 2. Hardware requirements is set to null
    When a PUT request is made to update the browser-hardware-requirements section for solution Sln1
    | HardwareRequirements |
    | NULL                 |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [] } |
