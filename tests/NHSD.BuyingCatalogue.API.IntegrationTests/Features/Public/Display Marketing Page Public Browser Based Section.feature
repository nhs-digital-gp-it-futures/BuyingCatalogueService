Feature: Display Marketing Page Public Browser Based Section
    As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                 |
        | Sln1       | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ], "BrowsersSupported": ["Google Chrome", "Edge"], "MobileResponsive": false, "Plugins":  { "Required": true, "AdditionalInformation": "Colourful water extension" }, "MinimumConnectionSpeed": "1GBps", "MinimumDesktopResolution": "1x1", "HardwareRequirements": "New Hardware", "AdditionalInformation": "Some Additional Info", "MobileFirstDesign": true } |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3576
Scenario: Get Solution Public contains client application types browser based answers for all data
    When a GET request is made for solution public Sln1
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
