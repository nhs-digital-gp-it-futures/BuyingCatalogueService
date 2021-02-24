Feature:  Supplier Edit Browser Supported
    As a Supplier
    I want to Edit the Browser Support Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
        | Sln5       | Pills          | 1                | Sup 2      |

@2786
Scenario: Browser Supported is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      | yeS              |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                                                     |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true } |
        | Sln2       | NULL                           | NULL                | NULL                                                                                                                  |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                       |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                                                          |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2786
Scenario: Browser Supported is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported                 | MobileResponsive |
        | "     Chrome    ", "     Edge   " | "    yeS     "   |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                                                     |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true } |
        | Sln2       | NULL                           | NULL                | NULL                                                                                                                  |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                       |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                                                          |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2786
Scenario: Browsers Supported is empty, Mobile Responsive has a result
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        |                   | true             |
    Then a response status of 400 is returned
    And the supported-browsers field value is the validation failure required
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
        | Sln5       | Pills          |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln2       | NULL                           | NULL                | NULL                                                                                      |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |

@2786
Scenario: Mobile Responsive is empty
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      |                  |
    Then a response status of 400 is returned
    And the mobile-responsive field value is the validation failure required
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
        | Sln5       | Pills          |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln2       | NULL                           | NULL                | NULL                                                                                      |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |

@2786
Scenario: Browsers Supported & Mobile Responsive are empty
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        |                   |                  |
    Then a response status of 400 is returned
    And the supported-browsers field value is the validation failure required
    And the mobile-responsive field value is the validation failure required
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
        | Sln5       | Pills          |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln2       | NULL                           | NULL                | NULL                                                                                      |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5       | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |

@2786
Scenario: Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the browser-browsers-supported section for solution Sln4
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 404 is returned

@2786
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 500 is returned

@2786
Scenario: Solution id is not present in the request
    When a PUT request is made to update the browser-browsers-supported section with no solution id
        | BrowsersSupported | MobileResponsive |
        | Edge, Safari      | true             |
    Then a response status of 400 is returned
