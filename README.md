# TvMaze Scraper and API

This is my entry for the TvMaze assignment. Let me quickly explain some of the choices I made in building this.  

For simplicity, I chose to work EF Core InMemoryDb. This means that every time the app is started, the database is empty. 

Another caveat of using InMemoryDb is that two .Net processes cannot use the same context. This is the reason I put the Scraper inside the API project. 
Normally this would be a separate project and application, that would share Data Access code with the API, and so read and write to the same database instance.

When the project is started, the scraper starts calling the TvMaze API. To avoid the rate limit, 20 calls are distributed over 10 seconds. 
This means it takes about 10 seconds for the first data to be queryable from the API endpoint. 
Data will be continually added in the background and become available for the API. 

I radically simplified the pagination to save time, IRL this could be a lot more fully featured, but I felt I spent enough time. 

## To run it:

- Clone the repo
- Restore the nuget packages
- Run from visual studio with the Docker launch profile

A browser should open and you should be directed to a Swagger page. You can try out the API from there by clicking on the `/Shows` endpoint and then the `Try it out` button
Alternatively you can simply navigate to `localhost:<yourport>/Shows?pageNumber=0`. 

Again, be aware that data is constantly arriving in the background. So navigating to page 8 right at the start will give an empty result. 
