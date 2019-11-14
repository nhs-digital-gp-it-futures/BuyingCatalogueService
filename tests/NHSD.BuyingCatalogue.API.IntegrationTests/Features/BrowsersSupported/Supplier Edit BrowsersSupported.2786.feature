Feature:  Supplier Edit Browser Supported 
    As a Supplier
    I want to Edit the Browser Support Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
        | Sln5       | Pills          | Thrills                        | Drs. Inc         | Bellyaches          | 1                |

@2786
Scenario: 1. Browser Supported is updated
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                                                                    |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null } |
        | Sln3     |                                                                                                                                      |
        | Sln5     | {"MobileResponsive": false }                                                                                                         |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      | yes              |
    Then a successful response is returned
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                                    |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true, "Plugins": null} |
        | Sln3     |                                                                                                                                      |
        | Sln5     | {"MobileResponsive": false }                                                                                                         |

@2786
Scenario: 2. Browsers Supported is empty, Mobile Responsive has a result
     Given MarketingDetail exist
        | Solution | ClientApplication                                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                                                        |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        |                   | true             |
    Then a response status of 400 is returned
    And the browser-based required field contains supported-browsers
     And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
        | Sln5       | Pills          | Thrills                        | Drs. Inc         | Bellyaches          | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                                                        |

@2786
Scenario: 3. Mobile Responsive is empty
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                                                        |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      |                  |
    Then a response status of 400 is returned
    And the browser-based required field contains mobile-responsive
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
        | Sln5       | Pills          | Thrills                        | Drs. Inc         | Bellyaches          | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                                                        |

@2786
Scenario: 4. Browsers Supported & Mobile Responsive are empty
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                                                        |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        |                   |                  |
    Then a response status of 400 is returned
    And the browser-based required field contains supported-browsers
    And the browser-based required field contains mobile-responsive
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
        | Sln5       | Pills          | Thrills                        | Drs. Inc         | Bellyaches          | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                                                        |

@2786
Scenario: 5. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update solution Sln4 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 404 is returned 

@2786
Scenario: 6. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln4 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 500 is returned

@2786
Scenario: 7. Solution id is not present in the request
    When a PUT request is made to update solution browsers-supported section with no solution id
        | BrowsersSupported | MobileResponsive |
        | Edge, Safari      | true             |
    Then a response status of 400 is returned
