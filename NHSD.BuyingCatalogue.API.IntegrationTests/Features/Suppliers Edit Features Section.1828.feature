@ignore
Feature: Suppliers Edit Features Section
    As a Supplier
    I want to Edit the Features Section
    So that I can make sure the information is correct

@1828
Scenario: 1. Feature does not exceed maximum
    Given the Supplier has entered a Feature
    And it does not exceed the maximum character count
    When the Supplier attempts to save 
    Then the Section is saved

@1828
Scenario: 2. Feature does exceed maximum
    Given the Supplier has entered a Feature
    And it does exceed the maximum character count
    When the Supplier attempts to save 
    Then the Section is not saved 
    And an indication is given to the Supplier as to why

@1828
Scenario: 3. Features Section marked as Complete No Mandatory Data
    Given the Features Section has no Mandatory Data
    And a Supplier has saved any data on the Features Section
    When the Marketing Page Form is presented 
    Then the Features Section is marked as Complete

@1828
Scenario: 4. Features Section marked as Incomplete No Data
    Given the Features Section has no Mandatory Data
    And a Supplier has not saved any data on the Features Section
    When the Marketing Page Form is presented 
    Then the Features Section is marked as Incomplete
