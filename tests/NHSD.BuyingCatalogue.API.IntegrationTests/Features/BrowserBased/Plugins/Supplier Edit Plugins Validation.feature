Feature:  Display Marketing Page Form Plugins Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Plugins
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } |

@2793
Scenario: Required is empty. AdditionalInformation is valid
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail              |
        | NULL            | A string with length of 30 |
    Then a response status of 400 is returned
    And the plugins-required field value is the validation failure required

@2793
Scenario: Required has a value. AdditionalInformation length is greater than 500 characters
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail               |
        | Yes             | A string with length of 501 |
    Then a response status of 400 is returned
    And the plugins-detail field value is the validation failure maxLength

@2793
Scenario: Required is empty and AdditionalInformations length is greater than 500 characters
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail               |
        | NULL            | A string with length of 501 |
    Then a response status of 400 is returned
    And the plugins-required field value is the validation failure required
    And the plugins-detail field value is the validation failure maxLength
