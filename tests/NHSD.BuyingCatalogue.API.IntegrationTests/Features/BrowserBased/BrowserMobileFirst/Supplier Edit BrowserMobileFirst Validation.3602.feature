Feature:  Supplier Edit Browser Mobile First Validation
    As a Supplier
    I want to Edit the Browser Mobile First Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware", "MobileFirstDesign": false } |

@3602
Scenario: 1. Browser Mobile First is updated to be empty
    When a PUT request is made to update the browser-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | NULL              |
    Then a response status of 400 is returned
    And the mobile-first-design field value is the validation failure required
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                 |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "AdditionalInformation": "Some Info", "HardwareRequirements": "New Hardware",  "MobileFirstDesign": false } |