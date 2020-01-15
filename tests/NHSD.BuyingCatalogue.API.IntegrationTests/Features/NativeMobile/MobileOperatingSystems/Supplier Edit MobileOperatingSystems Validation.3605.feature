Feature:  Supplier Edit Mobile Operating Systems Validation
    As a Supplier
    I want to Edit the Mobile Operating Systems Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
@3605
Scenario: 1. Mobile Operating Systems is updated to be null
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems | OperatingSystemsDescription |
        |                  | Descriptions                |
    Then a response status of 400 is returned
    And the operating-systems field value is the validation failure required
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |

@3605
Scenario: 2. Mobile Operating Systems Description exceeds the maxlength 1000
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems | OperatingSystemsDescription  |
        | Linux            | A string with length of 1001 |
    Then a response status of 400 is returned
    And the operating-systems-description field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |


@3605
Scenario: 3. Mobile Operating Systems is updated to null & the description exceeds the maxlength 1000
Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems | OperatingSystemsDescription  |
        |                  | A string with length of 1001 |
    Then a response status of 400 is returned
    And the operating-systems field value is the validation failure required
    And the operating-systems-description field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |
