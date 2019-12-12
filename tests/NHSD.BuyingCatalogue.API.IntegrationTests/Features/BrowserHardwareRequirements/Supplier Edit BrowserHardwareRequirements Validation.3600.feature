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
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1   | { "HardwareRequirements": "Hardware Information","ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } | 

@3600
Scenario: 1. HardwareRequirements exceeds the maxLength
    Given hardware-requirements-description is a string of 501 characters
    When a PUT request is made to update solution Sln1 hardware-requirements-description section
    Then a response status of 400 is returned
    And the browser-hardware-requirements maxLength field contains hardware-requirements-description

@3600
Scenario: 2. Hardware requirements is set to null
    Given hardware-requirements-description is null
    When a PUT request is made to update solution Sln1 hardware-requirements-description section
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                           |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" }, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "AdditionalInformation": null } |
