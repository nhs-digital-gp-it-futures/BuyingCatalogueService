Feature: Supplier Display Sub-Form Native Desktop Client Type
    As a Supplier
    I want to Display Sub-Form Native Desktop Client Type
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                | Sup 2      |

@3614
Scenario: Sub-Form Native Desktop Client Type all sections are Displayed
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And Solutions section contains all items  
        | Id                                    | Status     | Requirement |
        | native-desktop-operating-systems      | INCOMPLETE | Mandatory   |
        | native-desktop-connection-details     | INCOMPLETE | Mandatory   |
        | native-desktop-memory-and-storage     | INCOMPLETE | Mandatory   |
        | native-desktop-third-party            | INCOMPLETE | Optional    |
        | native-desktop-hardware-requirements  | INCOMPLETE | Optional    |
        | native-desktop-additional-information | INCOMPLETE | Optional    |

@3614
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop section dashboard for solution Sln4
    Then a response status of 404 is returned

@3614
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a response status of 500 is returned

@3614
Scenario: Solution id not present in request
    When a GET request is made for native-desktop section dashboard with no solution id
    Then a response status of 400 is returned
    
@3622
Scenario Outline: Native Desktop Hardware Requirements based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-desktop-hardware-requirements section status is <Status>
Examples:
    | ClientApplication                                           | Status     |
    |                                                             | INCOMPLETE |
    | { "NativeDesktopHardwareRequirements": null }               | INCOMPLETE |
    | { "NativeDesktopHardwareRequirements": "      " }           | INCOMPLETE |
    | { "NativeDesktopHardwareRequirements": "Hardware Details" } | COMPLETE   |

@3617
Scenario Outline: Native Desktop Operating Systems Description based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-desktop-operating-systems section status is <Status>
Examples:
    | ClientApplication                                                                | Status     |
    |                                                                                  | INCOMPLETE |
    | { "NativeDesktopOperatingSystemsDescription" : null }                            | INCOMPLETE |
    | { "NativeDesktopOperatingSystemsDescription" : "      " }                        | INCOMPLETE |
    | { "NativeDesktopOperatingSystemsDescription" : "Operating systems description" } | COMPLETE   |

@3619
Scenario Outline: Native Desktop Connectivity Details based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-desktop-connection-details section status is <Status>
Examples:
    | ClientApplication                                   | Status     |
    |                                                     | INCOMPLETE |
    | { "NativeDesktopMinimumConnectionSpeed": null }     | INCOMPLETE |
    | { "NativeDesktopMinimumConnectionSpeed": "      " } | INCOMPLETE |
    | { "NativeDesktopMinimumConnectionSpeed": "6 Mbps" } | COMPLETE   |

@3621
Scenario Outline: Native Desktop Third Party based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-desktop-third-party section status is <Status>
Examples:
    | ClientApplication                                                                                          | Status     |
    |                                                                                                            | INCOMPLETE |
    | { "NativeDesktopThirdParty": null }                                                                        | INCOMPLETE |
    | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "      ", "DeviceCapabilities": "  " } }            | INCOMPLETE |
    | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "      ", "DeviceCapabilities": null } }            | INCOMPLETE |
    | { "NativeDesktopThirdParty": { "ThirdPartyComponents": null, "DeviceCapabilities": "     " } }             | INCOMPLETE |
    | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": null } }         | COMPLETE   |
    | { "NativeDesktopThirdParty": { "ThirdPartyComponents": null, "DeviceCapabilities": "Capability" } }        | COMPLETE   |
    | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } | COMPLETE   |
            
@3620
Scenario Outline: Native Desktop Memory And Storage Based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-desktop-memory-and-storage section status is <Status>
Examples:
    | ClientApplication                                                                                                                                     | Status     |
    |                                                                                                                                                       | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : {  } }                                                                                                            | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB" } }                                                                           | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": null } }                                                                            | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "" } }                                                                              | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "    " } }                                                                          | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": null } }                                   | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "" } }                                     | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "    " } }                                 | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description" } }                        | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": null} }     | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": ""} }       | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": "    "} }   | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": null, "StorageRequirementsDescription": "A description", "MinimumCpu": "3.5Ghz"} }  | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "", "MinimumCpu": "3.5Ghz"} }              | INCOMPLETE |
    | { "NativeDesktopMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "StorageRequirementsDescription": "A description", "MinimumCpu": "3.5Ghz"} } | COMPLETE   |

@3623
Scenario Outline: 10. Native Desktop Additional Information Details based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-desktop section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-desktop-additional-information section status is <Status>
Examples:
    | ClientApplication                                                       | Status     |
    |                                                                         | INCOMPLETE |
    | { "NativeDesktopAdditionalInformation": null }                          | INCOMPLETE |
    | { "NativeDesktopAdditionalInformation": "      " }                      | INCOMPLETE |
    | { "NativeDesktopAdditionalInformation": "Some additional information" } | COMPLETE   |
