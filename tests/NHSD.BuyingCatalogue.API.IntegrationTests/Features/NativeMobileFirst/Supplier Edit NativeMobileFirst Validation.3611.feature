Feature:  Supplier Edit Native Mobile First Validation
    As an Authority User
    I want to edit the Mobile First Sub-Section
    So that I can make sure the information is correct

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
@3611
Scenario: 1. Native Mobile First is updated to be empty
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                  |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware", "MobileFirstDesign": false, "NativeMobileFirstDesign": false } |
    When a PUT request is made to update the native-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | NULL              |
    Then a response status of 400 is returned
    And the required field contains mobile-first-design
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware",  "MobileFirstDesign": false, "NativeMobileFirstDesign": false } |
