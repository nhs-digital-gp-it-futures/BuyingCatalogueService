Feature: Supplier Display Sub-Form Browser Based Client Type
    As a Supplier
    I want to Display Sub-Form Browser Based Client Type
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SummaryDescription             | FullDescription     | SupplierId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | Sup 1      |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | Sup 2      |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | Sup 2      |

@2782
Scenario: Sub-Form Browser Based Client Type all sections are Displayed
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And Solutions section contains all items 
        | Id                                  | Status     | Requirement |
        | browser-browsers-supported          | INCOMPLETE | Mandatory   |
        | browser-mobile-first                | INCOMPLETE | Mandatory   |
        | browser-plug-ins-or-extensions      | INCOMPLETE | Mandatory   |
        | browser-connectivity-and-resolution | INCOMPLETE | Mandatory   |
        | browser-hardware-requirements       | INCOMPLETE | Optional    |
        | browser-additional-information      | INCOMPLETE | Optional    |

@2782
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-based section dashboard for solution Sln4
    Then a response status of 404 is returned

@2782
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a response status of 500 is returned

@2782
Scenario: Solution id not present in request
    When a GET request is made for browser-based section dashboard with no solution id
    Then a response status of 400 is returned

@2782
Scenario: Browser Supported status incomplete when record not present
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-browsers-supported section status is INCOMPLETE

@2782
Scenario Outline: Browser Supported status based on data in ClientApplication
    Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1     | <ClientApplication> |
        
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-browsers-supported section status is <Status>
Examples:
    | ClientApplication                                                                                                        | Status     |
    |                                                                                                                          | INCOMPLETE |
    | { "ClientApplicationTypes" : [ "native-desktop" ] }                                                                      | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ] }                                                                                       | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ], "BrowsersSupported" : [ ] }                                                            | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ], "MobileResponsive": null }                                                             | INCOMPLETE |
    | { "BrowsersSupported" : [ ] }                                                                                            | INCOMPLETE |
    | { "MobileResponsive" : null }                                                                                            | INCOMPLETE |
    | { "BrowsersSupported" : [ ], "MobileResponsive" : null }                                                                 | INCOMPLETE |
    | { "BrowsersSupported" : [ "Google Chrome" ], "MobileResponsive" : null }                                                 | INCOMPLETE |
    | { "BrowsersSupported" : [ ], "MobileResponsive" : true }                                                                 | INCOMPLETE |
    | { "BrowsersSupported" : [ ], "MobileResponsive" : false }                                                                | INCOMPLETE |
    | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported" : [ "Google Chrome" ], "MobileResponsive" : true } | COMPLETE   |
    | { "BrowsersSupported" : [ "Google Chrome", "IE6" ], "MobileResponsive" : false }                                         | COMPLETE   |
    | { "BrowsersSupported" : [ "Mozilla Firefox", "IE11" ], "MobileResponsive" : true }                                       | COMPLETE   |

@2793
Scenario: Plugins status incomplete when record not present
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-plug-ins-or-extensions section status is INCOMPLETE

@2793
Scenario Outline: Plugins status based on data in ClientApplication
    Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1     | <ClientApplication> |
        
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-plug-ins-or-extensions section status is <Status>
Examples:
    | ClientApplication                                                                                                  | Status     |
    | { "ClientApplicationTypes" : [ ], "Plugins" : null }                                                               | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ], "Plugins": {"Required" : null, "AdditionalInformation": null } }                 | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ], "Plugins": {"Required" : false, "AdditionalInformation": null } }                | COMPLETE   |
    | { "ClientApplicationTypes" : ["browser-based" ], "Plugins": {"Required" : null, "AdditionalInformation": null } }  | INCOMPLETE |
    | { "ClientApplicationTypes" : ["browser-based" ], "Plugins": {"Required" : false, "AdditionalInformation": null } } | COMPLETE   |
    | { "ClientApplicationTypes" : ["browser-based" ], "Plugins": {"Required" : true, "AdditionalInformation": null } }  | COMPLETE   |
    | { "Plugins": {"Required" : null, "AdditionalInformation": null } }                                                 | INCOMPLETE |
    | { "Plugins": {"Required" : false, "AdditionalInformation": null } }                                                | COMPLETE   |
    | { "Plugins": {"Required" : true, "AdditionalInformation": null } }                                                 | COMPLETE   |


