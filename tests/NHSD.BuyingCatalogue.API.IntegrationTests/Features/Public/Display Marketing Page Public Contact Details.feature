Feature: Display Marketing Page Public Contact Details
    As a Public User
    I want to view the Solution Contact information
    So that I can understand who the Solution Contacts are

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sol1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sol2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription | Features                          |
        | Sol1     | UrlSln1  | The best solution  | [ "Appointments", "Prescribing" ] |

@3507
Scenario: 1. Both contacts are presented when two exist
    Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Sol1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales      |
        | Sol1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Complaints |
    When a GET request is made for solution public Sol1
    Then a successful response is returned
    And the solutions contact-1 has details
        | Name             | Email         | PhoneNumber  | Department |
        | Bob Bobbington   | bob@bob.bob   | 66666 666666 | Sales      |
    And the solutions contact-2 has details
        | Name             | Email         | PhoneNumber  | Department |
        | Betty Bobbington | betty@bob.bob | 99999 999999 | Complaints |

@3507
Scenario: 2. Only populated details are presented when a solution is requested
Given MarketingContacts exist
        | SolutionId | FirstName |
        | Sol1       | Bob       |
        | Sol1       | Betty     |
    When a GET request is made for solution public Sol1
    Then a successful response is returned
    And the solutions contact-1 has details
        | Name |
        | Bob  |
    And the solutions contact-2 has details
         | Name  |
         | Betty |

@3507
Scenario: 3. A single contact is presented when there is only one available
    Given MarketingContacts exist
        | SolutionId | FirstName |
        | Sol1       | Bob       |
        | Sol2       | Betty     |
    When a GET request is made for solution public Sol1
    Then a successful response is returned
    And the solutions contact-1 has details
        | Name |
        | Bob  |
    And there is no contact-2 for the solution

@3507
Scenario: 4. No contacts are presented when there aren't any available
    Given No contacts exist for solution Sol1
    When a GET request is made for solution public Sol1
    Then a successful response is returned
    And there is no contact-1 for the solution
    And there is no contact-2 for the solution

@3507
Scenario: 5. Two contacts are presented when more than two are available
    Given MarketingContacts exist
        | SolutionId | FirstName |
        | Sol1       | Bob       |
        | Sol1       | Betty     |
        | Sol1       | Bart      |
    When a GET request is made for solution public Sol1
    Then a successful response is returned
    And the solutions contact-1 has details
        | Name |
        | Bob  |
    And the solutions contact-2 has details
        | Name  |
        | Betty |
    And there is no contact-3 for the solution
