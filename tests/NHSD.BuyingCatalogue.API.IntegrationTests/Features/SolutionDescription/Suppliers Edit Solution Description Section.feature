Feature: Suppliers Edit Solution Description Section
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
        | Sln2       | UrlSln2  | Eye opening experience         | Eye opening6        | [ "Workflow", "Referrals" ]       |

@1843
Scenario: Solution description section data is updated
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                | Description            | Link       |
        | New type of medicine 4 | A new full description | UrlSln1New |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
    And solutions have the following details
        | SolutionId | AboutUrl   | SummaryDescription      | FullDescription        | Features                          |
        | Sln1       | UrlSln1New | New type of medicine 4  | A new full description | [ "Appointments", "Prescribing" ] |
        | Sln2       | UrlSln2    | Eye opening experience  | Eye opening6           | [ "Workflow", "Referrals" ]       |
    And Last Updated has been updated for solution Sln1

@1843
Scenario: Solution description section data is updated with trimmed whitespace
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                                    | Description                               | Link                 |
        | "           New type of medicine 4       " | "      A new full description           " | "    UrlSln1New    " |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
    And solutions have the following details
        | SolutionId | AboutUrl   | SummaryDescription      | FullDescription        | Features                          |
        | Sln1       | UrlSln1New | New type of medicine 4  | A new full description | [ "Appointments", "Prescribing" ] |
        | Sln2       | UrlSln2    | Eye opening experience  | Eye opening6           | [ "Workflow", "Referrals" ]       |
    And Last Updated has been updated for solution Sln1

@1828
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a PUT request is made to update the solution-description section for solution Sln3
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New |
    Then a response status of 404 is returned

@1828
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the solution-description section for solution Sln1
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln1New |
    Then a response status of 500 is returned

@1828
Scenario: Solution id not present in request
    When a PUT request is made to update the solution-description section with no solution id
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSlnNew |
    Then a response status of 400 is returned
