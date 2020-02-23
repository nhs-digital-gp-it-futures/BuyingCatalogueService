Feature: Buying Catalogue Service healthchecks
	As BuyingCatalogueService
	I want to be be able to check the health of my dependencies
    So that I can behave accordingly

@4821
Scenario: 1. DocumentAPI is healthy and Database is available
	Given The DocumentAPI is up
	And The Database server is available
	When the dependency health-check endpoint is hit
	Then the response will be Healthy

@4821
Scenario: 2. DocumentAPI is not healthy but Database is available
	Given The DocumentAPI is down
	And The Database server is available
	When the dependency health-check endpoint is hit
	Then the response will be Degraded

@4821d
Scenario: 3. DocumentAPI is healthy but Database is not available
	Given The DocumentAPI is up
	And The Database server is not available
	When the dependency health-check endpoint is hit
	Then the response will be Unhealthy

@4821d
Scenario: 4. DocumentAPI is not healthy and Database is not available
	Given The DocumentAPI is down
	And The Database server is not available
	When the dependency health-check endpoint is hit
	Then the response will be Unhealthy