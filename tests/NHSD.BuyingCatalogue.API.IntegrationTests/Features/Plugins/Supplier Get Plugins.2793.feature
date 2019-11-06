Feature:  Display Marketing Page Form Browsers Supported Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Browsers Support
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
        | Sln5       | SolutionTest   | Testing System                 | GPs-R-Us         | Full System         | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                                                                                                   |
        | Sln1     | { "PlugIns" : { "Required" : "yes", "AdditionalInformation": "orem ipsum…." }}, { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |
        | Sln3     |                                                                                                                                                                                                     |
        | Sln5     | { "PlugIns" : { "Required" : "yes"}, { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false }                                            |

@2786
Scenario: 1. Plugins are retrieved for the solution
    When a GET request is made for plug-ins-or-extensions for solution Sln1
    Then a successful response is returned
    And the required string is yes
    And the addition-information string is 'orem ipsum….'

@2786
Scenario: 2. Plugins are retrieved for the solution where no marketing detail exists
    When a GET request is made for plug-ins-or-extensions for solution Sln2
    Then a successful response is returned
    And the required string is null
    And the addition-information string is null

@2786
Scenario: 3. Plugins are retrieved for the solution where no plug-ins exist
    When a GET request is made for plug-ins-or-extensions for solution Sln3
    Then a successful response is returned
    And the required string is null
    And the addition-information string is null

@2786
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for plug-ins-or-extensions for solution Sln4
    Then a response status of 404 is returned

@2786
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for plug-ins-or-extensions for solution Sln1
    Then a response status of 500 is returned

@2786
Scenario: 6. Solution id not present in request
    When a GET request is made for plug-ins with no solution id
    Then a response status of 400 is returned
    
@2786
Scenario: 7. Plugins are retrieved for the solution where no plugins-required
    When a GET request is made for plug-ins-or-extensions for solution Sln5
    Then a successful response is returned
    And the required string is yes
    And the addition-information string is null
