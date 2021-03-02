Feature: Suppliers Edit Solution Description Section Validation
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is validated

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary is not filled out
    When a PUT request is made to update the solution-description section for solution Sln1
         | Summary                   | Description | Link |
         | A string with length of 0 | NULL        | NULL |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And Solutions exist
        | SolutionId | SolutionName |
        | Sln1       | MedicOnline  |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description | Link |
        | A string with length of 351 | NULL        | NULL |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName | 
        | Sln1       | MedicOnline  | 
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Description exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary   | Description                  | Link |
        | A Summary | A string with length of 1101 | NULL |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName |
        | Sln1       | MedicOnline  |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | AboutUrl | Features                          |
        | Sln1       | An full online medicine system | Online medicine 1 | UrlSln1  | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary   | Description                  | Link                         |
        | A Summary | It's the link's fault really | A string with length of 1001 |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
       And Solutions exist
        | SolutionId | SolutionName |
        | Sln1       | MedicOnline  |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary is not filled out & Description exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                   | Description                  | Link |
        | A string with length of 0 | A string with length of 1101 | NULL |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And the description field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName |
        | Sln1       | MedicOnline  |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary is not filled out & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                   | Description | Link                         |
        | A string with length of 0 | NULL        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And the link field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName |
        | Sln1       | MedicOnline  |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary & Description exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description                  | Link |
        | A string with length of 351 | A string with length of 1101 | Null |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And the summary field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName | 
        | Sln1       | MedicOnline  | 
    And solutions have the following details
        | SolutionId | AboutUrl |SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  |An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description | Link                         |
        | A string with length of 351 | NULL        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName | 
        | Sln1       | MedicOnline  | 
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Description & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary   | Description                  | Link                         |
        | A Summary | A string with length of 1101 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And the description field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName |
        | Sln1       | MedicOnline  |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary is not filled out, Description & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                   | Description                  | Link                         |
        | A string with length of 0 | A string with length of 1101 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure required
    And the description field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
       And Solutions exist
        | SolutionId | SolutionName | 
        | Sln1       | MedicOnline  | 
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: Summary, Description & Link exceeds the character limit
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                     | Description                  | Link                         |
        | A string with length of 351 | A string with length of 1101 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And the description field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName | 
        | Sln1       | MedicOnline  | 
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

