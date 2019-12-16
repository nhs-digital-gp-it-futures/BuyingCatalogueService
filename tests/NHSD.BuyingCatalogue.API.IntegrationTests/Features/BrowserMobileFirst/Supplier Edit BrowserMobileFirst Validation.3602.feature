Feature:  Supplier Edit Browser Mobile First Validation
    As a Supplier
    I want to Edit the Browser Mobile First Section
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
@3602
Scenario: 1. Browser Mobile First is updated to be empty
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware", "MobileFirstDesign": false } |
    When a PUT request is made to update solution Sln1 browser-mobile-first section
        | MobileFirstDesign |
        | NULL              |
    Then a response status of 400 is returned
    And the required field contains mobile-first-design
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                 |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware",  "MobileFirstDesign": false } |