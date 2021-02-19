Feature: Supplier Edit Contact Details
    As a Supplier
    I want to update the Solution Contact information
    So that I can modify who the Solution Contacts are

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | AboutUrl | SummaryDescription | Features                          |
        | Sln1       | UrlSln1  | The best solution  | [ "Appointments", "Prescribing" ] |

@3655
Scenario: Contacts are added when none existed before
    Given No contacts exist for solution Sln1
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Bob       |            | bob@bob.bob   | 66666 666666 | Sales      |
        |           | Bobbington | betty@bob.bob | 99999 999999 |            |
    Then a successful response is returned
    And MarketingContacts exist for solution Sln1
        | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Bob       |            | bob@bob.bob   | 66666 666666 | Sales      |
        |           | Bobbington | betty@bob.bob | 99999 999999 |            |
    And Last Updated has updated on the MarketingContact for solution Sln1

@3655
Scenario: Contacts are added with trimmed whitespace when none existed before
    Given No contacts exist for solution Sln1
    When a PUT request is made for solution Sln1 contact details
        | FirstName      | LastName         | Email                   | PhoneNumber           | Department      |
        | "      Bob   " | "       "        | "     bob@bob.bob     " | "    66666 666666   " | "    Sales    " |
        | "        "     | "    Bobbington" | "    betty@bob.bob   "  | "   99999 999999  "   | "         "     |
    Then a successful response is returned
    And MarketingContacts exist for solution Sln1
        | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Bob       |            | bob@bob.bob   | 66666 666666 | Sales      |
        |           | Bobbington | betty@bob.bob | 99999 999999 |            |
    And Last Updated has updated on the MarketingContact for solution Sln1

@3655
Scenario: Contacts are added when contacts previously existed
Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales      |
        | Sln1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Giraffe    |
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName   | Email          | PhoneNumber | Department |
        | Bill      | Billington | bill@bill.bill | 1           | Billing    |
    Then a successful response is returned
    And MarketingContacts exist for solution Sln1
        | FirstName | LastName   | Email          | PhoneNumber | Department |
        | Bill      | Billington | bill@bill.bill | 1           | Billing    |
    And Last Updated has updated on the MarketingContact for solution Sln1

@3655
Scenario: Contacts are removed when contacts previously existed
Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales      |
        | Sln1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Giraffe    |
    When a PUT request is made for empty solution Sln1 contact details
    Then a successful response is returned
    And No contacts exist for solution Sln1

@3655
Scenario: Contacts are not added when they have only null values
Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales      |
        | Sln1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Giraffe    |
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName | Email | PhoneNumber | Department |
        | NULL      | NULL     | NULL  | NULL        | NULL       |
        | NULL      | NULL     | NULL  | NULL        | NULL       |
    Then a successful response is returned
    And No contacts exist for solution Sln1

@3655
Scenario: Solution not found
    Given a Solution Sln2 does not exist
    When a PUT request is made for solution Sln2 contact details
        | FirstName | LastName   | Email          | PhoneNumber | Department |
        | Bill      | Billington | bill@bill.bill | 1           | Billing    |
    Then a response status of 404 is returned

@3655
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName   | Email          | PhoneNumber | Department |
        | Bill      | Billington | bill@bill.bill | 1           | Billing    |
    Then a response status of 500 is returned

@3655
Scenario: Solution id not present in request
    When a PUT request is made to update solution contact details with no solution id
        | FirstName | LastName   | Email          | PhoneNumber | Department |
        | Bill      | Billington | bill@bill.bill | 1           | Billing    |
    Then a response status of 400 is returned
