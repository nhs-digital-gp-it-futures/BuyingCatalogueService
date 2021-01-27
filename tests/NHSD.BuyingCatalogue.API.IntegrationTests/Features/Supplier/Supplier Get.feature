Feature: Get a single supplier
    As a buyer
    I want to retrieve the details of a supplier
    So that I can add the supplier information to the order form

Background:
    Given Suppliers exist
        | Id    | SupplierName | Address                                                                                                                                                                                                |
        | Sup 1 | Supplier A   | { "line1": "123 Line 1", "line2": "Line 2", "line3": "Line 3", "line4": "Line 4", "line5": "Line 5", "city": "Some town", "county": "Some county", "postcode": "LS15 1BS", "country": "Some country" } |
        | Sup 2 | Supplier B   | { "line1": "123 Line 1", "line2": "Line 2", "line3": "Line 3", "line4": "Line 4", "line5": "Line 5", "city": "Some town", "county": "Some county", "postcode": "LS15 1BS", "country": "Some country" } |
        | Sup 3 | Supplier C   | NULL                                                                                                                                                                                                   |
    And Supplier contacts exist
        | Id                                   | SupplierId | FirstName | LastName | Email              | PhoneNumber |
        | 17201621-07A5-47C9-82E9-7F9CD75CB71C | Sup 1      | Bob       | Smith    | bobsmith@email.com | 0123456789  |

@4621
Scenario: Get a supplier with a primary contact
    When a GET request is made to retrieve a supplier by ID Sup 1
    Then a successful response is returned
    And the response contains the following supplier details
        | SupplierId | Name       |
        | Sup 1      | Supplier A |
    And the response contains the following supplier address details
        | Line1      | Line2  | Line3  | Line4  | Line5  | Town      | County      | Postcode | Country      |
        | 123 Line 1 | Line 2 | Line 3 | Line 4 | Line 5 | Some town | Some county | LS15 1BS | Some country |
    And the response contains the following supplier primary contact details
        | FirstName | LastName | EmailAddress       | TelephoneNumber |
        | Bob       | Smith    | bobsmith@email.com | 0123456789      |

@4621
Scenario: Get a supplier without a primary contact
    When a GET request is made to retrieve a supplier by ID Sup 2
    Then a successful response is returned
    And the response contains the following supplier details
        | SupplierId | Name       |
        | Sup 2      | Supplier B |
    And the response contains the following supplier address details
        | Line1      | Line2  | Line3  | Line4  | Line5  | Town      | County      | Postcode | Country      |
        | 123 Line 1 | Line 2 | Line 3 | Line 4 | Line 5 | Some town | Some county | LS15 1BS | Some country |
    And the response does not contain a supplier primary contact

@4621
Scenario: Get a supplier without an address
    When a GET request is made to retrieve a supplier by ID Sup 3
    Then a successful response is returned
    And the response contains the following supplier details
        | SupplierId | Name       |
        | Sup 3      | Supplier C |
    And the response does not contain a supplier address

@4621
Scenario: Get a supplier that does not exist
    Given a Supplier Sup 4 does not exist
    When a GET request is made to retrieve a supplier by ID Sup 4
    Then a response status of 404 is returned

@4621
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made to retrieve a supplier by ID Sup 1
    Then a response status of 500 is returned
