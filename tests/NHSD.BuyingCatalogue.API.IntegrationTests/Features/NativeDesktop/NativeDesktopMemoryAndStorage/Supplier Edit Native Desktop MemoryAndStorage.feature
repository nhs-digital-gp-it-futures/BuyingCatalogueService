Feature:  Supplier Edit Native Desktop Memory, Storage, Processing and Resolution
    As a Supplier
    I want to Edit the Native Desktop Memory, Storage, Processing and Resolution Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |

@3620
Scenario: Native Desktop Memory, Storage, Processing and Resolution is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu | RecommendedResolution |
        | 512                      | SSD > HDD                      | 1337 Ghz   | 1x1 px                |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeDesktopMemoryAndStorage": { "MinimumMemoryRequirement": "512", "StorageRequirementsDescription": "SSD > HDD", "MinimumCpu": "1337 Ghz", "RecommendedResolution" : "1x1 px" } } |

@3620
Scenario: 2 Native Desktop Memory, Storage, Processing and Resolution is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription         | MinimumCpu           | RecommendedResolution |
        | "           512"         | "        SSD > HDD                   " | "    1337 Ghz      " | "     1x1 px"         |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeDesktopMemoryAndStorage": { "MinimumMemoryRequirement": "512", "StorageRequirementsDescription": "SSD > HDD", "MinimumCpu": "1337 Ghz", "RecommendedResolution" : "1x1 px" } } |

@3620
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln2
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu | RecommendedResolution |
        | 512                      | SSD > HDD                      | 1337 Ghz   | 1x1 px                |
    Then a response status of 404 is returned 

@3620
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu | RecommendedResolution |
        | 512                      | SSD > HDD                      | 1337 Ghz   | 1x1 px                |
    Then a response status of 500 is returned

@3620
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-desktop-memory-and-storage section with no solution id
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu | RecommendedResolution |
        | 512                      | SSD > HDD                      | 1337 Ghz   | 1x1 px                |
    Then a response status of 400 is returned
