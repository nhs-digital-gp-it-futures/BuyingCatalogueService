Feature: Display Marketing Page Preview Browser Based Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName        | SummaryDescription          | OrganisationName | FullDescription         | SupplierStatusId |
        | Sln1       | MedicOnline         |                             | GPs-R-Us         | Online medicine 1       | 1                |
        | Sln2       | TakeTheRedPill      | Eye opening experience      | Drs. Inc         | Eye opening6            | 1                |
        | Sln3       | PracticeMgr         | Fully fledged GP system     | Drs. Inc         | Fully fledged GP 12     | 1                |
        | Sln4       | SubStandardPractice | Not Quite fledged GP system | GPs-R-Us         | Not Quite fledged GP 16 | 1                |
        | Sln5       | Banana              | Fruit delivery system       | Drs. Inc         | Banana 1152             | 1                |
        | Sln6       | Water Bottle        | Water supplier system       | Drs. Inc         | High quality H20        | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                                               |
        | Sln1     | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ], "BrowsersSupported": ["Google Chrome", "Edge"], "MobileResponsive": false } |
        | Sln3     | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported": [ ], "MobileResponsive": null }                                          |
        | Sln4     | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported": [ ], "MobileResponsive": true }                                          |
        | Sln5     | { "ClientApplicationTypes" : [ "browser-based" ], "BrowsersSupported": ["Google Chrome", "Edge", "Safari"] }                                    |
        | Sln6     | { "ClientApplicationTypes" : [ "browser-based" ], "Plugins": { "Required": true, "AdditionalInformation": "Colourful water extension" } }       |

@3322
Scenario:1. Get Solution Preview contains client application types browser based answers for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution client-application-types section contains Browsers
        | Browser       |
        | Google Chrome |
        | Edge          |
    And the solution client-application-types section contains mobile responsive with value no

@3322
Scenario:2. Get Solution Preview contains client application types browser based mobile responsive answer
    When a GET request is made for solution preview Sln4
    Then a successful response is returned
    And the solution client-application-types section contains mobile responsive with value yes
    And the solution client-application-types section contains Browsers
        | Browser       |

@3322
Scenario:3. Get Solution Preview contains client application types browser based browser supported answer
    When a GET request is made for solution preview Sln5
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution client-application-types section contains Browsers
        | Browser       |
        | Google Chrome |
        | Edge          |
        | Safari        |
    And the solution client-application-types section contains mobile responsive with value null

@2793
Scenario:4. Get Solution Preview contains client application types browser based plugin required answer
    When a GET request is made for solution preview Sln6
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution client-application-types section contains plugin required with value yes
    And the solution client-application-types section contains Browsers
        | Browser       |
    And the solution client-application-types section contains mobile responsive with value null

@2793
Scenario:5. Get Solution Preview contains client application types browser based plugin detail answer
    When a GET request is made for solution preview Sln6
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution client-application-types section contains plugin detail with value Colourful water extension
    And the solution client-application-types section contains Browsers
        | Browser       |
    And the solution client-application-types section contains mobile responsive with value null
