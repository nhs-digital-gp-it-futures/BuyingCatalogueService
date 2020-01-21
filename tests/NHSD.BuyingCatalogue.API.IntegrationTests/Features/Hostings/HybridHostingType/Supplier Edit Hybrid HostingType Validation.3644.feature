Feature:  Display Marketing Page Form Hybrid HostingType Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Hybrid HostingType
    So that I can ensure the information is correct & valid

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": { "Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3644
Scenario: 1. Summary exceeds the maxLength
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary                     | Link  | HostingModel | RequiresHSCN |
        | A string with length of 501 | a@b.c | AZDO         | A new string |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                    |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3644
Scenario: 2. Link exceeds the maxLength
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary   | Link                         | HostingModel | RequiresHSCN |
        | A summary | A string with length of 1001 | AZDO         | A new string |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                    |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |

@3644
Scenario: 3. Hosting Type exceeds the maxLength
    When a PUT request is made to update the hosting-type-hybrid section for solution Sln1
        | Summary   | Link  | HostingModel                 | RequiresHSCN |
        | A summary | a@b.c | A string with length of 1001 | A new string |
    Then a response status of 400 is returned
    And the hosting-model field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                    |
        | Sln1     | An full online medicine system | Online medicine 1 | { "HybridHostingType": {"Summary": "A Summary", "Link": "A Link", "HostingModel": "A hosting type", "RequiresHSCN": "A string" } } |
