Feature: Suppliers Edit Solution Description Section Validation
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is validated

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 1. Summary is not filled out
    Given a request where the summary is a string of 0 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the required field contains summary
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |


@3319
Scenario: 2. Summary exceeds the character limit
    Given a request where the summary is a string of 301 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains summary
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 3. Description exceeds the character limit
    Given a request where the description is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains description
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | AboutUrl | Features                          |
        | Sln1     | An full online medicine system | Online medicine 1 | UrlSln1  | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 4. Link exceeds the character limit
    Given a request where the link is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains link
       And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 5. Summary is not filled out & Description exceeds the character limit
    Given a request where the summary is a string of 0 characters
    And a request where the description is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the required field contains summary
    And the maxLength field contains description
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 6. Summary is not filled out & Link exceeds the character limit
    Given a request where the summary is a string of 0 characters
    And a request where the link is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the required field contains summary
    And the maxLength field contains link
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 7. Summary & Description exceeds the character limit
    Given a request where the summary is a string of 301 characters
    And a request where the description is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains description
    And the maxLength field contains summary
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl |SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  |An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 8. Summary & Link exceeds the character limit
    Given a request where the summary is a string of 301 characters
    And a request where the link is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains summary
    And the maxLength field contains link
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 9. Description & Link exceeds the character limit
    Given a request where the description is a string of 1001 characters
    And a request where the link is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains link
    And the maxLength field contains description
    And Solutions exist
        | SolutionID | SolutionName |
        | Sln1       | MedicOnline  |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 10. Summary is not filled out, Description & Link exceeds the character limit
    Given a request where the summary is a string of 0 characters
    And a request where the description is a string of 1001 characters
    And a request where the link is a string of 1001 characters
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the required field contains summary
    And the maxLength field contains description
    And the maxLength field contains link
       And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 11. Summary, Description & Link exceeds the character limit
    Given a request where the summary is a string of 301 characters
    And a request where the description is a string of 1001 characters
    And a request where the link is a string of 1001 characters    
    When the update solution description request is made for Sln1
    Then a response status of 400 is returned
    And the maxLength field contains summary
    And the maxLength field contains description
    And the maxLength field contains link
    And Solutions exist
        | SolutionID | SolutionName | 
        | Sln1       | MedicOnline  | 
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription   | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1 | [ "Appointments", "Prescribing" ] |

