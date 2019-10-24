Feature: Supplier Edit Client Application Type
    As a Supplier
    I want to Edit the ClientApplicationType Section
    So that I can make sure the information is correct

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

@2726
Scenario: 1. Client Application Types are updated for the solution
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                     |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | {  }                                                                                  |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes       |
        | browser-based,native-mobile |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                          |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [],  "MobileResponsive": null } |
        | Sln2     | {  }                                                                                                                       |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }                                      |

@2726
Scenario: 2. Client Application Types are added to the solution
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes       |
        | browser-based,native-mobile |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                   |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [],  "MobileResponsive": null } |

@2726
Scenario: 3. Client Application Types that we do not understand are ignored
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                     |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | {  }                                                                                  |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes                      |
        | browser-based,native-mobile,elephant,cheese |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                         |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [],  "MobileResponsive": null } |
        | Sln2     | {  }                                                                                                                      |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }                                     |

@2726
Scenario: 4. Client Application Types can be completely cleared
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                     |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | {  }                                                                                  |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes |
        |                        |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                        |
        | Sln1     | { "ClientApplicationTypes" : [ ], "BrowsersSupported": [],  "MobileResponsive": null } |
        | Sln2     | {  }                                                                                     |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }    |

@2726
Scenario: 5. Empty Client Application Types can be added to the solution
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes |
        | cheese                 |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                        |
        | Sln1     | { "ClientApplicationTypes" : [ ], "BrowsersSupported": [],  "MobileResponsive": null } |
@2726
Scenario: 6. Solution not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update solution Sln4 client-application-types section
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 404 is returned

@2726
Scenario: 7. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln4 client-application-types section
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 500 is returned

@2726
Scenario: 8. Solution id not present in request
    When a PUT request is made to update solution client-application-types section with no solution id
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 400 is returned
