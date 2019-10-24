@Ignore
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

@2786
Scenario: 1. Browser Support is updated for the solution
    Given MarketingDetail exist
        | Solution | ClientApplication                                                       |
        | Sln1     | { "BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                         |
        | Sln5     | {"MobileResponsive": false }                                            |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Edge      | true             |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                       |
        | Sln1     | { "BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true} |
        | Sln3     |                                                                         |
        | Sln5     | {"MobileResponsive": false }                                            |  

@2786
Scenario: 2. Browser Support is added to the solution
   When a PUT request is made to update solution Sln2 browsers-supported section
         | BrowsersSupported   | MobileResponsive |
         | Chrome, Opera, Edge | true             |
   Then a successful response is returned
   And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
   And MarketingDetail exist
        | Solution | ClientApplication                                                                  |
        | Sln1     | { "BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true}          |
        | Sln2     | { "BrowsersSupported" : [ "Chrome", "Opera", "Edge" ], "MobileResponsive": true} |
        | Sln3     |                                                                                    |
        | Sln5     | {"MobileResponsive": false }                                                     |


@2786
Scenario: 3. Browser Support is completely cleared
    Given MarketingDetail exist
        | Solution | ClientApplication                                                        |
        | Sln1     | { "BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": true } |
        | Sln3     |                                                                          |
        | Sln5     | {"MobileResponsive": false }                                           |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                      |
        | Sln1     | { "BrowsersSupported" : [], "MobileResponsive": null } |
        | Sln3     |                                                        |
        | Sln5     | {"MobileResponsive": false }                           |

@2786
Scenario: 4. Empty Supported-Browsers is added to the solution, Mobile-Response is added
    Given MarketingDetail exist
        | Solution | ClientApplication                                                      |
        | Sln1     | { "BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": true } |
        | Sln3     |                                                                        |
        | Sln5     | {"MobileResponsive": false }                                           |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        |                   | false            |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                       |
        | Sln1     | { "BrowsersSupported" : [], "MobileResponsive": false } |
        | Sln3     |                                                         |
        | Sln5     | {"MobileResponsive": false }                            |

@2786
Scenario: 5. Supported-Browsers are added to the solution, Mobile-Response is empty
    Given MarketingDetail exist
        | Solution | ClientApplication                                                      |
        | Sln1     | { "BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": true } |
        | Sln3     |                                                                        |
        | Sln5     | {"MobileResponsive": false }                                           |
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported       | MobileResponsive |
        | Firefox, Chrome, Safari |                  |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                   |
        | Sln1     | { "BrowsersSupported" : ["Firefox", "Chrome", "Safari"], "MobileResponsive": null } |
        | Sln3     |                                                                                     |
        | Sln5     | {"MobileResponsive": false }                                                        |

@2786
Scenario: 6. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update solution Sln1 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 404 is returned 

@2786
Scenario: 7. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln4 browsers-supported section
        | BrowsersSupported | MobileResponsive |
        | Chrome, Safari    | false            |
    Then a response status of 500 is returned

@2786
Scenario: 8. Solution id is not present in the request
    When a PUT request is made to update solution browsers-supported section with no solution id
        | BrowsersSupported | MobileResponsive |
        | Edge, Safari      | true             |
    Then a response status of 400 is returned
