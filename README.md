# Hummingbird Feeder App

## Context and General Description
If you've never kept a hummingbird feeder before you may not be aware, but the sugar water must be changed out regularly to keep pathogens from growing.  In the past I've used a manual process to see if I need to change my feeder that involves keeping track of daily temperature highs.  I'm terrible at keeping track of this and end up changing the feeder more frequently than I probably need to just to be safe.  Here's the chart I would use every day to help me figure out if my Hummingbird Feeder needed changing:

![alt text](https://i.pinimg.com/originals/ea/ac/08/eaac08f6afd61debc79bc2ea720b5a87.jpg)

My App allows a user to enter in the zipcode of their feeder along with the last time they changed it out, and using data returned from a weather API my app will automatically calculate if the feeder needs to be changed out or not.  It also uses a SQLite database to store the user's feeder information for multiple feeders.

## Running The App & FYIs

This app uses:
.NET 7.0
NUnit and Bunit for Unit Tests
Blazor
SQLite

To run the app it should be able to be cloned locally, and as long as you have .NET 7.0 you should be able to run it from your IDE and the Blazor page should open.  In case any dependencies need to be manually added, here is everything this project is currently referencing:

Microsoft.EntityFrameworkCore.Sqlite Version 7.0.20
Microsoft.EntityFrameworkCore.Tools Version 7.0.20
Microsoft.EntityFrameworkCore.Design Version 7.0.20
Microsoft.EntityFrameworkCore Version 7.0.20
Microsoft.NET.Test.Sdk Version 17.5.0 
NUnit Version 3.13.3
NUnit3TestAdapter Version 4.2.1
NUnit.Analyzers Version 3.6.1
coverlet.collector Version 3.2.0
bunit.web Version 1.30.3
bunit Version 1.30.3
FluentAssertions Version=6.12.0

## Requirements & How They Are Met

### GitHub Repository and Commits

> -   Upload your project to GitHub repository with a minimum of 10 distinct commits.
    
>-   Uploading via Git commands demonstrates regular usage rather than a single final upload.

As of the writing of this readme on 07/31 this repo has over 20 commits

### README File

>-   Include a README file explaining your project.
    
>-   Describe your project in a paragraph or more.
    
>-   Identify 3+ features from the provided list that you've integrated.
    
>-   Add any special instructions for the reviewer to run your project.

You're lookin at it! :)

### Visual Appeal

>-   Design your project to be visually appealing; follow industry trends.
    
>-   Make sure all text is spelled correctly and legibly.
    
>-   Aim to create projects that employers find attractive and engaging.
    
>-   Explore other applications and websites for inspiration. Emulate styles and functionalities you find compelling.
    
>-   Select a color palette and font stack to enhance design consistency.

This app uses blazor with bootstrap to create visual appeal.  Notice the cute Hummingbird favicon too! 

### Web-based application

> Your application must be a web-based app using MVC, Razor pages, Blazor, or Single Page Applications (SPAs).

This app is a Blazor web-based app.

### APIs

> Integrate a student-created or hosted API into the project. (Example: Get and display weather information and save it to a SQLite database or JSON file.)

Hummingbird App uses an api from www.weatherapi.com to retrieve temperature highs from the previous week in order to calculate if the user's feeder needs to be changed out.

### Database Interaction (SQLite):

> Develop at least one class (excluding the default class in a new
project), create an object of that class, populate it with data from a
database, and incorporate the data in your application. A minimum of one table (entity) should be utilized. Note that classes should be created even when using object-relational mappers like Entity Framework.

Hummingbird uses a Feeder object located in Feeder.cs that is populated from the Feeders.db table in the database. The data from this database table is then used to calculate when a user's feeder should be changed.

### Functions/Methods

> Create and utilize a minimum of 3 functions or methods, with at least one returning a value integral to your application.

This Hummingbird app contains over 10 methods.  An example of one returning a value integral to the application can be found in method *GetTempMaxPerDayFromWeatherApi* which located in the *Index.razor.cs* file.  This method returns the maxTemp of a certain date from the weather API which is integral to the calculation of when the feeder should be changed.

### Software Development Capstone Features List

>Choose at least 3 items from the FEATURES LIST and integrate them into your project. While 3 is the minimum, consider adding more to account for unexpected issues.

|                |Feature Implemented                          |Location                         |
|----------------|-------------------------------|-----------------------------|
|1|Create 3 or more unit tests for your application| The Tests folder contains NUnit tests for the Index page|
|2|Implement a regular expression (regex) to validate or ensure a field is always stored and displayed in the correct format|In the Create.cs file there's a regular expression for the zipcode field that requires the input to be 5 digits|
|3|Create a dictionary or list, populate it with several values, retrieve at least one value, and use it in your program|In the Index.razor.cs file I populate two lists with values and use them to calculate the feeder change date, please see methods GetListOfDatesSinceLastChangeDate and GetListOfTemperatureMaxPerDate|
|4|Make your application asynchronous|Most of the methods in this app are async methods
