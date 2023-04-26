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




