# MoviesAPI

This API has been created as a solution to an assesment. I have tried using SOLID and DRY principles wherever i could.  There are some improvements that can be made but not done due to time limitation. See notes about them at the bottom.

## Tech Stack

- .Net core 3.1
- EF Core, SqlLite
- SeriLog
- REST,  SwaggerUI
- XUnit, Moq, InMemoryDatabase


## Running the project

The API is cloud-ready to be launched. Detailed API documentation using Swagger UI
**Swagger/documentation Url**: /help

#### Using Docker
Run below command on your local docker CLI

    docker build -t moviesapi https://github.com/piyusher/MoviesAPI.git && docker run -it -p 8087:80 moviesapi.
Once the container is running, go to: http://localhost:8087/help

#### Using Visual Studio
Clone the Git Repo, open the .sln file in visual studio and launch

## Some Insight
### Database and Data
- EF Code First, On startup, a SqlLite database gets created.
- Dummty data is setup and seeded to database from the file DataAccess/DataSeed/SeedData.json in the API project.
- The same data is also utilized to test the Repository layer using InMemoryDatabase.
- AvgRating is a calculated field in MovieEntity that maintains the overall average rating for a movie. Instead of calculating average rating everytime, this will keep the reads faster even during traffic. 

### More Details

- Structured logging in JSON format
- Detailed Swagger documentation using XML comments
- Model Validations
- Returning status codes as per REST guidelines and given API specifications.

## Reason for deviating from REST principles
*Endpoint to add/update Rating:* The API specification document mentions to create an API to add or update the rating. REST principles suggest to keep these operations separate as POST for insert and PUT for update. To adhere to API Specification, I created a POST that handles both insert and update.

## Future Improvements
- Deailed debug logging
- Dedicated filter/attribute to validate existence of user and movie in database. They can then be directly applied to models wherever validation is needed.
- May be even more consistent response models
- 100% code coverage
- Exception response modeling. For ex: Trying to add a movie name that already exists
- Integration tests
- Metrics implementation
- Authentication/Authorization


