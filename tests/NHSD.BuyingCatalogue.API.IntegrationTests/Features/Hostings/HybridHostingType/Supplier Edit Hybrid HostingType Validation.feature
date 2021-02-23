Feature:  Display Marketing Page Form Hybrid HostingType Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Hybrid HostingType
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | Hosting                                                                                                                             |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HybridHostingType": { "Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3644
Scenario: Summary exceeds the maxLength
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary                     | Link  | HostingModel | RequiresHSCN |
        | A string with length of 501 | a@b.c | AZDO         | A new string |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | Hosting                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HybridHostingType": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3644
Scenario: Link exceeds the maxLength
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary   | Link                         | HostingModel | RequiresHSCN |
        | A summary | A string with length of 1001 | AZDO         | A new string |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | Hosting                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HybridHostingType": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3644
Scenario: Hosting model exceeds the maxLength
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary   | Link  | HostingModel                 | RequiresHSCN |
        | A summary | a@b.c | A string with length of 1001 | A new string |
    Then a response status of 400 is returned
    And the hosting-model field value is the validation failure maxLength
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | Hosting                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HybridHostingType": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |
