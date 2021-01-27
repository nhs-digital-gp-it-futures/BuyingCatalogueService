Feature:  Display Marketing Page Form On Premise Hosting Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's On Premise Hosting
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": { "Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3651
Scenario: Summary exceeds the maxLength
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary                     | Link  | HostingModel | RequiresHSCN |
        | A string with length of 501 | a@b.c | AZDO         | A new string |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                    |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3651
Scenario: Link exceeds the maxLength
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary   | Link                         | HostingModel | RequiresHSCN |
        | A summary | A string with length of 1001 | AZDO         | A new string |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                    |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3651
Scenario: Hosting Type exceeds the maxLength
    When a PUT request is made to update the hosting-type-on-premise section for solution Sln1
        | Summary   | Link  | HostingModel                 | RequiresHSCN |
        | A summary | a@b.c | A string with length of 1001 | A new string |
    Then a response status of 400 is returned
    And the hosting-model field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                    |
        | Sln1     | An full online medicine system | Online medicine 1 | { "OnPremise": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |
