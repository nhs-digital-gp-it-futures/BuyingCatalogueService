Feature:  Supplier Edit Native Mobile First Validation
    As an Authority User
    I want to edit the Mobile First Sub-Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
@3611
Scenario: Native Mobile First is updated to be empty
    Given Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware", "MobileFirstDesign": false, "NativeMobileFirstDesign": false } |
    When a PUT request is made to update the native-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | NULL              |
    Then a response status of 400 is returned
    And the mobile-first-design field value is the validation failure required
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                   |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware",  "MobileFirstDesign": false, "NativeMobileFirstDesign": false } |
