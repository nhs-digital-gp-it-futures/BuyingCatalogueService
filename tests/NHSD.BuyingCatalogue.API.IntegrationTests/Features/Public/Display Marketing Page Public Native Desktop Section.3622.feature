Feature: Display Marketing Page Public Native Desktop Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
        | Sln1     | { "ClientApplicationTypes" : [ "native-desktop"], "NativeDesktopHardwareRequirements": "A native desktop hardware requirement","NativeDesktopOperatingSystemsDescription": "A native desktop OS description", "NativeDesktopMinimumConnectionSpeed": "2Mbps", "NativeDesktopThirdParty": { "ThirdPartyComponents": "Components", "DeviceCapabilities": "Capabilities" }, "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": "3.5Ghz", "RecommendedResolution": "800x600" }, "NativeDesktopAdditionalInformation": "some additional information" } |

@3605
Scenario:1. Get Solution Public contains client application types native-desktop answers for all data
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the response contains the following values
		| Section                               | Field                               | Value                                 |
		| native-desktop-hardware-requirements  | hardware-requirements               | A native desktop hardware requirement |
		| native-desktop-connection-details     | minimum-connection-speed            | 2Mbps                                 |
		| native-desktop-operating-systems      | operating-systems-description       | A native desktop OS description       |
		| native-desktop-third-party            | third-party-components              | Components                            |
		| native-desktop-third-party            | device-capabilities                 | Capabilities                          |
		| native-desktop-memory-and-storage     | minimum-memory-requirement          | 1GB                                   |
		| native-desktop-memory-and-storage     | storage-requirements-description    | A description                         |
		| native-desktop-memory-and-storage     | minimum-cpu                         | 3.5Ghz                                |
		| native-desktop-memory-and-storage     | recommended-resolution              | 800x600                               |
		| native-desktop-additional-information | additional-information              | some additional information           |
