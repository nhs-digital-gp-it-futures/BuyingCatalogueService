Feature:  Supplier Edit Browser Supported 
    As a Supplier
    I want to Edit the Browser Support Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
        | Sln5       | Pills          | Drs. Inc         | 1                | Sup 2      |

@2786
Scenario: 1. Browser Supported is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      | yeS              |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                       |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                                                          |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2786
Scenario: 2. Browsers Supported is empty, Mobile Responsive has a result
     Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        |                   | true             |
    Then a response status of 400 is returned
    And the supported-browsers field value is the validation failure required
     And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
        | Sln5       | Pills          |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |

@2786
Scenario: 3. Mobile Responsive is empty
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      |                  |
    Then a response status of 400 is returned
    And the mobile-responsive field value is the validation failure required
    And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
        | Sln5       | Pills          |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |

@2786
Scenario: 4. Browsers Supported & Mobile Responsive are empty
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        |                   |                  |
    Then a response status of 400 is returned
    And the supported-browsers field value is the validation failure required
    And the mobile-responsive field value is the validation failure required
    And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
        | Sln5       | Pills          |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                         |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"], "BrowsersSupported" : [ "IE8", "Opera" ] } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                           |
        | Sln5     | Thrills                        | Bellyaches          | {"MobileResponsive": false }                                                              |

@2786
Scenario: 5. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the browser-browsers-supported section for solution Sln4
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 404 is returned 

@2786
Scenario: 6. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-browsers-supported section for solution Sln1
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 500 is returned

@2786
Scenario: 7. Solution id is not present in the request
    When a PUT request is made to update the browser-browsers-supported section with no solution id
        | BrowsersSupported | MobileResponsive |
        | Edge, Safari      | true             |
    Then a response status of 400 is returned
