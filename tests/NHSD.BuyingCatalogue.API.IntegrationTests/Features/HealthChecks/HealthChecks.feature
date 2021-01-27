Feature: Buying Catalogue Service healthchecks
    As BuyingCatalogueService
    I want to be be able to check the health of my dependencies
    So that I can behave accordingly

# TODO: Make sure scenarios can be run in any order (currently B will fail if run last)

@4821
Scenario: A. Document API is healthy and Database is available
    Given The document api is up
    And The Database server is available
    When the dependency health-check endpoint is hit
    Then the response will be Healthy

@4821
Scenario: B. Document API is not healthy but Database is available
    Given The document api is down
    And The Database server is available
    When the dependency health-check endpoint is hit
    Then the response will be Degraded

@4821
Scenario: C. Document API is healthy but Database is not available
    Given The document api is up
    And The Database server is not available
    When the dependency health-check endpoint is hit
    Then the response will be Unhealthy

@4821
Scenario: D. Document API is not healthy and Database is not available
    Given The document api is down
    And The Database server is not available
    When the dependency health-check endpoint is hit
    Then the response will be Unhealthy
