Feature: Display Marketing Page Form Public Cloud Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Public Cloud
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |

@3639
Scenario: 1. Summary exceeds its maxLength
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary                     | Link     | RequiresHSCN                |
        | A string with length of 501 | someLink | It needs another connection |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
       
@3639
Scenario: 2. URL exceeds its maxLength 
 When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary         | Link                         | RequiresHSCN                |
        | Another Summary | A string with length of 1001 | It needs another connection |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |

@3639
Scenario: 3. Summary & URL exceeds their maxLength
    When a PUT request is made to update the hosting-type-public-cloud section for solution Sln1
        | Summary                     | Link                         | RequiresHSCN                |
        | A string with length of 501 | A string with length of 1001 | It needs another connection |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | Hosting                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
