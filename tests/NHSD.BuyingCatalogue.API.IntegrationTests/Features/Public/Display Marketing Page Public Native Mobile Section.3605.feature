Feature: Display Marketing Page Public Native Mobile Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName        | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline         | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription | FullDescription   | ClientApplication                                                                                                                                                                       |
        | Sln1     | Online Description | Online medicine 1 | { "ClientApplicationTypes" : [ "native-mobile"], "MobileOperatingSystems": { "OperatingSystems": ["Windows", "Linux"], "OperatingSystemsDescription": "For windows only version 10" }, "MobileConnectionDetails": { "ConnectionType": [ "3G", "4G" ], "MinimumConnectionSpeed": "1GBps", "Description": "A connecton detail description" } } |                                                                                                                                                                           

@3605
Scenario:1. Get Solution Public contains client application types native-mobile answers for all data
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution client-application-types section contains operating-systems
        | OperatingSystems |
        | Windows, Linux   |
   And the solution client-application-types section contains operating-systems-description with value For windows only version 10
   And the solution native-mobile mobile-connection-details section contains connection-type
        | ConnectionTypes |
        | 3G,4G           |
    And the solution native-mobile mobile-connection-details section contains minimum-connection-speed with value 1GBps
    And the solution native-mobile mobile-connection-details section contains connection-requirements-description with value A connecton detail description
    
Scenario:2. Get Solution Public contains client application types native-mobile answers for mobile connection details
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
   And the solution native-mobile mobile-connection-details section contains connection-type
        | ConnectionTypes |
        | 3G,4G           |
    And the solution native-mobile mobile-connection-details section contains minimum-connection-speed with value 1GBps
    And the solution native-mobile mobile-connection-details section contains connection-requirements-description with value A connecton detail description
