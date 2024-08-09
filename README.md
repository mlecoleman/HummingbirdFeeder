
# Hummingbird Feeder App

## Context and General Description
If you've never kept a hummingbird feeder before you may not be aware, but the sugar water must be changed out regularly to keep harmful mold from growing.  In the past I've used a manual process to see if I need to change my feeder that involves keeping track of daily temperature highs.  I'm terrible at keeping track of this and end up changing the feeder more frequently than I probably need to just to be safe.  Here's the chart I use to help me figure out if my Hummingbird Feeder needs changing:

![alt text](https://i.pinimg.com/originals/ea/ac/08/eaac08f6afd61debc79bc2ea720b5a87.jpg)

My App allows a user to enter in the zipcode of their feeder's location along with the last time they changed it out, and using data returned from a weather API my app will automatically calculate if the feeder needs to be changed out or not.  It also uses a SQLite database to store the user's feeder information for multiple feeders.

## Running The App & FYIs

This app uses:
.NET 7.0
NUnit and Bunit for Unit Tests
Blazor
SQLite

To run the app it should be able to be cloned locally, and as long as you have .NET 7.0 you should be able to run it from your IDE and the Blazor page should open.  In case any dependencies need to be manually added, here is everything this project is currently referencing:

Microsoft.EntityFrameworkCore.Sqlite Version 7.0.20 <br/>
Microsoft.EntityFrameworkCore.Tools Version 7.0.20 <br/>
Microsoft.EntityFrameworkCore.Design Version 7.0.20 <br/>
Microsoft.EntityFrameworkCore Version 7.0.20 <br/>
Microsoft.NET.Test.Sdk Version 17.5.0 <br/>
NUnit Version 3.13.3 <br/>
NUnit3TestAdapter Version 4.2.1 <br/>
NUnit.Analyzers Version 3.6.1 <br/>
coverlet.collector Version 3.2.0 <br/>
bunit.web Version 1.30.3 <br/>
bunit Version 1.30.3 <br/>
FluentAssertions Version=6.12.0 <br/>

## How To Use The App

### Home Page Features
- About Link in upper right corner links to the GitHub repo for this project
- Hummingbird Feeder Table
  - Each row displays data for each of the User's feeders
  - Feeder Name - Displays the name of the feeder set by the user i.e. Front Yard
  - Zipcode - Displays the Zipcode where the feeder is located
  - Last Change Date - Displays the last date the feeder was changed
  - Quick Update - Allows the User to click the Feeder Changed Today button to quickly update the change date to today's date and immediately see any data for that feeder get re-calculated
  - Check Feeder Status - Indicates if the feeder for that row is due to be changed
  - Edit - Displays a button that on-click allows the user to edit the Feeder's name, location, or change date
  - Delete - Displays a button that on-click allows the user to delete the given feeder
- Add New Feeder Button - On click redirects to a page which allows the user to create a new feeder
- Daily Highs Table(s)
  - For each feeder a table is displayed that shows each date since the feeder was changed, and the high temperature for that day
  -  If a feeder was changed over 7 days ago, the table will not be populated. Helper text is included for each table indicating that feeders should never be left unchanged for more than 7 days.

### Edit & Create Page Features
- Feeder Name Field
	- Allows for a string of letters, numbers, or spaces to be entered and can be left blank
- Zip Code Field
	- Does not allow any input that isn't 5 digits
	- Uses the weather API to validate the zip and will display an error message if a 400 error is returned for an invalid zip code
- Date Field
	- Does not allow for a date in the future
	- Does not allow invalid dates
- Save button
	- Either updates an existing feeder record (Edit page) or saves a new feeder record (Create page)
- Cancel button
	- Returns to the Home page without saving or updating any feeder record  

## Requirements & How They Are Met

### GitHub Repository and Commits

> -   Upload your project to GitHub repository with a minimum of 10 distinct commits.
    
>-   Uploading via Git commands demonstrates regular usage rather than a single final upload.

- [x] ***As of the writing of this readme on 08/04 this repo has over 30 commits***

-----

### README File

>-   Include a README file explaining your project.
    
>-   Describe your project in a paragraph or more.
    
>-   Identify 3+ features from the provided list that you've integrated.
    
>-   Add any special instructions for the reviewer to run your project.

- [x] ***You're lookin at it! :)***

-----

### Visual Appeal

>-   Design your project to be visually appealing; follow industry trends.
    
>-   Make sure all text is spelled correctly and legibly.
    
>-   Aim to create projects that employers find attractive and engaging.
    
>-   Explore other applications and websites for inspiration. Emulate styles and functionalities you find compelling.
    
>-   Select a color palette and font stack to enhance design consistency.

- [x] ***This app uses blazor with bootstrap to create visual appeal.  Notice the cute Hummingbird favicon too!*** 

-----

### Web-based application

> Your application must be a web-based app using MVC, Razor pages, Blazor, or Single Page Applications (SPAs).

 - [x] ***This app is a Blazor web-based app.***

-----

### APIs

> Integrate a student-created or hosted API into the project. (Example: Get and display weather information and save it to a SQLite database or JSON file.)

- [x] ***Hummingbird App uses an api from www.weatherapi.com to retrieve temperature highs from the previous week in order to calculate if the user's feeder needs to be changed out.***

-----

### Database Interaction (SQLite):

> Develop at least one class (excluding the default class in a new
project), create an object of that class, populate it with data from a
database, and incorporate the data in your application. A minimum of one table (entity) should be utilized. Note that classes should be created even when using object-relational mappers like Entity Framework.

- [x] ***Hummingbird uses a Feeder object located in Feeder.cs that is populated from the Feeders.db table in the database. The data from this database table is then used to calculate when a user's feeder should be changed.***

-----

### Functions/Methods

> Create and utilize a minimum of 3 functions or methods, with at least one returning a value integral to your application.

- [x] ***This Hummingbird app contains well over 10 methods.  An example of one returning a value integral to the application can be found in the *Index.razor.cs* file on line 84 in method *GetTempMaxPerDayFromWeatherApi*.  This method returns the maxTemp of a certain date from the weather API which is integral to the calculation of when the feeder should be changed.***

-----

### Software Development Capstone Features List

>Choose at least 3 items from the FEATURES LIST and integrate them into your project. While 3 is the minimum, consider adding more to account for unexpected issues.

|                |Feature Implemented                          |Location                         |
|----------------|-------------------------------|-----------------------------|
|1|Create 3 or more unit tests for your application| The Tests folder contains 3 NUnit tests for the Index page|
|2|Implement a regular expression (regex) to validate or ensure a field is always stored and displayed in the correct format|In Create.razor (lines 13 & 18) and Update.razor (lines 19 & 24) there are regular expressions used for the zipcode field that requires the input to be 5 digits and for the Feeder name field that restricts inputs to letters, numbers, or spaces.|
|3|Create a dictionary or list, populate it with several values, retrieve at least one value, and use it in your program|In the Index.razor.cs file I populate a dictionary using dates as the key and daily highs as the value (See method GetDatesSinceLastChangeDate - Line 67). These are then used to calculate if the feeder needs to be changed and are displayed to the UI (See method DoesFeederNeedToBeChanged - Line 95 and see the table starting on line 66 of Index.razor).|
|4|Make your application asynchronous|Most of the methods in this app are async methods
