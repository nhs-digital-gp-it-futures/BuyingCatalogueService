Feature:  Display Marketing Page Dashboard Client Application Type Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Client Application Types
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName     | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline      | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill   | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | PracticeMgr      | GPs-R-Us         | 1                | Sup 1      |
        | Sln4       | PracticeMgr      | GPs-R-Us         | 1                | Sup 1      |
        | Sln5       | Integral         | GPs-R-Us         | 1                | Sup 1      |
        | Sln6       | Medical Stuff    | GPs-R-Us         | 1                | Sup 1      |
        | Sln7       | TooCoolForSchool | GPs-R-Us         | 1                | Sup 1      |
        | Sln8       | MedicsAnonymous  | GPs-R-Us         | 1                | Sup 1      |

    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                                                                                                                                              |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                                                                                                                                                                                                                                           |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                                                                                                                                                                                                                |
        | Sln4     | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [] }                                                                                                                                                                                                                                                                              |
        | Sln5     | Fully fledged GP system        | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop", "native-mobile" ] }                                                                                                                                                                                                                          |
        | Sln6     | More Summaries                 | Online System       | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": true, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "MobileFirstDesign": true, "MinimumConnectionSpeed": "Connection Speed" }                                            |
        | Sln7     | More Summaries                 | Online System       | { "ClientApplicationTypes": ["native-mobile"], "MobileOperatingSystems": { "OperatingSystems": ["Windows"] }, "NativeMobileFirstDesign": false, "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" }}                                                              |
        | Sln8     | More Summaries                 | Online System       | { "ClientApplicationTypes": ["native-desktop"], "NativeDesktopOperatingSystemsDescription": "Some Description", "NativeDesktopMinimumConnectionSpeed": "2Mbps", "NativeDesktopMemoryAndStorage": { "MinimumMemoryRequirement": "512MB", "StorageRequirementsDescription": "Some Desc", "MinimumCpu": "min" } } |
        

@2724
Scenario: 1. Sections presented where the Solution exists
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    And the solution client-application-types section status is COMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario Outline: 2.Sections presented where the SolutionDetail does not exist
    When a GET request is made for solution dashboard <Solution>
    Then a successful response is returned
    And the solution client-application-types section status is INCOMPLETE
    And the solution client-application-types section requirement is Mandatory
Examples:
    | Solution |
    | Sln2     |
    | Sln3     |

@2724
Scenario: 3. Sections presented where ClientApplicationTypes is empty
    When a GET request is made for solution dashboard Sln4
    Then a successful response is returned
    And the solution client-application-types section status is INCOMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario: 4. Sections Mandatory when ClientApplicationTypes is set
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    And the solution browser-based section requirement is Mandatory
    And the solution native-desktop section requirement is Mandatory
    And the solution native-mobile section requirement is Mandatory

@3597
Scenario: 5. Section Browser Based is Incomplete
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    Then the solution browser-based status is INCOMPLETE
    And the solution browser-based section requirement is Mandatory

@3597
Scenario: 6. Section Browser Based is Complete
    When a GET request is made for solution dashboard Sln6
    Then a successful response is returned
    Then the solution browser-based status is COMPLETE
    And the solution browser-based section requirement is Mandatory

@3612
Scenario: 7. Section Native Mobile is Incomplete
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    Then the solution native-mobile status is INCOMPLETE
    And the solution native-mobile section requirement is Mandatory

@3612
Scenario: 8. Section Native Mobile is Complete
    When a GET request is made for solution dashboard Sln7
    Then a successful response is returned
    Then the solution native-mobile status is COMPLETE
    And the solution native-mobile section requirement is Mandatory

@3615
Scenario: 9. Section Native Desktop is Incomplete
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    Then the solution native-desktop status is INCOMPLETE
    And the solution native-desktop section requirement is Mandatory

@3615
Scenario: 10. Section Native Desktop is Complete
    When a GET request is made for solution dashboard Sln8
    Then a successful response is returned
    Then the solution native-desktop status is COMPLETE
    And the solution native-desktop section requirement is Mandatory


