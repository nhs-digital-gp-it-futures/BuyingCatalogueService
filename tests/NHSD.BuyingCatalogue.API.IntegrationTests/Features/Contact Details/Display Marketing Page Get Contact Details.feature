Feature: Display Marketing Page Public Contact Details
    As a Public User
    I want to view the Solution Contact information
    So that I can understand who the Solution Contacts are

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription | Features                          |
        | Sln1       | UrlSln1  | The best solution  | [ "Appointments", "Prescribing" ] |

@3655
Scenario: Both contacts are presented when two exist
    Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | DepartmentName |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales          |
        | Sln1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Complaints     |
    When a GET request is made for contact-details section for solution Sln1
    Then a successful response is returned
    And the contact-detail contact-1 has details
        | FirstName | LastName   | PhoneNumber  | EmailAddress | DepartmentName |
        | Bob       | Bobbington | 66666 666666 | bob@bob.bob  | Sales          |
    And the contact-detail contact-2 has details
        | FirstName | LastName   | PhoneNumber  | EmailAddress | DepartmentName |
        | Betty     | Bobbington | 99999 999999 | betty@bob.bob  | Complaints     |

@3655
Scenario: Some Contact Details are missing
Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | DepartmentName |
        | Sln1       | Bob       |            | bob@bob.bob   | 66666 666666 | Sales          |
        | Sln1       |           | Bobbington | betty@bob.bob | 99999 999999 |                |
    When a GET request is made for contact-details section for solution Sln1
    Then a successful response is returned
    And the contact-detail contact-1 has details
        | FirstName | LastName | EmailAddress | PhoneNumber  | DepartmentName |
        | Bob       |          | bob@bob.bob  | 66666 666666 | Sales          |
    And the contact-detail contact-2 has details
        | FirstName | LastName   | EmailAddress  | PhoneNumber  | DepartmentName |
        |           | Bobbington | betty@bob.bob | 99999 999999 |                |

@3655
Scenario: A single contact is presented when there is only one available
    Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | EmailAddress  | PhoneNumber  | DepartmentName |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales          |
    When a GET request is made for contact-details section for solution Sln1
    Then a successful response is returned
    And the contact-detail contact-1 has details
        | FirstName | LastName   | EmailAddress | PhoneNumber  | DepartmentName |
        | Bob       | Bobbington | bob@bob.bob  | 66666 666666 | Sales          |
    And there is no contact-2 for the contact-detail

@3655
Scenario: No contacts are presented when there aren't any available
    Given No contacts exist for solution Sln1
    When a GET request is made for contact-details section for solution Sln1
    Then a successful response is returned
    And there is no contact-1 for the contact-detail
    And there is no contact-2 for the contact-detail

@3655
Scenario: Two contacts are presented when more than two are available
    Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | DepartmentName |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales          |
        | Sln1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Complaints     |
        | Sln1       | Bart      | Simpson    | bart@s.com    | 11111 111111 | GP             |
    When a GET request is made for contact-details section for solution Sln1
    Then a successful response is returned
    And the contact-detail contact-1 has details
        | SolutionId | FirstName | LastName   | EmailAddress | PhoneNumber  | DepartmentName |
        | Sln1       | Bob       | Bobbington | bob@bob.bob  | 66666 666666 | Sales          |
    And the contact-detail contact-2 has details
        | FirstName | LastName   | EmailAddress  | PhoneNumber  | DepartmentName |
        | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Complaints     |
    And there is no contact-3 for the contact-detail

@3655
Scenario: Solution not found
    Given a Solution Sln2 does not exist
    When a GET request is made for contact-details section for solution Sln2
    Then a response status of 404 is returned

@3655
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for contact-details section for solution Sln2
    Then a response status of 500 is returned

@3655
Scenario: Solution id not present in request
    When a GET request is made for contact-details section with no solution id
    Then a response status of 400 is returned
