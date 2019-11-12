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
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                |

@2726
Scenario: 1. Client Application Types are updated for the solution
    Given MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     |  ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   |  { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 |  {  }                                                                                  |
        | Sln3     | Thrills                        | Bellyaches          |  { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes       |
        | browser-based,native-mobile |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId |
        | Sln1       | MedicOnline    | 1                |
        | Sln2       | TakeTheRedPill | 1                |
        | Sln3       | PracticeMgr    | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     |  ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1   |  { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [],  "MobileResponsive": null, "Plugins": null } |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 |  {  }                                                                                                                                     |
        | Sln3     | Thrills                        | Bellyaches          |  { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }                                                    |

@2726
Scenario: 2. Client Application Types are added to the solution
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes       |
        | browser-based,native-mobile |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId |
        | Sln1       | MedicOnline    | 1                |
        | Sln2       | TakeTheRedPill | 1                |
        | Sln3       | PracticeMgr    | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [],  "MobileResponsive": null, "Plugins": null } |

@2726
Scenario: 3. Client Application Types that we do not understand are ignored
    Given MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                  |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes                      |
        | browser-based,native-mobile,elephant,cheese |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId |
        | Sln1       | MedicOnline    | 1                |
        | Sln2       | TakeTheRedPill | 1                |
        | Sln3       | PracticeMgr    | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [],  "MobileResponsive": null, "Plugins": null } |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                                                                     |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }                                                    |

@2726
Scenario: 4. Client Application Types can be completely cleared
    Given MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                  |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update solution Sln1 client-application-types section
        | ClientApplicationTypes |
        |                        |
    Then a response status of 400 is returned
    Then the client-application-types required field contains client-application-types
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId |
        | Sln1       | MedicOnline    | 1                |
        | Sln2       | TakeTheRedPill | 1                |
        | Sln3       | PracticeMgr    | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                  |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |

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
