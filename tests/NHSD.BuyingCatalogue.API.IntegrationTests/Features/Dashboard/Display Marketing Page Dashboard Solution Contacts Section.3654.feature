Feature:  Display Marketing Page Dashboard Contacts Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Contacts
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | NoContact      | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription        | ClientApplication                                                    |
        | Sln1     | An full online medicine system | Online medicine 1      | { "ClientApplicationTypes" : [ "browser-based", "native-desktop" ] } |
        | Sln2     | Fully fledged GP system        | Fully fledged GP 12    |                                                                      |
        | Sln3     | We cannot be contacted         | Seriously, no contacts |                                                                      |
    And MarketingContacts exist
         | SolutionId | FirstName |
         | Sln1       | Bob       |
         | Sln3       |           |
        
@3654
Scenario: 1. Sections presented where the Solution exists
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    And the solution contact-details section status is COMPLETE
    And the solution contact-details section requirement is Optional

@3654
Scenario: 2.Sections presented where the MarketingContacts do not exist
    When a GET request is made for solution dashboard Sln2
    Then a successful response is returned
    And the solution contact-details section status is INCOMPLETE
    And the solution contact-details section requirement is Optional
    
@3654
Scenario: 3.Sections presented where the MarketingContacts have no details
    When a GET request is made for solution dashboard Sln3
    Then a successful response is returned
    And the solution contact-details section status is INCOMPLETE
    And the solution contact-details section requirement is Optional

