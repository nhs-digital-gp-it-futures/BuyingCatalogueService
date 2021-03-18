Feature:  Display Marketing Page Dashboard Client Application Type Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Client Application Types
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName     | SupplierId |
        | Sln1       | MedicOnline      | Sup 1      |
        | Sln2       | TakeTheRedPill   | Sup 1      |
        | Sln3       | PracticeMgr      | Sup 1      |
        | Sln4       | PracticeMgr      | Sup 1      |
        | Sln5       | Integral         | Sup 1      |
        | Sln6       | Medical Stuff    | Sup 1      |
        | Sln7       | TooCoolForSchool | Sup 1      |
        | Sln8       | MedicsAnonymous  | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                                                                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] }                                                                                                                                                                                                                                           |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                                                                                                                                                                                                                |
        | Sln4       | Thrills                        | Bellyaches          | { "ClientApplicationTypes" : [] }                                                                                                                                                                                                                                                                              |
        | Sln5       | Fully fledged GP system        | Online medicine 1   | { "ClientApplicationTypes" : [ "browser-based", "native-desktop", "native-mobile" ] }                                                                                                                                                                                                                          |
        | Sln6       | More Summaries                 | Online System       | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": true, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "MobileFirstDesign": true, "MinimumConnectionSpeed": "Connection Speed" }                                            |
        | Sln7       | More Summaries                 | Online System       | { "ClientApplicationTypes": ["native-mobile"], "MobileOperatingSystems": { "OperatingSystems": ["Windows"] }, "NativeMobileFirstDesign": false, "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" }}                                                              |
        | Sln8       | More Summaries                 | Online System       | { "ClientApplicationTypes": ["native-desktop"], "NativeDesktopOperatingSystemsDescription": "Some Description", "NativeDesktopMinimumConnectionSpeed": "2Mbps", "NativeDesktopMemoryAndStorage": { "MinimumMemoryRequirement": "512MB", "StorageRequirementsDescription": "Some Desc", "MinimumCpu": "min" } } |
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |
        | Sln4       | true         | NHSDGP001   |
        | Sln5       | false        | DFOCVC001   |
        | Sln6       | false        | NHSDGP001   |
        | Sln7       | true         | NHSDGP001   |
        | Sln8       | false        | DFOCVC001   |

@2724
Scenario: Sections presented where the Solution exists
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    And the solution client-application-types section status is COMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario Outline: Sections presented where the SolutionDetail does not exist
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution client-application-types section status is INCOMPLETE
    And the solution client-application-types section requirement is Mandatory
Examples:
    | SolutionId |
    | Sln2       |
    | Sln3       |

@2724
Scenario: Sections presented where ClientApplicationTypes is empty
    When a GET request is made for solution dashboard Sln4
    Then a successful response is returned
    And the solution client-application-types section status is INCOMPLETE
    And the solution client-application-types section requirement is Mandatory

@2724
Scenario: Sections Mandatory when ClientApplicationTypes is set
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    And the solution client-application-types section browser-based subsection requirement is Mandatory
    And the solution client-application-types section native-mobile subsection requirement is Mandatory
    And the solution client-application-types section native-desktop subsection requirement is Mandatory

@3597
Scenario: Section Browser Based is Incomplete
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    Then the solution client-application-types section browser-based subsection status is INCOMPLETE
    And the solution client-application-types section browser-based subsection requirement is Mandatory

@3597
Scenario: Section Browser Based is Complete
    When a GET request is made for solution dashboard Sln6
    Then a successful response is returned
    Then the solution client-application-types section browser-based subsection status is COMPLETE
    And the solution client-application-types section browser-based subsection requirement is Mandatory

@3612
Scenario: Section Native Mobile is Incomplete
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    Then the solution client-application-types section native-mobile subsection status is INCOMPLETE
    And the solution client-application-types section native-mobile subsection requirement is Mandatory

@3612
Scenario: Section Native Mobile is Complete
    When a GET request is made for solution dashboard Sln7
    Then a successful response is returned
    Then the solution client-application-types section native-mobile subsection status is COMPLETE
    And the solution client-application-types section native-mobile subsection requirement is Mandatory

@3615
Scenario: Section Native Desktop is Incomplete
    When a GET request is made for solution dashboard Sln5
    Then a successful response is returned
    Then the solution client-application-types section native-desktop subsection status is INCOMPLETE
    And the solution client-application-types section native-desktop subsection requirement is Mandatory

@3615
Scenario: 10. Section Native Desktop is Complete
    When a GET request is made for solution dashboard Sln8
    Then a successful response is returned
    Then the solution client-application-types section native-desktop subsection status is COMPLETE
    And the solution client-application-types section native-desktop subsection requirement is Mandatory