@3600
Scenario: Browser Hardware Requirements incomplete when record is not preset
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-hardware-requirements section status is INCOMPLETE

@3600
Scenario Outline: 10. Browser Hardware Requirements Based on data in Client Application
    Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-hardware-requirements section status is <Status>
Examples:
    | ClientApplication                                                                                | Status     |
    |                                                                                                  | INCOMPLETE |
    | { "ClientApplicationTypes" : [ "native-desktop" ] }                                              | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ] }                                                               | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ], "HardwareRequirements": null }                                 | INCOMPLETE |
    | { "ClientApplicationTypes" : [ ], "HardwareRequirements": "Another Requirement"}                 | COMPLETE   |
    | { "HardwareRequirements": null }                                                                 | INCOMPLETE |
    | { "HardwareRequirements": "This is a new Hardware Requirement" }                                 | COMPLETE   |
    | { "ClientApplicationTypes" : ["browser-based" ], "HardwareRequirements": null }                  | INCOMPLETE |
    | { "ClientApplicationTypes" : ["browser-based" ], "HardwareRequirements": "	" }                | INCOMPLETE |
    | { "ClientApplicationTypes" : ["browser-based" ], "HardwareRequirements": "" }                    | INCOMPLETE |
    | { "ClientApplicationTypes" : ["browser-based" ], "HardwareRequirements": "Another Requirement" } | COMPLETE   |

@3599
Scenario Outline: 11. Browser Connectivity and Resolution Based on data in Client Application
    Given solutions have the following details
        | SolutionId    | ClientApplication   |
        | Sln1          | <ClientApplication> |
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-connectivity-and-resolution section status is <Status>
Examples:
    | ClientApplication                                                        | Status     |
    |                                                                          | INCOMPLETE |
    | { "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null }     | INCOMPLETE |
    | { "MinimumConnectionSpeed": '1GBps', "MinimumDesktopResolution": null }  | COMPLETE   |
    | { "MinimumConnectionSpeed": null, "MinimumDesktopResolution": '1x1' }    | INCOMPLETE |
    | { "MinimumConnectionSpeed": '1GBps', "MinimumDesktopResolution": '1x1' } | COMPLETE   |

@3601
Scenario: 12. Browser Additional Information incomplete when record is not preset
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-hardware-requirements section status is INCOMPLETE

@3601
Scenario Outline: 13 Browser Additional Information Based on data in Client Application
  Given solutions have the following details
        | SolutionId    | ClientApplication   |
        | Sln1          | <ClientApplication> |
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-additional-information section status is <Status>
Examples:
    | ClientApplication                                               | Status     |
    | { "AdditionalInformation": "This is an additional information"} | COMPLETE   |
    | { "AdditionalInformation": null }                               | INCOMPLETE |
    | { "AdditionalInformation": "	" }                               | INCOMPLETE |
    | { "AdditionalInformation": "" }                                 | INCOMPLETE |

@3602
Scenario Outline: 14. Browser Mobile Frst Based on data in Client Application
  Given solutions have the following details
        | SolutionId    | ClientApplication   |
        | Sln1          | <ClientApplication> |
    When a GET request is made for browser-based section dashboard for solution Sln1
    Then a successful response is returned
    And the solution browser-mobile-first section status is <Status>
Examples:
    | ClientApplication              | Status     |
    |                                | INCOMPLETE |
    | { "MobileFirstDesign": null }  | INCOMPLETE |
    | { "MobileFirstDesign": false } | COMPLETE   |
    | { "MobileFirstDesign": true }  | COMPLETE   |
