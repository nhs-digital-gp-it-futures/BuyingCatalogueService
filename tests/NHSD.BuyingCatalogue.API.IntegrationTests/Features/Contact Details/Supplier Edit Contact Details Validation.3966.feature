Feature:  Supplier Edit Contact Details Validation
    As a Supplier
    I want to update the Solution Contact information
    So that I can modify who the Solution Contacts are

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription | Features                          |
        | Sln1     | UrlSln1  | The best solution  | [ "Appointments", "Prescribing" ] |

@3966
Scenario: 1. FirstName exceeds the character length
    When a PUT request is made for solution Sln1 contact details
        | FirstName                  | LastName   | Email       | PhoneNumber  | Department |
        | A string with length of 36 | Bobbington | bob@bob.bob | 66666 666666 | Tech       |
    Then a response status of 400 is returned
    And the contact-1.first-name field value is the validation failure maxLength
    And No contacts exist for solution Sln1

@3966
Scenario: 2. LastName exceeds the character length
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName                   | Email       | PhoneNumber  | Department |
        | Bob       | A string with length of 36 | bob@bob.bob | 66666 666666 | Tech       |
    Then a response status of 400 is returned
    And the contact-1.last-name field value is the validation failure maxLength
    And No contacts exist for solution Sln1

@3966
Scenario: 3. Email exceeds the character length
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName   | Email                       | PhoneNumber  | Department |
        | Bob       | Bobbington | A string with length of 256 | 66666 666666 | Tech       |
    Then a response status of 400 is returned
    And the contact-1.email-address field value is the validation failure maxLength
    And No contacts exist for solution Sln1

@3966
Scenario: 4. PhoneNumber exceeds the character length
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName   | Email       | PhoneNumber                 | Department |
        | Bob       | Bobbington | bob@bob.bob | A string with length of 256 | Tech       |
    Then a response status of 400 is returned
    And the contact-1.phone-number field value is the validation failure maxLength
    And No contacts exist for solution Sln1

@3966
Scenario: 5. Department name exceeds the character length
    When a PUT request is made for solution Sln1 contact details
        | FirstName | LastName   | Email       | PhoneNumber  | Department                 |
        | Bob       | Bobbington | bob@bob.bob | 66666 666666 | A string with length of 51 |
    Then a response status of 400 is returned
    And the contact-1.department-name field value is the validation failure maxLength
    And No contacts exist for solution Sln1

@3966
Scenario: 6. FirstName, LastName, Email, PhoneNumber and Department name exceeds the character length
    Given MarketingContacts exist
        | SolutionId | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Sln1       | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales      |
        | Sln1       | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Giraffe    |
    When a PUT request is made for solution Sln1 contact details
        | FirstName                  | LastName                   | Email                       | PhoneNumber                | Department                 |
        | Bob                        | Bobbington                 | bob@bob.bob                 | 66666 666666               | Sales                      |
        | A string with length of 36 | A string with length of 36 | A string with length of 256 | A string with length of 36 | A string with length of 51 |
    Then a response status of 400 is returned
    And the contact-2.first-name field value is the validation failure maxLength
    And the contact-2.last-name field value is the validation failure maxLength
    And the contact-2.email-address field value is the validation failure maxLength
    And the contact-2.phone-number field value is the validation failure maxLength
    And the contact-2.department-name field value is the validation failure maxLength
    And MarketingContacts exist for solution Sln1
        | FirstName | LastName   | Email         | PhoneNumber  | Department |
        | Bob       | Bobbington | bob@bob.bob   | 66666 666666 | Sales      |
        | Betty     | Bobbington | betty@bob.bob | 99999 999999 | Giraffe    |
