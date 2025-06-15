# SmoothStackTest
1. The solution is made up of 2 projects
	1. SStack -> This projects hosts the Client and API services for simplicity (Typically on production the services would on a different project)
	2. SSTackTests -> Contains the units Tests
2. API Documentation -> I installed swagger on SStack projects for API documentation and run perfectly fine

https://localhost:44366/swagger

3. Performance Optimization
I considered create a non clustered Index on CustomerOrderItem DB table to improve query execution time
	CREATE NONclustered INDEX OrderId_ProductId ON dbo.CustomerOrderItem (ProductId asc, CustomerOrderId asc);

4. Database - I used MS SQL as DB, the schema script file in on DBFiles directory
	Once setup, the application connection string (inside web config) will need to be changed to reflect the server host
5. ORM -> I used EntityFramework 6.0

NB/ For time purposes I did not manage to include JWT based authentication
