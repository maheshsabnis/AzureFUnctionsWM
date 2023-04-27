# Date: 25-April-2023

1. COmplete Azure Function wth HttpTrigger to perform Http API operations i.e. GET/POST/PUT/DELETE on Department and EMployee
	- COmplete APIFunction.cs with Delete method
	- Complete All HTTP Method by adding new Azure FUnction with HttpTrigger doing GET/POST/PUT/DELETE  for Employee

# Date: 26-April-2023

1. Modify the HtpTrigger Function CReated on 25-April-2023 based on following requirements
	- WHen a new Employee is added, based on DeptNAme for the EMployee add the EMployee in Queue withe name as follows
		- queue-[deptname]
			- e.g. if deptname is 'IT', the the queuename will be queue-it
				- PLease not that queuename is always in lower case
2. Create a seperate Azure Function Project, that will use the QueueTrigger to read EMployee data from the queue for that department and store this data in Azure TableStorage / Database having seperate tables for Each Department


# Date: 27-APril-2023
1. Create a Durable FUncation (YOU DECIDE THE PATTERN) to Perform Following OPeration
	- F1: This will REad a local Json File that contains Products data as per following Schema
		- { ProductId, ProductName, Manufacturer, CatgoryNAme, Price}
	- F2: THis will be used to Accept one or more Products Information that is read use  F1 function and write this information into the Product Table of Database
	- F3: THis Function will receiving records from the Table and write it in seperate Queues Created based on Categoryame
		- E.g. If the CategoryName is 'Electronics', then the QueueName will be 'elecronicsq'




