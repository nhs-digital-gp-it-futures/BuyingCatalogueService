Feature: Display Marketing Page Preview Browser Based Section
    As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And Solution have following details
        | SolutionId | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                 |
        | Sln1       | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ], "BrowsersSupported": ["Google Chrome", "Edge"], "MobileResponsive": false, "Plugins":  { "Required": true, "AdditionalInformation": "Colourful water extension" }, "MinimumConnectionSpeed": "1GBps", "MinimumDesktopResolution": "1x1", "HardwareRequirements": "New Hardware", "AdditionalInformation": "Some Additional Info", "MobileFirstDesign": true } |

@3322
Scenario: Get Solution Preview contains client application types browser based answers for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the response contains the following values
        | Section                             | Field                             | Value                     |
        | browser-browsers-supported          | supported-browsers                | Google Chrome,Edge        |
        | browser-browsers-supported          | mobile-responsive                 | No                        |
        | browser-plug-ins-or-extensions      | plugins-required                  | Yes                       |
        | browser-plug-ins-or-extensions      | plugins-detail                    | Colourful water extension |
        | browser-hardware-requirements       | hardware-requirements-description | New Hardware              |
        | browser-connectivity-and-resolution | minimum-connection-speed          | 1GBps                     |
        | browser-connectivity-and-resolution | minimum-desktop-resolution        | 1x1                       |
        | browser-additional-information      | additional-information            | Some Additional Info      |
        | browser-mobile-first                | mobile-first-design               | Yes                       |
