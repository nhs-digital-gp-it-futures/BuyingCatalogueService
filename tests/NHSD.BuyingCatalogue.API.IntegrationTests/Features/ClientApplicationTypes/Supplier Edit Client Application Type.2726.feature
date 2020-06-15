Feature: Supplier Edit Client Application Type
    As a Supplier
    I want to Edit the ClientApplicationType Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId | SummaryDescription             | FullDescription     | ClientApplication                                                                      |
        | Sln1       | MedicOnline    | Sup 1      | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                   |
        | Sln2       | TakeTheRedPill | Sup 2      | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                   |
        | Sln3       | PracticeMgr    | Sup 2      | Thrills                        | Bellyaches          |  { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |

@2726
Scenario: 1. Client Application Types are updated for the solution
    When a PUT request is made to update the client-application-types section for solution Sln1
        | ClientApplicationTypes       |
        | browser-based,native-mobile |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [] } |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                         |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }        |
    And Last Updated has updated on the SolutionDetail for solution Sln1
    
@2726
Scenario: 2. Client Application Types are updated for the solution with trimmed whitespace
    When a PUT request is made to update the client-application-types section for solution Sln1
        | ClientApplicationTypes                              |
        | "      browser-based     ", "    native-mobile    " |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [] } |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                         |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }        |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2726
@ignore # solution detail will always be present now
Scenario: 3. If SolutionDetail is missing for the solution, thats an error case
	Given a SolutionDetail Sln1 does not exist
    When a PUT request is made to update the client-application-types section for solution Sln1
        | ClientApplicationTypes      |
        | browser-based,native-mobile |
    Then a response status of 500 is returned

@2726
Scenario: 4. Client Application Types that we do not understand are ignored
    When a PUT request is made to update the client-application-types section for solution Sln1
        | ClientApplicationTypes                      |
        | browser-based,native-mobile,elephant,cheese |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-mobile" ], "BrowsersSupported": [] } |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | { }                                                                                          |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] }        |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2726
Scenario: 5. Client Application Types cannot be completely cleared
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                  |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |
    When a PUT request is made to update the client-application-types section for solution Sln1
        | ClientApplicationTypes |
        |                        |
    Then a response status of 400 is returned
    Then the client-application-types field value is the validation failure required
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                  |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12 | {  }                                                                                  |
        | Sln3     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [ "browser-based", "native-mobile", "native-desktop" ] } |

@2726
Scenario: 6. Solution not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the client-application-types section for solution Sln4
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 404 is returned

@2726
Scenario: 7. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the client-application-types section for solution Sln1
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 500 is returned

@2726
Scenario: 8. Solution id not present in request
    When a PUT request is made to update the client-application-types section with no solution id
        | ClientApplicationTypes       |
        | browser-based,native-desktop |
    Then a response status of 400 is returned
