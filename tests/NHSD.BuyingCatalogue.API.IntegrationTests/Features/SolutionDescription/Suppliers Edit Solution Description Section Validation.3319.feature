Feature: Suppliers Edit Solution Description Section Validation
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is validated

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
        | Solution | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 1. Summary is not filled out
    When a PUT request is made to update the solution-description section for solution Sln1
         | Summary                   | Description | Link |
         | A string with length of 0 | NULL        | NULL |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 2. Summary exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description | Link |
        | A string with length of 301 | NULL        | NULL |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 3. Description exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary   | Description                  | Link |
        | A Summary | A string with length of 1001 | NULL |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | AboutUrl | Features                          |
        | Sln1     | An full online medicine system | Online medicine 1 | UrlSln1  | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 4. Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary   | Description                  | Link                         |
        | A Summary | It's the link's fault really | A string with length of 1001 |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
       And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 5. Summary is not filled out & Description exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                   | Description                  | Link |
        | A string with length of 0 | A string with length of 1001 | NULL |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And the description field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 6. Summary is not filled out & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                   | Description | Link                         |
        | A string with length of 0 | NULL        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And the link field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 7. Summary & Description exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description                  | Link |
        | A string with length of 301 | A string with length of 1001 | Null |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And the summary field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl |SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  |An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 8. Summary & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description | Link                         |
        | A string with length of 301 | NULL        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 9. Description & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary   | Description                  | Link                         |
        | A Summary | A string with length of 1001 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And the description field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 10. Summary is not filled out, Description & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                   | Description                  | Link                         |
        | A string with length of 0 | A string with length of 1001 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And the description field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
       And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 11. Summary, Description & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description                  | Link                         |
        | A string with length of 301 | A string with length of 1001 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And the description field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

