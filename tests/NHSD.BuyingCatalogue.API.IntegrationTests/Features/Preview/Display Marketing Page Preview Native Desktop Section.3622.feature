Feature: Display Marketing Page Preview Native Desktop Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
        | Sln1     | Online Description | Online medicine 1 | { "ClientApplicationTypes" : [ "native-desktop"], "NativeDesktopHardwareRequirements": "A native desktop hardware requirement","NativeDesktopOperatingSystemsDescription": "A native desktop OS description", "NativeDesktopMinimumConnectionSpeed": "2Mbps", "NativeDesktopThirdParty": { "ThirdPartyComponents": "Components", "DeviceCapabilities": "Capabilities" }, "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": "3.5Ghz", "RecommendedResolution": "800x600" }, "NativeDesktopAdditionalInformation": "some additional information" } |

@3605
Scenario:1. Get Solution Preview contains client application types native-desktop answers for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution native-desktop native-desktop-hardware-requirements section contains hardware-requirements with value A native desktop hardware requirement
    And the solution native-desktop native-desktop-connection-details section contains minimum-connection-speed with value 2Mbps
    And the solution native-desktop native-desktop-operating-systems section contains operating-systems-description with value A native desktop OS description
    And the solution native-desktop native-desktop-third-party section contains third-party-components with value Components
    And the solution native-desktop native-desktop-third-party section contains device-capabilities with value Capabilities
    And the solution native-desktop native-desktop-memory-and-storage section contains minimum-memory-requirement with value 1GB
    And the solution native-desktop native-desktop-memory-and-storage section contains storage-requirements-description with value A description
    And the solution native-desktop native-desktop-memory-and-storage section contains minimum-cpu with value 3.5Ghz
    And the solution native-desktop native-desktop-memory-and-storage section contains recommended-resolution with value 800x600
    And the solution native-desktop native-desktop-additional-information section contains additional-information with value some additional information
